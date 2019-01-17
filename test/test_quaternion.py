import unittest
import numpy as np
from src.Quaternion import Quaternion

class QuaternionTest(unittest.TestCase):
    def test_quaternion(self):
        x = np.array([1,0,0])
        z = np.array([0,0,1])

        q = Quaternion(0, z)
        v = q.rotation(x, np.pi)
        
        np.testing.assert_almost_equal(v, np.array([-1., 0., 0.]))

        v = q.rotation(x, np.pi/2.)
        np.testing.assert_almost_equal(v, np.array([0., 1., 0.]))

        v = q.rotation(x, -np.pi/2.)
        np.testing.assert_almost_equal(v, np.array([0., -1., 0.]))