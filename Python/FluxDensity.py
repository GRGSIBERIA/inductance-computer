# 作成者：竹渕瑛一
from typing import List
import numpy as np
import quaternion

# 測定場のクラス
class Field:
    def __init__(self, width, height, depth):
        self.width = width
        self.height = height
        self.depth = depth

# コイルのクラス
class Coil:
    def __init__(self, position, forward, right, height, radius):
        self.position = position
        self.forward = forward
        self.right = right
        self.height = height
        self.radius = radius
        self.sigma = 1
        self.gamma = 1

class Wire:
    def __init__(self, position):
        self.position = position

class FluxDensity:
    def __init__(self, field: Field):
        self.field = field

    def _fracUp(self, r, coil: Coil):
        return coil.sigma * r
    
    def fracDown(self, r, theta, coil: Coil, wire: Wire):
        q = np.quaternion
        return r * np.quaternion([])

    def _computeBw(self, coil: Coil, wires: List[Wire]):
        pass

    def Bw(self, field: Field, coils: List[Coil], wires: List[Wire]):
        total = 0.
        for coil in coils:
            pass