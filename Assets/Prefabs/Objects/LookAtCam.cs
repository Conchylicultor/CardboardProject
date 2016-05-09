using UnityEngine;
using System.Collections;

public class LookAtCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform == null || Camera.main == null) {
			return;
		}
		transform.LookAt(Camera.main.transform.position, -Vector3.up);
		transform.Rotate(new Vector3(90,0,0));
	}
}
