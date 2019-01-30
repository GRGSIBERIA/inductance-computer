import csv 
import numpy as np

class ColumnsNotConformsException(Exception):
    pass

def load_times(path):
    with open(path, "r") as f:
        reader = csv.reader(f)
        numof_times = int(next(reader)[0])
        times = []
        
        for row in reader:
            if row[0] == "":
                break
            times.append(int(row[0]))
        
        if len(times) != numof_times:
            if len(times) - 1 != numof_times:
        return (numof_times, times)

def load_wires(path, numof_times):
    with open(path, "r") as f:
        reader = csv.reader(f)
        numof_wires = int(next(reader)[0])
        gamma = float(next(reader)[0])
        positions = []

        for row in reader:
            if row[0] == "":    # 空行が挟まっている場合は抜ける
                break

            for i in range(numof_wires):
                positions.append(np.array([float(row[i * 3]), float(row[i * 3 + 1]), float(row[i * 3 + 1])]))

        return (numof_wires, gamma, positions)

def load_coils(path, numof_times):
    with open(path, "r") as f:
        reader = csv.reader(f)
        numof_coils = int(next(reader)[0])
        positions = []
        forwards = []
        rights = []

        for row in reader:
            if row[0] == "":
                break
            
            for i in range(numof_coils):
                k = i * 3
                positions.append(np.array([float(row[k]), float(row[k + 1]), float(row[k + 2])]))
                forwards.append(np.array([float(row[k + 3]), float(row[k + 4]), float(row[k + 5])]))
                rights.append(np.array([float(row[k + 6]), float(row[k + 7]), float(row[k + 8])]))
        
        return (numof_coils, positions, forwards, rights)
        