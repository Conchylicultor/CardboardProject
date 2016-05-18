using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Vars : MonoBehaviour {

	public NetworkManager manager;
	// The GUI with the wheel/car button
	public Canvas canvas;

	public static bool isInternet = false;

	public bool isWheel = false;

	private bool connectedToMatch = false;

	void OnLevelWasLoaded(int level) {
		Debug.Log ("yooolooo" + level);

		// Car
		if (level == 1) {
		}
		// Wheel
		else if (level == 2) {
		}
	}

	// Use this for initialization
	void Start () {
		Debug.Log ("start vars" + this.isWheel + " " + isInternet);
		if (this.isWheel) {
			if (isInternet) {
				Debug.Log ("Start the matchmaker");
				manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
				manager.StartMatchMaker();
				//manager.StartHost();
			} else {
				// For LAN
				Debug.Log ("Start LAN");

				manager.StartHost();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isInternet) {
			if (!this.isWheel) {
				if (manager.matchInfo == null && !connectedToMatch && manager.matches != null) {
					var match = manager.matches [0];
					manager.matchName = match.name;
					manager.matchSize = (uint)match.currentSize;
					manager.matchMaker.JoinMatch (match.networkId, "", manager.OnMatchJoined);

					this.connectedToMatch = true;
				}

				if (manager.matchInfo != null) {
					//Debug.Log ("connecting client");
					//manager.StartClient();
				}
			} else {
				Debug.Log (manager.matchMaker);
				if (manager.matchMaker == null) {
					manager.StartMatchMaker ();
				}
				if (manager.matchMaker != null && manager.matchInfo == null) {
					if (manager.matches == null) {
						manager.matchMaker.CreateMatch (manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
					}
				}
			}
		} else {
			if (this.isWheel && !NetworkServer.active) {
				Debug.Log ("Start LAN");
				manager.StartHost();
			}
		}
	}

	public void setIsWheel(bool isWheel)
	{
		this.isWheel = isWheel;
	}

	public void loadClientScene()
	{
		Application.LoadLevel("clientScene");
	}

	/**
	 * Called when the User pressed the wheel button.
	 **/
	public void makeWHeel()
	{
		isInternet = false;

		canvas.enabled = false;
		this.setIsWheel (true);
		this.loadClientScene ();
	}

	public void makeWHeelInternet()
	{
		isInternet = true;

		canvas.enabled = false;
		this.setIsWheel (true);
		this.loadClientScene ();
	}

	/**
	 * Called when the User pressed the car button.
	 **/
	public void makeCar()
	{
		isInternet = false;

		canvas.enabled = false;
		this.setIsWheel (false);

		manager.StartClient();
	}

	public void makeCarInternet()
	{
		isInternet = true;
		canvas.enabled = false;
		this.setIsWheel (false);

		manager.StartMatchMaker();
		manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
		manager.matchMaker.ListMatches(0,20, "", manager.OnMatchList);

	}
}
