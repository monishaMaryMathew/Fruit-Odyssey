using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour 
{

	// Use this for initialization
	public Material mat;
	public GameObject start;
	public GameObject end;
	void Start () 
	{
		MeshFilter filter = new MeshFilter();
		MeshRenderer rend = GetComponent<MeshRenderer>();
		Mesh m = GetComponent<MeshFilter>().mesh;
		rend.material = mat;
		Vector3[] v = new Vector3[2];
		v [0] = start.transform.position;
		v[1] = end.transform.position;
		int[] arr = new int[]{0,1};

		//arr = [0,1
		/*List<Vector3> list = */ m.SetIndices(arr, MeshTopology.Lines, 0);
		m.vertices = v;

		filter.mesh = m;
		//rend.
	}

}
