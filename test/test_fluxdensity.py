import unittest
import numpy as np
from src.FluxDensity import Coil, Wire
import matplotlib.pyplot as plot
import quaternion

class WiredFluxDensityTest(unittest.TestCase):

    def test_wire(self):
        wire = Wire(np.array([0, 0, 5]))
        coils = self.make_zero_position_one_coil()
        self.assertNotEqual(wire, None)
        self.assertNotEqual(coils, None)
    
    def make_wire_positions(self, z):
        arr = []
        for i in range(201):
            arr.append(0.5 * i - 50)
        return [Wire(np.array([x, 0, z])) for x in arr]
    
    def make_zero_position_one_coil(self):
        return [Coil(np.array([0, 0, -0.5]), np.array([0., 0., 1.]), np.array([1., 0., 0.]), 1, 1)]
        
    def test_positions(self):
        wires = self.make_wire_positions(5)
        coils = self.make_zero_position_one_coil()
        x = [w.position[0] for w in wires]
        fluxes = []
        for wire in wires:
            fluxes.append(wire.FluxDensity(coils))
        fluxes = np.array(fluxes) / np.max(fluxes)
        var = np.std(fluxes)
        print("var: %f" % (var))

        debug = False
        if debug:
            plot.plot(x, fluxes)
            plot.xlim(-10, 10)
            plot.show()
    
    def test_field_point(self):
        pass
    
    def test_quaternion(self):
        x = np.array([1,0,0])
        z = np.array([0,0,1])
        q = np.quaternion(2.*np.pi, z[0], z[1], z[2])
        a = (q * x * ((1./q.norm().conjugate()) * q))[0]
        print(a)