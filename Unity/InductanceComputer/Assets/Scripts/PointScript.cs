using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointScript : MonoBehaviour {

    [SerializeField] public TextAsset csv;

    [SerializeField] public float gamma;

    [SerializeField] public float[] times;

    [SerializeField] public int timeCount;

    [SerializeField] public int pointCount;

    [SerializeField] public Vector3[] pointPositions;

    [SerializeField] public Vector3[] fluxDensities;

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
            var elements = line.Replace(" ", "").Replace("\t", "").Split(',');

            if (count == 0)
            {
                timeCount = int.Parse(elements[0]);
                pointCount = int.Parse(elements[1]);
                times = new float[timeCount];
                pointPositions = new Vector3[pointCount];
                fluxDensities = new Vector3[pointCount];
            }
            else
            {
                for (int i = 0; i < pointCount; ++i)
                {
                    pointPositions[count - 1] = new Vector3(
                        float.Parse(elements[i * 3 + 1]),
                        float.Parse(elements[i * 3 + 2]),
                        float.Parse(elements[i * 3 + 3])
                        );
                }
            }

            ++count;
        }
    }
}
