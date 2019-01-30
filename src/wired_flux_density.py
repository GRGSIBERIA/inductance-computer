# pylint: disable=E1133, W0621
from typing import List
from time import time
from numba import jit, njit, f8, i4, f4, prange
import numpy as np 
from scipy.integrate import nquad

@njit(nogil=True, cache=True, fastmath=True)
def norm3(vec):
    total = vec[0] * vec[0] + vec[1] * vec[1] + vec[2] * vec[2]
    return np.sqrt(total)

@njit(f8(f8[:], f8, f8[:], f8), nogil=True, cache=True)
def mul_quaternion_R(Qi, Qr, Pi, Pr):
    """クォータニオンの積，実数部を返す"""
    return Qr * Pr - (Qi[0] * Pi[0] + Qi[1] * Pi[1] + Qi[2] * Pi[2])

@njit(nogil=True, cache=True, fastmath=True)
def mul_quaternion_I(Qi, Qr, Pi, Pr):
    """クォータニオンの積，虚数部を返す"""
    array = [Qi[1] * Pi[2] - Qi[2] * Pi[1], Qi[2] * Pi[0] - Qi[0] * Pi[2], Qi[0] * Pi[1] - Qi[1] * Pi[0]]
    return Qr * Pi + Pr * Qi + np.array(array)

@njit(f8(f8, f8, f8[:], f8[:], f8[:], f8[:], f8), nogil=True, cache=True, fastmath=True)
def double_quad(dr: float, dt: float, wire_position: np.ndarray, coil_position: np.ndarray, coil_forward: np.ndarray, coil_right: np.ndarray, sigma: float):
    # クォータニオンの計算
    cos = np.cos(dt)
    sin = np.sin(dt)

    Qi = coil_forward * sin
    Qr = cos
    Ri = coil_forward * -sin 
    Rr = cos
    Pi = coil_right
    Pr = 0.

    # Q * P * R -> B
    Ar = mul_quaternion_R(Qi, Qr, Pi, Pr)
    Ai = mul_quaternion_I(Qi, Qr, Pi, Pr)
    Bi = mul_quaternion_I(Ai, Ar, Ri, Rr)   # 虚数部のみを使うので実数部は省略する
    
    # 関数内部の計算
    frac_up = dr * sigma
    quat = dr * Bi 
    frac_down_vec = quat + wire_position - coil_position
    frac_down = norm3(frac_down_vec)
    frac_down = frac_down * frac_down * frac_down
    return frac_up / frac_down

@jit(nogil=True, cache=True, fastmath=True)
def wired_flux_density(wire_position: np.ndarray, coil_position: np.ndarray, coil_forward: np.ndarray, coil_right: np.ndarray, coil_height: float, coil_radius: float, sigma: float):
    quadargs1 = (wire_position, coil_position + coil_forward * coil_height * 0.5, coil_forward, coil_right, sigma)
    quadargs2 = (wire_position, coil_position - coil_forward * coil_height * 0.5, coil_forward, coil_right, sigma)
    
    top = nquad(double_quad, [[0., 2.*np.pi], [0., coil_radius]], args=quadargs1)[0]
    under = nquad(double_quad, [[0., 2.*np.pi], [0., coil_radius]], args=quadargs2)[0]
    return top - under

@jit(parallel=True, nogil=True)
def wired_flux_density_on_coils(wire_count: int, wire_positions: np.ndarray, coil_count: int, coil_positions: np.ndarray, coil_forwards: np.ndarray, coil_rights: np.ndarray, coil_heights: List[float], coil_radius: List[float], sigma: float):
    totals = []
    for wi in prange(wire_count):
        total = 0.
        for ci in range(coil_count):
            total += wired_flux_density(wire_positions[wi], coil_positions[ci], coil_forwards[ci], coil_rights[ci], coil_heights[ci], coil_radius[ci], sigma)
        totals.append(total)
    return np.array(totals)


def main():
    wire_count = 100
    wire_positions = np.array([[0.5*x - 25., 0., 5.] for x in range(wire_count)], dtype="f8")

    coil_count = 2
    coil_positions = np.array([[0, -3, 0], [0, 3, 0]], dtype="f8")
    coil_fronts = np.array([[0, 0, 1] for x in range(coil_count)], dtype="f8")
    coil_rights = np.array([[1, 0, 0] for x in range(coil_count)], dtype="f8")
    coil_heights = np.array([1 for x in range(coil_count)], dtype="f8")
    coil_radius = np.array([1 for x in range(coil_count)], dtype="f8")

    start = time()
    fluxes = wired_flux_density_on_coils(wire_count, wire_positions, coil_count, coil_positions, coil_fronts, coil_rights, coil_heights, coil_radius, 1.)
    elapsed = time() - start

    print(fluxes)
    print(elapsed)

    import matplotlib.pyplot as plot 

    plot.plot(fluxes)
    plot.show()

if __name__ == "__main__":
    main()