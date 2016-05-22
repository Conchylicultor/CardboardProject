using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections.Generic;

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
			manager.networkAddress = "192.168.43.1";
			manager.StartClient ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		/**
		if (GlobalVarsScript.isInternet) {
			if (manager.matchInfo == null && !connectedToMatch && manager.matches != null) {
				if (manager.matches.Count > 0) {
					MatchDesc match = manager.matches [manager.matches.Count - 1];

					manager.matchName = match.name;
					manager.matchSize = (uint)match.currentSize;
					manager.matchMaker.JoinMatch (match.networkId, "", manager.OnMatchJoined);

					this.connectedToMatch = true;   
				}
			}

			if (manager.matchInfo != null) {
				//Debug.Log ("connecting client");
				//manager.StartClient();
			}
		}
		**/  
	}
}
