# pylint: disable=W0611, W0106
import sys
from typing import List
from numba import jit, prange, f8, f4, i4
import numpy as np 
from time import time
import matplotlib.pyplot as plot
from wired_flux_density import wired_flux_density_on_coils

@jit(f8(f8[:], f8[:], f8[:]), nogil=True, nopython=True)
def compute_fraction(measure_point: np.ndarray, wire_position: np.ndarray, coil_forward: np.ndarray):
    fracUpV = np.dot(coil_forward, wire_position - measure_point) * coil_forward
    fracUp = np.linalg.norm(fracUpV)
    fracDown = np.linalg.norm(wire_position - measure_point)
    fracDown = fracDown * fracDown * fracDown

    if fracDown == 0.0:
        return 0.0
    return fracUp / fracDown

@jit(f8(f8[:], f8[:], i4, f8[:,:]), nogil=True, nopython=True)
def field_flux_density_inner_product_coil(measure_point: np.ndarray, wire_position: np.ndarray, coil_count: int, coil_forwards: np.ndarray):
    total = 0.
    for ci in range(coil_count):
        total += compute_fraction(measure_point, wire_position, coil_forwards[ci])
    return total

@jit(f8(f8[:], i4, f8[:,:], f8[:], i4, f8[:,:], f8), nogil=True, nopython=True)
def field_flux_density_inducted_wire(measure_point: np.ndarray, wire_count: int, wire_positions: np.ndarray, wired_flux_densities: np.ndarray, coil_count: int, coil_forwards: np.ndarray, gamma: float):
    total = 0.
    for i in range(wire_count):
        result = field_flux_density_inner_product_coil(measure_point, wire_positions[i], coil_count, coil_forwards)
        total += result * wired_flux_densities[i] * gamma
    return total

def field_flux_density(origin: np.ndarray, field_size: np.ndarray, field_delta: np.ndarray, wire_count: int, wire_positions: np.ndarray, field_flux_densities: np.ndarray, coil_count: int, coil_forwards: np.ndarray, gamma: float):
    numof_size = (field_size / field_delta).astype("i4")
    field_fluxes = np.zeros(numof_size, dtype="f8")

    for x in range(numof_size[0]):
        for y in range(numof_size[1]):
            for z in range(numof_size[2]):
                measure_point = np.array([x, y, z], dtype="f8") * field_delta + origin
                field_fluxes[x][y][z] = field_flux_density_inducted_wire(measure_point, wire_count, wire_positions, field_flux_densities, coil_count, coil_forwards, gamma)

    return field_fluxes

@jit(f8[:,:](f8[:], f8[:], f8[:], f8[:], f8[:], i4, f8[:,:], f8[:], i4, f8[:,:], f8), nogil=True, parallel=True)
def plane_flux_density(origin: np.ndarray, planeX: np.ndarray, planeY: np.ndarray, plane_size: np.ndarray, plane_delta: np.ndarray, wire_count: int, wire_positions: np.ndarray, wired_flux_densities: np.ndarray, coil_count: int, coil_forwards: np.ndarray, gamma: float):
    numof_size = (plane_size / plane_delta).astype("i4")
    plane_fluxes = np.zeros(numof_size, dtype="f8")

    for x in prange(numof_size[0]):
        for y in range(numof_size[1]):
            measure_point = (planeX * x * plane_delta[0] + planeY * y * plane_delta[0]) + origin
            total = field_flux_density_inducted_wire(measure_point, wire_count, wire_positions, wired_flux_densities, coil_count, coil_forwards, gamma)
            plane_fluxes[y][x] = total
    
    return plane_fluxes


def main():
    wire_count = 100
    wire_positions = np.array([[0.5*x - 25., 0., 5.] for x in range(wire_count)], dtype="f8")

    coil_count = 2
    coil_positions = np.array([[0, -3, 0], [0, 3, 0]], dtype="f8")
    coil_forwards = np.array([[0, 0, 1] for x in range(coil_count)], dtype="f8")
    coil_rights = np.array([[1, 0, 0] for x in range(coil_count)], dtype="f8")
    coil_heights = np.array([1 for x in range(coil_count)], dtype="f8")
    coil_radius = np.array([1 for x in range(coil_count)], dtype="f8")

    origin = np.array([-25., 0., -25.], dtype="f8")
    xvec = np.array([1., 0., 0.], dtype="f8")
    yvec = np.array([0., 0., 1.], dtype="f8")
    size = np.array([50., 50.], dtype="f8")
    delta = np.array([0.5, 0.5], dtype="f8")

    fluxes = wired_flux_density_on_coils(wire_count, wire_positions, coil_count, coil_positions, coil_forwards, coil_rights, coil_heights, coil_radius, 1.)
    print("--- start measurement ---")

    start = time()
    heatmap = plane_flux_density(origin, xvec, yvec, size, delta, wire_count, wire_positions, fluxes, coil_count, coil_forwards, 1.)
    elapsed = time() - start

    print(elapsed)

    plot.imshow(heatmap, interpolation="nearest", vmin=np.min(heatmap), vmax=np.max(heatmap), cmap="jet", origin="lower")
    plot.xlabel("-> x")
    plot.ylabel("-> z")
    plot.colorbar()
    plot.show()

if __name__ == "__main__":
    main()