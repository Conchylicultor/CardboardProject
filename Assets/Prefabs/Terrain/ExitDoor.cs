using UnityEngine;
using System.Collections;

public class ExitDoor : MonoBehaviour {
	public generateTerrain terrainManager;

	public string folderName;
	public float angle;

	void OnTriggerEnter(Collider other)
	{
		// Check that the collision is only trigger by the car
		if (other.tag == "car_body") {
			Debug.Log("Collision triggered!");
			terrainManager.launchExitMode(this);
		}
	}
}
