import unittest
import numpy as np
from src.Coil import Coil 
from src.Wire import Wire


class WireTest(unittest.TestCase):
    def setUp(self):
        self.wire = Wire(np.array([0, 0, 5]))
        self.coil = Coil(np.array([0, 0, 0]), np.array([0, 0, 1]), np.array([1, 0, 0]), 1, 1)

    def test_load_wire(self):
        np.testing.assert_array_almost_equal(self.wire.positions, np.array([0, 0, 5]))
    
    def test_flux_density(self):
        self.wire.FluxDensity(1, [self.coil])
