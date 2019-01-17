import unittest
import numpy as np
from src.FluxDensity import Coil

class TestCoil(unittest.TestCase):
    def test_cross(self):
        coil = Coil(np.array([0, 0, 0]), np.array([0, 0, 1]), np.array([1, 0, 0]), 1, 1)
        np.testing.assert_array_equal(np.cross(coil.forward, coil.right), np.array([0, 1, 0]))