using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CarNetworkManagerScript : MonoBehaviour {

	public NetworkManager manager;

	private bool connectedToMatch = false;

	// Use this for initialization
	void Start () {
		if (GlobalVarsScript.isInternet) {
			manager.StartMatchMaker ();
			manager.SetMatchHost ("mm.unet.unity3d.com", 443, true);
			manager.matchMaker.ListMatches (0, 20, "", manager.OnMatchList);
		} else {
			manager.StartClient ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (GlobalVarsScript.isInternet) {
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
		}
	}
}
