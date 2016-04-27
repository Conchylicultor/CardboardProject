using UnityEngine;
using System.Collections;
using System.IO;

public class generateTerrain : MonoBehaviour {
	public Transform road01;

	public Transform platform;
	public Transform exitDoor;

	GameObject referencePerso;
	GameObject terrain;
	int limitPos;

	Vector3 positionCurrentPlatform; // The position of the folder in the world

	GameObject nextPlateform; // The futur folder plateform to reach

	string nameNextFolder;
	float angleNextFolder;
	// From norm(posPlayer - posCurrentPlatform), we can deduce the distance of the player to the plateform
	float nextDistance; // To make the next plateform appear
	bool middleReach;


	/**
	 * We create a new platform with x exit for each subfolder (+1 for parent)
	 * 
	 * */
	void loadFolder(string folderName, Vector3 position) {
		Debug.Log("Try loading folder...");
		Debug.Log(folderName);

		// Create the main platform
		nextPlateform = Instantiate(platform, position, Quaternion.identity) as GameObject;

		// Extract all subfolders list
		System.IO.DirectoryInfo rootDir = new System.IO.DirectoryInfo(folderName);
		DirectoryInfo[] dirsInfo = rootDir.GetDirectories();

		// Create one exit by subfolder (".." for parent)
		int nbExit = dirsInfo.Length; // (+1 for parent ?)
		int currentExit = 0; // (+1 for parent ?)
		foreach (DirectoryInfo dir in dirsInfo) { // TODO: Put a limit. For now, lets hope ther is not 100 subfolder
			Debug.Log(dir);
			currentExit++;

			// Each one with a particular angle (float) and destination (string)
		}

		// What about parent (And what about we try to leave the application folder)
	}

	void arriveOnFolderPlatform() {
		
	}

	// Use this for initialization
	void Start () {
		terrain = GameObject.Find("Terrain"); // Get the terrain
		referencePerso = GameObject.Find("Car"); // Get the player
		limitPos = 0;

		// Loading the terrain
		loadFolder(Application.dataPath, Vector3.zero);

		// TODO: Initialize currentFolder, currentFolderPos, ...
	}
	
	// Update is called once per frame
	void Update () {
		// Check the collisions (are we exiting the platform ?)
		// If yes, initialize nameNextFolder, angle & cie >> go in travelMode

		// If we are in travel mode, we check the distance of the player relative to the
		// current platform
			// If greater than next distance: Add new road portion
			// If we reach the middle, creating the next platform and make it appear (loadFolder)
			// If we reach the next plateform: remove previous plateform and road, go in exploration mode (create enemy ??)
			// If we go back on our traces (impossible ?)

		// Check if the player die (>> go to trash)


		/*if(referencePerso.transform.position.z > limitPos) { // We reach the limit: add a new child
			limitPos += 20; // TODO: Replace by cst or smth like object.getSize().length()
			GameObject newPortion = Instantiate(road01, new Vector3(0, 0, limitPos), Quaternion.identity) as GameObject;
			// newPortion.transform.parent = terrain.transform;
			Debug.Log("d");
			Debug.Log(limitPos);
		}*/
	}
}
