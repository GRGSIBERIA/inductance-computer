from typing import List
import numpy as np
import pycuda.autoinit
import pycuda.driver as gpuarray
import pycuda.gpuarray as gpuarray
#from src.Coil import Coil
from Coil import Coil


class Wire:
    def __init__(self, time: float, positions: np.array, deltaDim:List[int]):
        self.time =  time
        self.positions = positions
        self.deltaDim = deltaDim

    def FluxDensity(self, coils: List[Coil]):
        self.fluxDensity = 0. 
        
        deviceArray = gpuarray.to_gpu(np.zeros((len(coils), len(self.positions))).astype(np.float32))
        resultArray = deviceArray.get()

        #for cid in enumerate(coils):
        #    for wid in enumerate(self.positions):
        #        self.fluxDensity += resultArray[cid][wid]

        print(deviceArray)
        print(type(resultArray))

        return self.fluxDensity

wirePositions = []
for i in range(100):
    wirePositions.append([i, 0, 0])

wire = Wire(0, np.array(wirePositions), [100, 100])
wire.FluxDensity([Coil(), Coil()])
