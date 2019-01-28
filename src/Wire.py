from typing import List
import numpy as np
from numba import jit
from scipy.integrate import dblquad
from Coil import Coil
from Quaternion import Quaternion

class Wire:
    def __init__(self, time: float, positions: np.array, deltaDim:List[int]):
        self.time =  time
        self.positions = positions
        self.deltaDim = deltaDim
        self.fluxDensity = 0.
        self._sigma = 1.

    @jit
    def _quadCoil(self, dr: float, dt: float, coil: Coil):
        fracUp = self._sigma * dr
        q = Quaternion(0, coil.front)
        fracDownVec = dr * q.rotation(dt, coil.right) + self.positions - coil.position
        fracDownVal = np.linalg.norm(fracDownVec)
        fracDownVal = fracDownVal * fracDownVal * fracDownVal
        return fracUp / fracDownVal

    def FluxDensity(self, sigma: float, coils: List[Coil]):
        self.fluxDensity = 0. 
        self._sigma = sigma

        for coil in coils:
            self.fluxDensity += dblquad(self._quadCoil, 0, 2.*np.pi, lambda x: 0, lambda x: coil.radius, (coil))[0]

        return self.fluxDensity
