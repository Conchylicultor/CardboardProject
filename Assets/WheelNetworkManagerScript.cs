using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WheelNetworkManagerScript : MonoBehaviour {

	public NetworkManager manager;

	// Use this for initialization
	void Start () {
		if (GlobalVarsScript.isInternet) {
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
	
	// Update is called once per frame
	void Update () {
		if (GlobalVarsScript.isInternet) {
			Debug.Log (manager.matchMaker);
			if (manager.matchMaker == null) {
				manager.StartMatchMaker ();
			}
			if (manager.matchMaker != null && manager.matchInfo == null) {
				if (manager.matches == null) {
					manager.matchMaker.CreateMatch (manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
				}
			}
		} else {
			if (!NetworkServer.active) {
				Debug.Log ("Start LAN");
				manager.StartHost();
			}
		}
	}
}
