using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Vars : NetworkBehaviour {

	public bool isWheel = false;

	// Use this for initialization
	void Start () {
	
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

	public void sendTheLolMessage()
	{
		CmdLol ();
	}

	[Command]
	void CmdLol()
	{
		Debug.Log ("From cmdLol");
	}
}
