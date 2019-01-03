using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public struct CoilData
{
    [SerializeField] public float Radius { get; }
    [SerializeField] public float HalfHeight { get; }
    [SerializeField] public Vector3[] Positions { get; }
    [SerializeField] public Vector3[] Fronts { get; }
    [SerializeField] public Vector3[] Rights { get; }
    
    public CoilData(float radius, float halfHeight, int timeCount)
    {
        this.Positions = new Vector3[timeCount];
        this.Fronts = new Vector3[timeCount];
        this.Rights = new Vector3[timeCount];
        this.Radius = radius;
        this.HalfHeight = halfHeight;
    }

    public void SetVector(int frame, Vector3 position, Vector3 front, Vector3 right)
    {
        Positions[frame] = position;
        Fronts[frame] = front;
        Rights[frame] = right;
    }

    public Vector3 GetPosition(int frame)
    {
        return Positions[frame];
    }

    public Vector3 GetFront(int frame)
    {
        return Fronts[frame];
    }

    public Vector3 GetRight(int frame)
    {
        return Rights[frame];
    }
}

public class CoilScript : MonoBehaviour {

    [SerializeField] public TextAsset CSV { get; set; }
    [SerializeField] public int NumberOfPartitionOfRadius { get; set; } = 100;
    [SerializeField] public int NumberOfPartitionOfRadians { get; set; } = 360;

    [SerializeField] public int CoilCount { get; set; }
    [SerializeField] public int TimeCount { get; set; }

    [SerializeField] public float[] Times { get; private set; }
    [SerializeField] public CoilData[] Coils { get; private set; }

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
            var str = line.Replace(" ", "").Replace("\t", "");
            var elements = str.Split(',');

            if (count == 0)
            {
                // ヘッダ行の読み込み
                TimeCount = int.Parse(elements[0]);
                CoilCount = int.Parse(elements[1]);
                Times = new float[TimeCount];
                Coils = new CoilData[CoilCount];
            }
            else if (count - 1 < Coils.Length)
            {
                // コイル情報の読み込み
                for (int i = 0; i < Coils.Length; ++i)
                {
                    float radius = float.Parse(elements[i * 2]);
                    float height = float.Parse(elements[i * 2 + 1]);
                    Coils[i] = new CoilData(radius, height, TimeCount);
                }
            }
            else
            {
                // 各行の情報を読み取り
                Times[count - (Coils.Length + 1)] = float.Parse(elements[0]);

                for (int i = 0; i < Coils.Length; ++i)
                {
                    Vector3 position = new Vector3(
                        float.Parse(elements[i * 9 + 1]),
                        float.Parse(elements[i * 9 + 2]),
                        float.Parse(elements[i * 9 + 3])
                        );
                    Vector3 front = new Vector3(
                        float.Parse(elements[i * 9 + 4]),
                        float.Parse(elements[i * 9 + 5]),
                        float.Parse(elements[i * 9 + 6])
                        );
                    Vector3 right = new Vector3(
                        float.Parse(elements[i * 9 + 7]),
                        float.Parse(elements[i * 9 + 8]),
                        float.Parse(elements[i * 9 + 9])
                        );
                    Coils[i].SetVector(count - 2, position, front, right);
                }
            }
            ++count;
        }
    }

    public int LoadTimeCount()
    {
        var lines = CSV.text.Split('\n');
        return int.Parse(lines[0].Replace(" ", "").Replace("\t", "").Split(',')[0]);
    }
    
    public CoilBuffers GenerateCoilBuffer()
    {
        return new CoilBuffers(CoilCount);
    }

    public void SetCoilBuffer(int frame, CoilBuffers buffers)
    {
        var positions = new Vector3[CoilCount];
        var fronts = new Vector3[CoilCount];
        var rights = new Vector3[CoilCount];
        var heights = new float[CoilCount];
        var radius = new float[CoilCount];

        for (int ci = 0; ci < CoilCount; ++ci)
        {
            positions[ci] = Coils[ci].GetPosition(frame);
            fronts[ci] = Coils[ci].GetFront(frame);
            rights[ci] = Coils[ci].GetRight(frame);
            heights[ci] = Coils[ci].HalfHeight * 0.5f;
            radius[ci] = Coils[ci].Radius;
        }

        buffers.SetData(positions, radius, heights, fronts, rights);
    }
}

public class CoilBuffers : IDisposable
{
    public ComputeBuffer Positions { get; set; }
    public ComputeBuffer Heights { get; set; }
    public ComputeBuffer Radius { get; set; }
    public ComputeBuffer Fronts { get; set; }
    public ComputeBuffer Rights { get; set; }

    public CoilBuffers(int coilCount)
    {
        Positions = new ComputeBuffer(coilCount, Marshal.SizeOf(typeof(Vector3)));
        Heights = new ComputeBuffer(coilCount, Marshal.SizeOf(typeof(float)));
        Radius = new ComputeBuffer(coilCount, Marshal.SizeOf(typeof(float)));
        Fronts = new ComputeBuffer(coilCount, Marshal.SizeOf(typeof(Vector3)));
        Rights = new ComputeBuffer(coilCount, Marshal.SizeOf(typeof(Vector3)));
    }

    public void SetData(Vector3[] positions, float[] radius, float[] heights, Vector3[] fronts, Vector3[] rights)
    {
        Positions.SetData(positions);
        Radius.SetData(radius);
        Heights.SetData(heights);
        Fronts.SetData(fronts);
        Rights.SetData(rights);
    }

    public void SetBuffer(ComputeShader shader, int kernel)
    {
        shader.SetBuffer(kernel, "coilPosition", Positions);
        shader.SetBuffer(kernel, "coilRight", Rights);
        shader.SetBuffer(kernel, "coilFront", Fronts);
        shader.SetBuffer(kernel, "coilRadius", Radius);
        shader.SetBuffer(kernel, "coilHalfHeight", Heights);
    }

    public void Dispose()
    {
        Positions.Dispose();
        Rights.Dispose();
        Fronts.Dispose();
        Radius.Dispose();
        Heights.Dispose();
    }

    ~CoilBuffers()
    {
        Dispose();
    }
}

