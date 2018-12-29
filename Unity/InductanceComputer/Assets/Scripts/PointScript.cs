﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointScript : MonoBehaviour {

    [SerializeField] public TextAsset CSV { get; set; }

    [SerializeField] public float Sigma { get; set; } = 1f;

    [SerializeField] public float Gamma { get; set; } = 1f;

    [SerializeField] public float[] Times { get; private set; }

    [SerializeField] public int TimeCount { get; private set; }

    [SerializeField] public int PointCount { get; private set; }

    [SerializeField] public Vector3[] PointPositions { get; private set; }

    [SerializeField] public Vector3[] FluxDensities { get; private set; }

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
                FluxDensities = new Vector3[PointCount];
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
}
