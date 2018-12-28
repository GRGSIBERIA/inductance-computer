using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CoilInformation
{
    [SerializeField] public float radius;
    [SerializeField] public Vector3[] positions;
    [SerializeField] public Vector3[] fronts;
    [SerializeField] public Vector3[] rights;
    
    public CoilInformation(float radius, int timeCount)
    {
        this.positions = new Vector3[timeCount];
        this.fronts = new Vector3[timeCount];
        this.rights = new Vector3[timeCount];
        this.radius = radius;
    }

    public void SetVector(int index, Vector3 position, Vector3 front, Vector3 right)
    {
        positions[index] = position;
        fronts[index] = front;
        rights[index] = right;
    }
}

public class CoilScript : MonoBehaviour {

    [SerializeField] public TextAsset csv;
    [SerializeField] public int numberOfPartitionOfRadius;
    [SerializeField] public int numberOfPartitionOfRadians;

    [SerializeField] public int coilCount;
    [SerializeField] public int timeCount;

    [SerializeField] public float[] times;
    [SerializeField] public CoilInformation[] coilInformations;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadCSV()
    {
        if (csv == null)
            throw new System.ArgumentNullException("csv is not set text asset");

        var lines = csv.text.Split('\n');
        int count = 0;

        foreach (var line in lines)
        {
            var str = line.Replace(" ", "").Replace("\t", "");
            var elements = str.Split(',');

            if (count == 0)
            {
                // ヘッダ行の読み込み
                timeCount = int.Parse(elements[0]);
                coilCount = int.Parse(elements[1]);
                times = new float[timeCount];
                coilInformations = new CoilInformation[coilCount];
            }
            else if (count == 1)
            {
                // コイル情報の読み込み
                for (int i = 0; i < coilInformations.Length; ++i)
                {
                    float radius = float.Parse(elements[i]);
                    coilInformations[i] = new CoilInformation(radius, timeCount);
                }
            }
            else
            {
                // 各行の情報を読み取り
                times[count - 2] = float.Parse(elements[0]);

                for (int i = 0; i < coilInformations.Length; ++i)
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
                    coilInformations[i].SetVector(count - 2, position, front, right);
                }
            }
            ++count;
        }
    }
}
