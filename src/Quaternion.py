import numpy as np
from numba import jit

class Quaternion:
    @jit
    def __init__(self, r: float, v: np.array):
        self.v = v
        self.r = r
    
    def conj(self):
        self.v = -self.v
        return self
    
    @jit
    def norm(self):
        total = self.distance()
        self.v = self.v / total
        self.r = self.r / total
        return self
    
    @jit
    def distance(self):
        total = 0.
        for i in range(3):
            total += self.v[i] * self.v[i]
        total += self.r * self.r
        return np.sqrt(total)

    def inverse(self):
        return self.conj()
    
    @jit
    def __mul__(self, q: np.array):
        t = self.r * q.r - np.dot(self.v, q.v)
        v = self.r * q.v + q.r * self.v + np.cross(self.v, q.v)
        return Quaternion(t, v)

    @jit
    def rotation(self, p: np.array, theta: float):
        cos = np.cos(theta * 0.5)
        sin = np.sin(theta * 0.5)
        P = Quaternion(0, p)
        Q = Quaternion(cos, self.v * sin)
        R = Quaternion(cos, self.v * -sin)
        return (Q * P * R).v