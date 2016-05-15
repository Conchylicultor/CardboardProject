using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class generateTerrain : MonoBehaviour {
	enum GameMode {TravelMode, ExplorationMode};
	GameMode gameMode;

	public Transform road01;
	public GameObject platform;
	public ExitDoor exitDoor;

	public GameObject refClara;
	public GameObject refKevin;

	GameObject referencePlayer;
	GameObject terrain;

	GameObject currentPlateform; // Reference to the current platform (to destroy it)
	GameObject nextPlateform; // Reference to the created platform

	float angleNextFolder;
	float nextDistance; // To make the next plateform appear (We can compare to the distance of the player to the plateform)

	float radiusPlatform = 50.0f; // TODO: Extract that dynamically
	float distanceBetweenPlateforms = 100.0f;
	float roadLength = 20.0f;

	System.Random random = new System.Random();

	/**
	 * Math utilities fcts
	 * 
	 * */
	Vector3 toCarthesian(float r, float theta, float y) {
		return new Vector3(
			r * Mathf.Cos( theta ),
			y,
			r * Mathf.Sin( theta ));
	}

	List<DirectoryInfo> toList(DirectoryInfo[] array) {
		List<DirectoryInfo> list = new List<DirectoryInfo>(array.Length + 1);

		foreach (DirectoryInfo t in array) {
			list.Add (t);
		}

		return list;
	}

	float randFloat(float min, float max)
	{
		return (float)(min + (random.NextDouble() * (max - min)));
	}

	/**
	 * We create a new platform with x exit for each subfolder (+1 for parent)
	 * 
	 * */
	void loadFolder(string folderName, Vector3 position) {
		Debug.Log("Loading folder " + folderName);

		// Create the main platform
		nextPlateform = Instantiate(platform, position, Quaternion.identity) as GameObject;

		// Extract all subfolders list
		System.IO.DirectoryInfo rootDir = new System.IO.DirectoryInfo(folderName);
		List<DirectoryInfo> dirsInfo = toList(rootDir.GetDirectories());
		dirsInfo.Add (rootDir.Parent); // Add the parent directory

		// Instanciate one Kevin and Clara by files
		foreach (FileInfo file in rootDir.GetFiles()) {
			// Generate 2 random positions
			float posR = randFloat(0.0f, radiusPlatform);
			float posTheta = randFloat(0.0f, 2*Mathf.PI);

			Vector3 positionPers = toCarthesian(posR, posTheta, 2.15f);

			GameObject nextPers;
			if (random.Next () % 2 == 0) {
				nextPers = Instantiate (refClara, Vector3.zero, Quaternion.identity) as GameObject;
			} else {
				nextPers = Instantiate (refKevin, Vector3.zero, Quaternion.identity) as GameObject;
			}

			nextPers.transform.parent = nextPlateform.transform;
			nextPers.transform.localPosition = positionPers;
		}

		// Create one exit by subfolder (".." for parent)
		int nbExit = dirsInfo.Count; // (The parent is already included)
		int currentExit = 0; // (+1 for parent ?)
		foreach (DirectoryInfo dir in dirsInfo) { // TODO: Put a limit. For now, lets hope there is not 100 subfolder
			ExitDoor nextDoor = Instantiate(exitDoor, Vector3.zero, Quaternion.identity) as ExitDoor;

			float theta = 2 * Mathf.PI * (currentExit + 1) / nbExit; // The angle of the door (equially divided among the circle)

			// Convertion from cylinder to carthesian (with small elevation)
			Vector3 posDoor = toCarthesian(radiusPlatform, theta, 0.26f);

			nextDoor.transform.parent = nextPlateform.transform; // Set parent
			nextDoor.transform.localPosition = posDoor; // Pos relative to parent
			nextDoor.transform.Rotate(new Vector3(0, 90 -360*theta/(2*Mathf.PI),0));

			// Set the text
			if (rootDir.Parent.FullName == dir.FullName) { // Probably a faster way (compare reference ?, use currentExit)
				nextDoor.GetComponentInChildren<TextMesh>().text = "../";
			} else {
				nextDoor.GetComponentInChildren<TextMesh>().text = dir.Name + "/";
			}

			nextDoor.terrainManager = this;
			nextDoor.folderName = dir.FullName;
			nextDoor.angle = theta;

			currentExit++;
		}

		// What about we try to leave the application folder ??
	}

	/**
	 * Check the collisions (are we exiting the platform ?)
	 * If yes, initialize nameNextFolder, angle & cie >> go in travelMode
	 * */
	public void launchExitMode(ExitDoor exitDoor) {
		Debug.Log("Exit through: " + exitDoor.folderName);

		gameMode = GameMode.TravelMode;

		string nameNextFolder = exitDoor.folderName;
		angleNextFolder = exitDoor.angle;

		Vector3 positionNextPlatform =  // The futur folder plateform to reach
			currentPlateform.transform.position + 
			toCarthesian(2*radiusPlatform + distanceBetweenPlateforms, angleNextFolder,0);

		nextDistance = radiusPlatform; // Should create a new plateform imediatelly
		//middleReach = false;

		Destroy(nextPlateform); // Avoid creating the plateform twice
		loadFolder (nameNextFolder, positionNextPlatform);
	}

	// Use this for initialization
	void Start () {
		gameMode = GameMode.ExplorationMode; // We start on the big platform

		//exitDoor = Resources.Load("ExitDoor") as ExitDoor;

		//terrain = GameObject.Find("Terrain"); // Get the terrain (useless ?? no parenting)
		referencePlayer = GameObject.Find("Car"); // Get the player

		// Loading the terrain
		loadFolder(Application.dataPath, Vector3.zero);
		currentPlateform = nextPlateform; // We are currently on the platform we just created
		nextPlateform = null; // Otherwise, the current platform could be destroyed when destroying this (when falling)
	}
	
	// Update is called once per frame
	void Update () {
		if (gameMode == GameMode.TravelMode) {
			// If we are in travel mode, we check the distance of the player relative to the
			// current platform
			float currentDistance = Vector3.Distance(referencePlayer.transform.position, currentPlateform.transform.position);
			if (currentDistance > nextDistance - roadLength*1.5f) { // We need to add a new plateform
				//Debug.Log("Creation new portion:");
				Vector3 nextRoadPosition = currentPlateform.transform.position + toCarthesian(nextDistance, angleNextFolder, 0);
				Transform newPortion = Instantiate(road01, nextRoadPosition, Quaternion.identity) as Transform;
				newPortion.transform.Rotate(new Vector3(0, 90 -360*angleNextFolder/(2*Mathf.PI),0));
				newPortion.transform.parent = currentPlateform.transform;
				nextDistance += roadLength;
			}
			if (currentDistance > radiusPlatform + distanceBetweenPlateforms + roadLength/2) { // We reach our destination
				Debug.Log("Platform reached, try destroying previous one");
				Debug.Log(currentPlateform.transform.position);
				Debug.Log(nextPlateform.transform.position);

				Destroy(currentPlateform);
				currentPlateform = nextPlateform;
				nextPlateform = null;

				gameMode = GameMode.ExplorationMode;

				// TODO: Create enemies/obstacles on the new platform ?
			}
		}

		// Check if the player die (TODO: go to trash)
		if (referencePlayer.transform.position.y < -5.0f || Input.GetKeyUp(KeyCode.R)) {
			gameMode = GameMode.ExplorationMode; // Going back in exploration mode

			// Reset position
			referencePlayer.transform.position = currentPlateform.transform.position;
			referencePlayer.transform.rotation = new Quaternion ();

			// Reset velocity of children
			referencePlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
			referencePlayer.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

			// Avoid creation multiple times
			//Destroy(nextPlateform); // Not necessary (is reconstruct when we cross any door)
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("Collision triggered! Parent!!!");
		// do something
	}
}
