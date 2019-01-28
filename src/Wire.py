from typing import List
import numpy as np
from numba import jit
from scipy.integrate import dblquad
from src.Coil import Coil
from src.Quaternion import Quaternion

class Wire:
    def __init__(self, positions: np.array):
        self.positions = positions
        self.fluxDensity = 0.
        self._sigma = 1.

    def _quadCoil(self, dr: float, dt: float, coil: Coil):
        fracUp = self._sigma * dr

        q = Quaternion(0, coil.front)
        rotated = dr * q.rotation(coil.right, dt)

        fracDownVec = rotated + self.positions - coil.position
        fracDownVal = np.linalg.norm(fracDownVec)
        fracDownVal = fracDownVal * fracDownVal * fracDownVal
        return fracUp / fracDownVal

    def FluxDensity(self, sigma: float, coils: List[Coil]):
        self.fluxDensity = 0. 
        self._sigma = sigma

        for coil in coils:
            self.fluxDensity += dblquad(self._quadCoil, 0, 2.*np.pi, lambda x: 0, lambda x: coil.radius, args=[coil])[0]

        return self.fluxDensity
