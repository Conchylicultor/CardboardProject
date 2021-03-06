﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Vehicles.Car;

public class NetworkedPlayerScript : NetworkBehaviour {

	private CarController m_Car;

	private bool isWheel = false;

	// Use this for initialization
	void Start () {
		isWheel = GlobalVarsScript.isWheel;

		if(!isWheel)
		{
			// We are the car, so let's load the car in the local var
			GameObject car = GameObject.Find ("Car");
			m_Car = car.GetComponent<CarController>();

			Debug.Log ("done setting car");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
		{
			return;
		}

		if (!isWheel) 
		{
			return;
		}

		//Debug.Log ("sending");
						
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");
		float handbrake = 0f;
#if !MOBILE_INPUT
		handbrake = CrossPlatformInputManager.GetAxis("Jump");
#else
		//m_Car.Move(h, v, v, 0f);
#endif

		RpcControlCar(h, v, handbrake);
	}

	[ClientRpc]
	void RpcControlCar(float h, float v, float handbrake)
	{
		//Debug.Log ("rijden toet toet");
		if (!isWheel && m_Car) {
			m_Car.Move(h, v, v, handbrake);
			//Debug.Log ("rijden toet toet 123 ");
		}
	}
}
