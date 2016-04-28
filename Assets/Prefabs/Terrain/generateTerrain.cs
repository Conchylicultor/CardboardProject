using UnityEngine;
using System.Collections;
using System.IO;

public class generateTerrain : MonoBehaviour {
	enum GameMode {TravelMode, ExplorationMode};
	GameMode gameMode;

	public Transform road01;
	public Transform platform;
	public ExitDoor exitDoor;

	GameObject referencePlayer;
	GameObject terrain;

	Transform currentPlateform; // Reference to the current platform (to destroy it)
	Transform nextPlateform; // Reference to the created platform

	Vector3 positionCurrentPlatform; // The position of the folder in the world
	Vector3 positionNextPlatform; // The futur folder plateform to reach
	string nameNextFolder;
	float angleNextFolder;
	// From norm(posPlayer - posCurrentPlatform), we can deduce the distance of the player to the plateform
	float nextDistance; // To make the next plateform appear
	bool middleReach;

	float radiusPlatform = 50.0f; // TODO: Extract that dynamically
	float distanceBetweenPlateforms = 100.0f;
	float roadLength = 20.0f;


	/**
	 * Math utilities fcts
	 * 
	 * */
	Vector3 toCarthesian(float r, float theta, float y, Vector3 origin) {
		return origin + toCarthesian(r, theta, y);
	}

	Vector3 toCarthesian(float r, float theta, float y) {
		return new Vector3(
			r * Mathf.Cos( theta ),
			y,
			r * Mathf.Sin( theta ));
	}

	/**
	 * We create a new platform with x exit for each subfolder (+1 for parent)
	 * 
	 * */
	void loadFolder(string folderName, Vector3 position) {
		Debug.Log("Try loading folder...");
		Debug.Log(folderName);

		// Create the main platform
		nextPlateform = Instantiate(platform, position, Quaternion.identity) as Transform;
		Debug.Log(nextPlateform);

		// Extract all subfolders list
		System.IO.DirectoryInfo rootDir = new System.IO.DirectoryInfo(folderName);
		DirectoryInfo[] dirsInfo = rootDir.GetDirectories();

		// Create one exit by subfolder (".." for parent)
		int nbExit = dirsInfo.Length; // (+1 for parent ?)
		int currentExit = 0; // (+1 for parent ?)
		foreach (DirectoryInfo dir in dirsInfo) { // TODO: Put a limit. For now, lets hope ther is not 100 subfolder
			ExitDoor nextDoor = Instantiate(exitDoor, Vector3.zero, Quaternion.identity) as ExitDoor;

			float theta = 2 * Mathf.PI * (currentExit + 1) / nbExit; // The angle of the door (equially divided among the circle)

			// Convertion from cylinder to carthesian (with small elevation)
			Vector3 posDoor = toCarthesian(radiusPlatform, theta, 0.26f);

			nextDoor.transform.parent = nextPlateform; // Set parent
			nextDoor.transform.localPosition = posDoor; // Pos relative to parent
			nextDoor.transform.Rotate(new Vector3(0, 90 -360*theta/(2*Mathf.PI),0));

			// Set the text
			nextDoor.GetComponentInChildren<TextMesh>().text = dir.Name + "/";

			// Rotate the door correctly
			//Vector3 localPos = nextDoor.InverseTransformDirection(Vector3.zero-nextDoor.position);
			//localPos.y = 0;
			//Vector3 lookPos = transform.position + transform.TransformDirection(localPos);
			//nextDoor.LookAt(lookPos);

			nextDoor.terrainManager = this;

			nextDoor.folderName = dir.FullName;
			nextDoor.angle = theta;


			currentExit++;
		}

		// What about parent (And what about we try to leave the application folder)
	}

	/**
	 * Check the collisions (are we exiting the platform ?)
	 * If yes, initialize nameNextFolder, angle & cie >> go in travelMode
	 * */
	public void launchExitMode(ExitDoor exitDoor) {
		Debug.Log(exitDoor.folderName);
		Debug.Log(exitDoor.angle);

		gameMode = GameMode.TravelMode;

		nameNextFolder = exitDoor.folderName;
		angleNextFolder = exitDoor.angle;

		positionCurrentPlatform = currentPlateform.position;
		positionNextPlatform = positionCurrentPlatform + toCarthesian(radiusPlatform + distanceBetweenPlateforms,angleNextFolder,0 , positionCurrentPlatform);

		nextDistance = radiusPlatform; // Should create a new plateform imediatelly
		middleReach = false;

		loadFolder (nameNextFolder, positionNextPlatform);
	}

	// Use this for initialization
	void Start () {
		gameMode = GameMode.ExplorationMode; // We start on the big platform

		//exitDoor = Resources.Load("ExitDoor") as ExitDoor;

		terrain = GameObject.Find("Terrain"); // Get the terrain
		referencePlayer = GameObject.Find("Car"); // Get the player

		// Loading the terrain
		loadFolder(Application.dataPath, Vector3.zero);
		currentPlateform = nextPlateform; // We are currently on the platform we just created

		// TODO: Initialize currentFolder, currentFolderPos, ...
	}
	
	// Update is called once per frame
	void Update () {
		if (gameMode == GameMode.TravelMode) {
			// Compute current distance car-currentPlateform
			float currentDistance = Vector3.Distance(referencePlayer.transform.position, currentPlateform.transform.position);
			if (currentDistance > nextDistance - roadLength/2) { // We need to add a new plateform
				//Debug.Log("Creation new portion:");
				Vector3 nextRoadPosition = toCarthesian(nextDistance, angleNextFolder, 0, positionCurrentPlatform);
				Transform newPortion = Instantiate(road01, nextRoadPosition, Quaternion.identity) as Transform;
				newPortion.transform.Rotate(new Vector3(0, 90 -360*angleNextFolder/(2*Mathf.PI),0));
				newPortion.transform.parent = currentPlateform.transform;
				nextDistance += roadLength;
				Debug.Log(nextRoadPosition);
			}
			if (currentDistance > radiusPlatform + distanceBetweenPlateforms) { // We reach our destination
				Debug.Log("Platform reached");
				Destroy(currentPlateform);
				currentPlateform = nextPlateform;

				gameMode = GameMode.ExplorationMode;
			}
		}
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

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("Collision triggered! Parent!!!");
		// do something
	}
}
