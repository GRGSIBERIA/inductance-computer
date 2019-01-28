from typing import List
import numpy as np

class Coil:
    def __init__(self, position: np.array, front: np.array, right: np.array, height: float, radius: float):
        self.position = position 
        self.front = front 
        self.right = right 
        self.height = height 
        self.radius = radius
