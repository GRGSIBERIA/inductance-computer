using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldScript : MonoBehaviour {

    [SerializeField] public Vector3 FieldSize { get; } = new Vector3(0.1f, 0.1f, 0.1f);

    [SerializeField] public Vector3Int NumberOfPartition { get; } = new Vector3Int(200, 200, 200);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        Vector3 p = transform.position;

        Vector3[] v = new Vector3[8] {
            p + new Vector3(0, 0, 0),
            p + new Vector3(FieldSize.x, 0, 0),
            p + new Vector3(0, 0, FieldSize.z),
            p + new Vector3(FieldSize.x, 0, FieldSize.z),
            p + new Vector3(0, FieldSize.y, 0),
            p + new Vector3(FieldSize.x, FieldSize.y, 0),
            p + new Vector3(0, FieldSize.y, FieldSize.z),
            p + new Vector3(FieldSize.x, FieldSize.y, FieldSize.z)
        };

        Gizmos.DrawLine(v[0], v[1]);
        Gizmos.DrawLine(v[0], v[2]);
        Gizmos.DrawLine(v[3], v[1]);
        Gizmos.DrawLine(v[3], v[2]);
        Gizmos.DrawLine(v[4], v[5]);
        Gizmos.DrawLine(v[4], v[6]);
        Gizmos.DrawLine(v[7], v[5]);
        Gizmos.DrawLine(v[7], v[6]);
        for (int i = 0; i < 4; ++i)
            Gizmos.DrawLine(v[i], v[4 + i]);
    }
}
