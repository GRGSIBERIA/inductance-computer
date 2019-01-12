import unittest
import numpy as np
from src.FluxDensity import Coil, Wire

class WiredFluxDensityTest(unittest.TestCase):
    def test_wire(self):
        wire = Wire(np.array([0, 0, 5]))
        coils = self.make_zero_position_one_coil()
    
    def make_wire_positions(self):
        arr = []
        for i in range(201):
            arr.append(0.5 * i - 50)
        return arr
    
    def make_zero_position_one_coil(self):
        return [Coil(np.array([0, 0, 0]), np.array([0., 0., 1.]), np.array([1., 0., 0.]), 1, 1)]
        
    def test_positions(self):
        a = self.make_wire_positions()
        wires = [Wire(np.array([x, 0, 5])) for x in a]
        coils = self.make_zero_position_one_coil()
        for wire in wires:
            wire.FluxDensity(coils)