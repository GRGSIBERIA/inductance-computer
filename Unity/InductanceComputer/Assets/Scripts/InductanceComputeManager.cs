using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InductanceComputeManager : MonoBehaviour
{
    [SerializeField] public int StartFrame { get; set; }

    [SerializeField] public int EndFrame { get; set; }

    [SerializeField] public string SaveFolder { get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        if (transform.rotation != Quaternion.identity)
        {
            transform.rotation = Quaternion.identity;
            Debug.LogWarning("Do not rotation");
        }
    }
}
