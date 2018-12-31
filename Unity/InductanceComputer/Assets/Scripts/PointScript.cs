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

    public void Compute(CoilScript coil, int frame)
    {
        int cfdfKernel = shader.FindKernel("ComputeFluxDensityOfFerromagnetic");
        int addedKernel = shader.FindKernel("ComputeAddedFerromagnetic");
        int ferroKernel = shader.FindKernel("ComputeFerromagnetic");

        // データの入力
        CoilBuffers coilBuffers = coil.GenerateCoilBuffer(frame);
        PointBuffers pointBuffers = GeneratePointBuffer(frame, coil.CoilCount);

        coilBuffers.SetBuffer(shader, cfdfKernel);
        pointBuffers.SetBuffer(shader, cfdfKernel, addedKernel, ferroKernel);

        // 実行
        shader.Dispatch(cfdfKernel, coil.CoilCount, PointCount, 2);
        shader.Dispatch(addedKernel, coil.CoilCount, PointCount, 1);
        shader.Dispatch(ferroKernel, PointCount, 1, 1);

        // 結果の受け取り
        pointBuffers.Ferromagnetics.GetData(FluxDensities);

        // バッファの解放
        coilBuffers.Dispose();
    }

    public PointBuffers GeneratePointBuffer(int frame, int coilCount)
    {
        return new PointBuffers(PointPositions[frame], coilCount);
    }
}

public class PointBuffers : IDisposable
{
    public ComputeBuffer PointPositions { get; private set; }

    public ComputeBuffer FerromagneticTop { get; private set; }
    public ComputeBuffer FerromagneticBottom { get; private set; }
    public ComputeBuffer FerromagneticAdded { get; private set; }
    public ComputeBuffer Ferromagnetics { get; private set; }

    public PointBuffers(Vector3[] pointPositions, int coilCount)
    {
        PointPositions = new ComputeBuffer(pointPositions.Length, Marshal.SizeOf(typeof(Vector3)));
        FerromagneticTop = new ComputeBuffer(pointPositions.Length * coilCount, Marshal.SizeOf(typeof(float)));
        FerromagneticBottom = new ComputeBuffer(pointPositions.Length * coilCount, Marshal.SizeOf(typeof(float)));
        FerromagneticAdded = new ComputeBuffer(pointPositions.Length * coilCount, Marshal.SizeOf(typeof(float)));
        Ferromagnetics = new ComputeBuffer(pointPositions.Length, Marshal.SizeOf(typeof(float)));

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
