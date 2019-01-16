import numpy as np

class Quaternion:
    def __init__(self, r: float, v: np.array):
        self._v = v
        self._r = r
    
    def conj(self):
        self._v = -self._v
        return self
    
    def norm(self):
        total = 0.
        for i in range(3):
            total += self._v[i] * self._v[i]
        total += self._r * self._r
        self._v = self._v / total
        self._r = self._r / total
    
    def inverse(self):
        pass
        