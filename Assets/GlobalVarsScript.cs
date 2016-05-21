using UnityEngine;
using System.Collections;

public class GlobalVarsScript : MonoBehaviour {

	public static bool isInternet = true;
	public static bool isWheel = true;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void startWheelInternet(){
		isInternet = true;
		isWheel = true;

		Debug.Log ("??");

		Application.LoadLevel("wheelScene");
	}

	public void startWheelLocal(){
		isInternet = false;
		isWheel = true;

		Application.LoadLevel("wheelScene");
	}

	public void startCarInternet(){
		isInternet = true;
		isWheel = false;

		Application.LoadLevel("carScene");
	}

	public void startCarLocal(){
		isInternet = false;
		isWheel = false;

		Application.LoadLevel("carScene");
	}
}
