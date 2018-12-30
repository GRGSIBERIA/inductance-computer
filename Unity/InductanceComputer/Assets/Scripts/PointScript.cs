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

    [SerializeField] public Vector3[] PointPositions { get; private set; }

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
                PointPositions = new Vector3[PointCount];
                FluxDensities = new float[PointCount];
            }
            else
            {
                for (int i = 0; i < PointCount; ++i)
                {
                    PointPositions[count - 1] = new Vector3(
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
        int kernel = shader.FindKernel("ComputeFluxDensityOfFerromagnetic");

        // データの入力
        CoilBuffers coilBuffers = coil.GenerateCoilBuffer(frame);
        coilBuffers.SetBuffer(shader, kernel);

        ComputeBuffer pointPositions = new ComputeBuffer(PointCount, Marshal.SizeOf(typeof(Vector3)));
        ComputeBuffer fluxDensity = new ComputeBuffer(PointCount * coil.CoilCount, Marshal.SizeOf(typeof(float)));

        // 実行
        shader.Dispatch(kernel, coil.CoilCount, PointCount, 1);

        // 結果の受け取り
        var fluxDensityOfFerromagnetic = new float[PointCount];
        var FD2D = new float[PointCount * coil.CoilCount];
        fluxDensity.GetData(FD2D);
        for (int i = 0; i < PointCount; ++i)
        {
            fluxDensityOfFerromagnetic[i] = 0f;
            for (int j = 0; j < coil.CoilCount; ++j)
            {
                fluxDensityOfFerromagnetic[i] += FD2D[j * coil.CoilCount + i];
            }
        }
        // fluxDensityOfFerromagnetic に強磁性体における磁束密度の合算値

        // バッファの解放
        coilBuffers.DisposeBuffers();
    }
}
