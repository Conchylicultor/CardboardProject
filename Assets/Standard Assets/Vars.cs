using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Vars : MonoBehaviour {

	public NetworkManager manager;
	// The GUI with the wheel/car button
	public Canvas canvas;

	public bool isWheel = false;

	// Use this for initialization
	void Start () {
		if (this.isWheel) {
			// For LAN
			manager.StartHost();
		}
	}
	
	// Update is called once per frame
	void Update () {
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
		canvas.enabled = false;
		this.setIsWheel (true);
		this.loadClientScene ();
	}

	/**
	 * Called when the User pressed the car button.
	 **/
	public void makeCar()
	{
		canvas.enabled = false;
		this.setIsWheel (false);

		manager.StartClient();
	}
}
