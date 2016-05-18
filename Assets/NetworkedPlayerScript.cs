using UnityEngine;
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
						
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");
		float handbrake = 0f;
#if !MOBILE_INPUT
		handbrake = CrossPlatformInputManager.GetAxis("Jump");
#else
		//m_Car.Move(h, v, v, 0f);
#endif
		RpcLol(h, v, handbrake);
	}

	[ClientRpc]
	void RpcLol(float h, float v, float handbrake)
	{
		if (!isWheel) {
			m_Car.Move(h, v, v, handbrake);
		}
	}
}
