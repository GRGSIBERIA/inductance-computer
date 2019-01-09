# 作成者：竹渕瑛一
from typing import List
import numpy as np
import quaternion
from scipy.integrate import dblquad

# 測定場のクラス
class Field:
    def __init__(self, width, height, depth):
        self.width = width
        self.height = height
        self.depth = depth

# コイルのクラス
class Coil:
    def __init__(self, position: np.array, forward: np.array, right: np.array, height: float, radius: float):
        self.position = position
        self.forward = forward
        self.right = right
        self.height = height
        self.radius = radius
        self.sigma = 1
        self.gamma = 1
        self.halfHeight = height / 2.
        self.top = self.position + self.forward * self.halfHeight
        self.bottom = self.position - self.forward * self.halfHeight

class Wire:
    def __init__(self, position):
        self.position = position
        self.fluxDensity = 0.

class FluxDensity:
    def __init__(self, coil: Coil, wire: Wire):
        self.coil = coil 
        self.wire = wire

    def _fracUp(self, r):
        return self.coil.sigma * r
    
    def _fracDown(self, dtheta: float, dr: float, coilPosition: np.array):
        q = np.quaternion(dtheta, self.coil.forward[0], self.coil.forward[1], self.coil.forward[2])
        q = 1. / q.absolute() * q
        return pow(abs(dr * q * self.coil.right + self.wire.position - coilPosition), 3.)

    # ワイヤの磁束密度を求める
    def _integrateBw(self, dtheta: float, dr: float):
        return \
            self._fracUp(dr) / self._fracDown(dr, dtheta, self.coil.top) - \
            self._fracUp(dr) / self._fracDown(dr, dtheta, self.coil.bottom)

    # ワイヤの磁束密度を計算する
    def Bw(self):
        self.wire.fluxDensity = dblquad(self._integrateBw, 0, 2.*np.pi, lambda dr: 0, lambda dr: self.coil.radius)
        return self.wire.fluxDensity
    
    # 測定点の磁束密度を計算する
    def Bwz(self, point: np.array):
        out = self.coil.gamma * self.Bw()
        fracUp = np.dot(self.coil.forward, self.wire.position - point)
        fracDown = np.pow(np.abs(self.wire.position - pont), 3.)
        return out * (fracUp / fracDown)
        