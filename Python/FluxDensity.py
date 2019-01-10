# 作成者：竹渕瑛一
from typing import List
import numpy as np
import quaternion
import threading
from scipy.integrate import dblquad

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
        self._fluxDensity = 0.
        self._threads = []
        self._lock = threading.Lock()
        self._coils = coils

        for coil in coils:
            thread = threading.Thread(target=self._computeFluxDensity, args=(coil))
            self._threads.append(thread)
            
    def _computeFluxDensity(self, coil: Coil):
        fd = FluxDensity(coil, self)
        flux = fd.Bw()

        self._lock.acquire()
        self._fluxDensity += flux
        self._lock.release()
    
    def FluxDensity(self):
        for thread in self._threads:    # まとめて走らせてまとめて受け取る
            thread.start()
        for thread in self._threads:
            thread.join()
        return self._fluxDensity

# 磁束密度を計算するクラス
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

    # ワイヤの磁束密度を求める, 差分計算あり
    def _integrateBw(self, dtheta: float, dr: float) -> float:
        return \
            self._fracUp(dr) / self._fracDown(dr, dtheta, self.coil.top) - \
            self._fracUp(dr) / self._fracDown(dr, dtheta, self.coil.bottom)

    # ワイヤの磁束密度を計算する
    def Bw(self) -> float:
        return dblquad(self._integrateBw, 0, 2.*np.pi, lambda dr: 0, lambda dr: self.coil.radius)
    
# 測定場のクラス
class Field:
    def __init__(self, width, height, depth):
        self.width = width
        self.height = height
        self.depth = depth
        self._data = np.zeros((width, height, depth), dtype=float)
    
    # 測定点の磁束密度を計算する
    def Bwz(self, point: np.array, wire: Wire, coil: Coil) -> float:
        out = coil.gamma * wire.FluxDensity()
        fracUp = np.dot(coil.forward, wire.position - point)
        fracDown = pow(np.abs(wire.position - point), 3.)
        return out * (fracUp / fracDown)