using UnityEngine;
using System.Collections;

public class carsMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Check if we died (TODO: Move into generateTerrain)
		if (transform.position.y < -5.0f) {
			// Reset position
			transform.position = new Vector3 (0, 1.0f, 0);
			transform.rotation = new Quaternion ();
			// TODO: Reset velocity of children
			//rigidbody.velocity = Vector3.zero;
			//rigidbody.angularVelocity = Vector3.zero;
			// TODO: Clear the terrain
		}
	}
}
