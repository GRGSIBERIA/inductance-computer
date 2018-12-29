using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CoilData
{
    [SerializeField] public float Radius { get; }
    [SerializeField] public Vector3[] Positions { get; }
    [SerializeField] public Vector3[] Fronts { get; }
    [SerializeField] public Vector3[] Rights { get; }
    
    public CoilData(float radius, int timeCount)
    {
        this.Positions = new Vector3[timeCount];
        this.Fronts = new Vector3[timeCount];
        this.Rights = new Vector3[timeCount];
        this.Radius = radius;
    }

    public void SetVector(int index, Vector3 position, Vector3 front, Vector3 right)
    {
        Positions[index] = position;
        Fronts[index] = front;
        Rights[index] = right;
    }
}

public class CoilScript : MonoBehaviour {

    [SerializeField] public TextAsset CSV { get; set; }
    [SerializeField] public int NumberOfPartitionOfRadius { get; set; }
    [SerializeField] public int NumberOfPartitionOfRadians { get; set; }

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
            else if (count == 1)
            {
                // コイル情報の読み込み
                for (int i = 0; i < Coils.Length; ++i)
                {
                    float radius = float.Parse(elements[i]);
                    Coils[i] = new CoilData(radius, TimeCount);
                }
            }
            else
            {
                // 各行の情報を読み取り
                Times[count - 2] = float.Parse(elements[0]);

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
}
