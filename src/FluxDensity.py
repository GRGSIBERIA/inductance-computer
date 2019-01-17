# 作成者：竹渕瑛一
from typing import List
import numpy as np
import threading
from scipy.integrate import dblquad
from src.Quaternion import Quaternion

class Coil:
    def __init__(self, position: np.array, forward: np.array, right: np.array, height: float, radius: float, sigma=1., gamma=1.):
        """コイルのクラス"""
        self.position = position
        self.forward = forward / np.linalg.norm(forward)
        self.right = right / np.linalg.norm(right)
        self.height = height
        self.radius = radius
        self.sigma = sigma
        self.gamma = gamma
        self.halfHeight = height * 0.5
        self.top = self.position + self.forward * self.halfHeight
        self.bottom = self.position - self.forward * self.halfHeight

class Wire:
    def __init__(self, position: np.array):
        """ワイヤのクラス"""
        self.position = position
        self._fluxDensity = 0.
        self._computed = False
            
    def _computeFluxDensity(self, coil: Coil):
        fd = FluxDensity(coil, self)
        self._fluxDensity += fd.Bw()
    
    def FluxDensity(self, coils: List[Coil]) -> float:
        """ワイヤの磁束密度を計算して返す"""
        if self._computed:
            return self._fluxDensity

        for coil in coils:
            self._computeFluxDensity(coil)
        return self._fluxDensity

class FluxDensity:
    def __init__(self, coil: Coil, wire: Wire):
        """磁束密度を計算するクラス"""
        self.coil = coil 
        self.wire = wire

    def _fracUp(self, r) -> float:
        return self.coil.sigma * r
    
    def _fracDown(self, dtheta: float, dr: float, coilPosition: np.array) -> float:
        q = Quaternion(0, self.coil.forward)
        v = q.rotation(self.coil.right, dtheta) * dr + self.wire.position - coilPosition
        v = np.linalg.norm(v)
        return v * v * v

    def _integrateBw(self, dtheta: float, dr: float) -> float:
        """ワイヤの磁束密度を求める, 差分計算あり"""
        return \
            self._fracUp(dr) / self._fracDown(dr, dtheta, self.coil.top) - \
            self._fracUp(dr) / self._fracDown(dr, dtheta, self.coil.bottom)

    def Bw(self) -> float:
        """ワイヤの磁束密度を計算する"""
        return dblquad(self._integrateBw, 0, 2.*np.pi, lambda dr: 0, lambda dr: self.coil.radius)[0]
    
# 測定場のクラス
class Field:
    def __init__(self, origin: np.array, forward: np.array, right: np.array, field_size: np.array, field_divisions: np.array):
        """ 規定場クラス """
        self.origin = origin
        self.forward = forward / np.linalg.norm(forward)
        self.right = forward / np.linalg.norm(right)
        self.up = np.cross(self.forward, self.right)
        self.width = field_size[0]      # 説明変数にしておく
        self.height = field_size[1]
        self.depth = field_size[2]
        self.field_divisions = field_divisions
        self.num_sizes = field_size / field_divisions
        self.num_sizes = [int(self.num_sizes[0]), int(self.num_sizes[1]), int(self.num_sizes[2])]
        self.fluxDensity = np.zeros(self.num_sizes, dtype=float)
        self._computed = False
    
    def Bwz(self, point: np.array, wire: Wire, coil: Coil, coils: List[Coil]) -> float:
        """測定点の磁束密度を計算する"""
        out = coil.gamma * wire.FluxDensity(coils)
        fracUp = np.dot(coil.forward, wire.position - point)
        downpow = np.linalg.norm(wire.position - point)
        fracDown = downpow * downpow * downpow
        return out * (fracUp / fracDown)
    
    def _GetPoint(self, w: int, h: int, d: int) -> np.array:
        """w,h,dのインデックスに応じて空間上の座標を取得"""
        return np.array([\
            w * self.right * self.field_divisions[0], \
            h * self.up * self.field_divisions[1], \
            d * self.forward * self.field_divisions[2]])

    def _workerThreadForDepth(self, w: int, h: int, wire: Wire, coils: List[Coil]):
        for d in range(self.num_sizes[2]):
            point = self._GetPoint(w, h, d)
            for coil in coils:
                self.fluxDensity[w][h][d] += self.Bwz(point, wire, coil, coils)

    def FluxDensity(self, wire: Wire, coils: List[Coil]) -> np.array:
        """空間の磁束密度の計算"""
        if self._computed:
            return self.fluxDensity
        
        for w in range(self.num_sizes[0]):
            for h in range(self.num_sizes[1]):
                #thread = threading.Thread(target=self._workerThreadForDepth, args=(w, h, wire, coils))
                #thread.start()      # 効率化のためdepthごとにスレッドを生成する
                self._workerThreadForDepth(w, h, wire, coils)

        # スレッドをjoinして待つ
        main_thread = threading.current_thread()
        for thread in threading.enumerate():
            if thread is main_thread:
                continue
            thread.join()
        
        self._computed = True
        return self.fluxDensity