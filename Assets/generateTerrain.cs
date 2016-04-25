using UnityEngine;
using System.Collections;

public class generateTerrain : MonoBehaviour {
	public Transform road01;

	GameObject referencePerso;
	GameObject terrain;
	int limitPos;

	// Use this for initialization
	void Start () {
		terrain = GameObject.Find("Terrain"); // Get the terrain
		referencePerso = GameObject.Find("Car"); // Get the player
		limitPos = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(referencePerso.transform.position.z > limitPos) { // We reach the limit: add a new child
			limitPos += 20; // TODO: Replace by cst or smth like object.getSize().length()
			GameObject newPortion = Instantiate(road01, new Vector3(0, 0, limitPos), Quaternion.identity) as GameObject;
			// newPortion.transform.parent = terrain.transform;
			Debug.Log ("Pos");
			Debug.Log (limitPos);
			Debug.Log (referencePerso.transform.position.z);
		}
	}
}
