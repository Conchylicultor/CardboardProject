using UnityEngine;
using System.Collections;

public class CollisionPers : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	void OnTriggerEnter(Collider other) {
		if (other.tag == "persr") {
			Debug.Log("Person killed");
			Debug.Log(other.transform.position);
			Debug.Log(this.transform.position);
			Debug.Log(other.tag);

			//Destroy(other.gameObject);
			//Destroy(gameObject);
		}
	}
}
