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
        self.sigma = 1.
        self.gamma = 1.
        self.halfHeight = height * 0.5
        self.top = self.position + self.forward * self.halfHeight
        self.bottom = self.position - self.forward * self.halfHeight

class Wire:
    def __init__(self, position: np.array, coils: List[Coil]):
        self.position = position
        self.fluxDensity = self._computeFluxDensity(coils)

    def _computeFluxDensity(self, coils: List[Coil]) -> float:
        fluxDensity = 0.
        for coil in coils:
            fd = FluxDensity(coil, self)
            fluxDensity += fd.Bw()
        return fluxDensity

class FluxDensity:
    def __init__(self, coil: Coil, wire: Wire):
        self.coil = coil 
        self.wire = wire

    def _fracUp(self, r) -> float:
        return self.coil.sigma * r
    
    def _fracDown(self, dtheta: float, dr: float, coilPosition: np.array) -> float:
        q = np.quaternion(dtheta, self.coil.forward[0], self.coil.forward[1], self.coil.forward[2])
        q = 1. / q.absolute() * q
        return pow(abs(dr * q * self.coil.right + self.wire.position - coilPosition), 3.)

    # ワイヤの磁束密度を求める
    def _integrateBw(self, dtheta: float, dr: float) -> float:
        return \
            self._fracUp(dr) / self._fracDown(dr, dtheta, self.coil.top) - \
            self._fracUp(dr) / self._fracDown(dr, dtheta, self.coil.bottom)

    # ワイヤの磁束密度を計算する
    def Bw(self) -> float:
        return dblquad(self._integrateBw, 0, 2.*np.pi, lambda dr: 0, lambda dr: self.coil.radius)
    
    # 測定点の磁束密度を計算する
    def Bwz(self, point: np.array, bw=None) -> float:
        if bw == None:
            bw = self.Bw()
        out = self.coil.gamma * bw
        fracUp = np.dot(self.coil.forward, self.wire.position - point)
        fracDown = pow(np.abs(self.wire.position - point), 3.)
        return out * (fracUp / fracDown)
    