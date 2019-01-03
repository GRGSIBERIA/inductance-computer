using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class PointScript : MonoBehaviour {

    [SerializeField] public TextAsset CSV { get; set; }

    [SerializeField] public float Sigma { get; set; } = 1f;

    [SerializeField] public float Gamma { get; set; } = 1f;

    [SerializeField] public float[] Times { get; private set; }

    [SerializeField] public int TimeCount { get; private set; }

    [SerializeField] public int PointCount { get; private set; }

    [SerializeField] public Vector3[][] PointPositions { get; private set; }

    [SerializeField] public float[] FluxDensities { get; private set; }

    [SerializeField] public ComputeShader shader;
    
    public int UsingThreads(CoilScript coil)
    {
        return coil.CoilCount * PointCount;
    }

    public int UsingMemory(CoilScript coil)
    {
        return UsingThreads(coil) * 4 * 3 + PointCount * 4 * 4 + UsingThreads(coil) * 11 * 4;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadCSV()
    {
        if (CSV == null)
            throw new System.ArgumentNullException("csv is not set text asset");

        var lines = CSV.text.Split('\n');
        int count = 0;

        foreach (var line in lines)
        {
            var elements = line.Replace(" ", "").Replace("\t", "").Split(',');

            if (count == 0)
            {
                TimeCount = int.Parse(elements[0]);
                PointCount = int.Parse(elements[1]);
                Times = new float[TimeCount];
                PointPositions = new Vector3[TimeCount][];
                FluxDensities = new float[PointCount];
            }
            else
            {
                PointPositions[count - 1] = new Vector3[PointCount];
                for (int i = 0; i < PointCount; ++i)
                {
                    PointPositions[count - 1][i] = new Vector3(
                        float.Parse(elements[i * 3 + 1]),
                        float.Parse(elements[i * 3 + 2]),
                        float.Parse(elements[i * 3 + 3])
                        );
                }
            }

            ++count;
        }
    }

    public float[] Compute(CoilBuffers coilBuffers, PointBuffers pointBuffers, int coilCount, int frame)
    {
        int cfdfKernel = shader.FindKernel("ComputeFluxDensityOfFerromagnetic");
        int addedKernel = shader.FindKernel("ComputeAddedFerromagnetic");
        int ferroKernel = shader.FindKernel("ComputeFerromagnetic");

        coilBuffers.SetBuffer(shader, cfdfKernel);
        pointBuffers.SetBuffer(shader, cfdfKernel, addedKernel, ferroKernel);

        // 実行
        shader.Dispatch(cfdfKernel, coilCount, PointCount, 2);
        shader.Dispatch(addedKernel, coilCount, PointCount, 1);
        shader.Dispatch(ferroKernel, PointCount, 1, 1);

        // 結果の受け取り
        pointBuffers.Ferromagnetics.GetData(FluxDensities);

        // バッファの解放
        coilBuffers.Dispose();

        return FluxDensities;
    }

    public void SetPointBuffer(int frame, PointBuffers pointBuffers)
    {
        pointBuffers.SetData(PointPositions[frame]);
    }

    public PointBuffers GeneratePointBuffer(int coilCount)
    {
        return new PointBuffers(coilCount, PointCount);
    }
}

public class PointBuffers : IDisposable
{
    public ComputeBuffer PointPositions { get; private set; }

    public ComputeBuffer FerromagneticTop { get; private set; }
    public ComputeBuffer FerromagneticBottom { get; private set; }
    public ComputeBuffer FerromagneticAdded { get; private set; }
    public ComputeBuffer Ferromagnetics { get; private set; }

    public PointBuffers(int coilCount, int pointCount)
    {
        PointPositions = new ComputeBuffer(pointCount, Marshal.SizeOf(typeof(Vector3)));
        FerromagneticTop = new ComputeBuffer(pointCount * coilCount, Marshal.SizeOf(typeof(float)));
        FerromagneticBottom = new ComputeBuffer(pointCount * coilCount, Marshal.SizeOf(typeof(float)));
        FerromagneticAdded = new ComputeBuffer(pointCount * coilCount, Marshal.SizeOf(typeof(float)));
        Ferromagnetics = new ComputeBuffer(pointCount, Marshal.SizeOf(typeof(float)));
    }

    public void SetData(Vector3[] pointPositions)
    {
        PointPositions.SetData(pointPositions);
    }

    public void SetBuffer(ComputeShader shader, int cfdf, int added, int ferro)
    {
        shader.SetBuffer(cfdf, "ferroMatrixTop", FerromagneticTop);
        shader.SetBuffer(cfdf, "ferroMatrixBottom", FerromagneticBottom);
        shader.SetBuffer(cfdf, "pointPosition", PointPositions);
        shader.SetBuffer(added, "ferroMatrixTop", FerromagneticTop);
        shader.SetBuffer(added, "ferroMatrixBottom", FerromagneticBottom);
        shader.SetBuffer(added, "ferroMatrixAdded", FerromagneticAdded);
        shader.SetBuffer(ferro, "ferroMatrixAdded", FerromagneticAdded);
        shader.SetBuffer(ferro, "ferroMagnetics", Ferromagnetics);
    }

    ~PointBuffers()
    {
        Dispose();
    }

    public void Dispose()
    {
        PointPositions.Dispose();
        FerromagneticTop.Dispose();
        FerromagneticBottom.Dispose();
        FerromagneticAdded.Dispose();
        Ferromagnetics.Dispose();
    }
}
