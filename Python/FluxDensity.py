# 作成者：竹渕瑛一
from typing import List
import numpy as np
import quaternion
import threading
from scipy.integrate import dblquad

class Coil:
    def __init__(self, position: np.array, forward: np.array, right: np.array, height: float, radius: float):
        """コイルのクラス"""
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
    def __init__(self, position: np.array):
        self.position = position
        self._fluxDensity = 0.
        self._lock = threading.Lock()
        self._computed = False
            
    def _computeFluxDensity(self, coil: Coil):
        fd = FluxDensity(coil, self)
        flux = fd.Bw()

        # 足し算するところでスレッドをブロック
        self._lock.acquire()
        self._fluxDensity += flux
        self._lock.release()
    
    def FluxDensity(self, coils: List[Coil]) -> float:
        """ワイヤの磁束密度を計算して返す"""
        if self._computed:
            return self._fluxDensity

        threads = []
        for coil in coils:
            thread = threading.Thread(target=self._computeFluxDensity, args=(coil))
            threads.append(thread)

        for thread in threads:    # まとめて走らせてまとめて受け取る
            thread.start()
        for thread in threads:
            thread.join()
        self._computed = True
        return self._fluxDensity

class FluxDensity:
    def __init__(self, coil: Coil, wire: Wire):
        """磁束密度を計算するクラス"""
        self.coil = coil 
        self.wire = wire

    def _fracUp(self, r) -> float:
        return self.coil.sigma * r
    
    def _fracDown(self, dtheta: float, dr: float, coilPosition: np.array) -> float:
        q = np.quaternion(dtheta, self.coil.forward[0], self.coil.forward[1], self.coil.forward[2])
        q = 1. / q.absolute() * q
        return pow(abs(dr * q * self.coil.right + self.wire.position - coilPosition), 3.)

    def _integrateBw(self, dtheta: float, dr: float) -> float:
        """ワイヤの磁束密度を求める, 差分計算あり"""
        return \
            self._fracUp(dr) / self._fracDown(dr, dtheta, self.coil.top) - \
            self._fracUp(dr) / self._fracDown(dr, dtheta, self.coil.bottom)

    def Bw(self) -> float:
        """ワイヤの磁束密度を計算する"""
        return dblquad(self._integrateBw, 0, 2.*np.pi, lambda dr: 0, lambda dr: self.coil.radius)
    
# 測定場のクラス
class Field:
    def __init__(self, origin: np.array, forward: np.array, right: np.array, width: float, height: float, depth: float):
        """ 規定場クラス """
        self.origin = origin
        self.forward = forward
        self.right = right
        self.up = np.cross(self.forward, self.right)
        self.width = width
        self.height = height
        self.depth = depth
        self.fluxDensity = np.zeros((width, height, depth), dtype=float)
    
    def Bwz(self, point: np.array, wire: Wire, coil: Coil, coils: List[Coil]) -> float:
        """測定点の磁束密度を計算する"""
        out = coil.gamma * wire.FluxDensity(coils)
        fracUp = np.dot(coil.forward, wire.position - point)
        fracDown = pow(np.abs(wire.position - point), 3.)
        return out * (fracUp / fracDown)
    
    def _GetPoint(self, w: int, h: int, d: int) -> List[np.array]:
        return [w * self.right, h * self.up, d * self.forward]

    def FluxDensity(self, wire: Wire, coils: List[Coil]) -> np.array:
        """空間の磁束密度の計算"""
        for w in range(self.width):
            for h in range(self.height):
                for d in range(self.depth):
                    point = self._GetPoint(w, h, d)
                    for coil in coils:
                        self.fluxDensity[w][h][d] += self.Bwz(point, wire, coil, coils)