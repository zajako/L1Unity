using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

public class opening : MonoBehaviour {


	
	// Use this for initialization
	void Start () {
	Debug.Log("Startup screen");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void something()
	{
		
		InputField login = GameObject.Find("Login").GetComponent<InputField>();
		InputField password = GameObject.Find("Password").GetComponent<InputField>();


		LoginVars loginVars = GameObject.Find("loginvars").GetComponent<LoginVars>();

		loginVars.setValues(login.text,password.text);
		Debug.Log("Login Button Pressed U:" + login.text + " P:"+password.text);

		Application.LoadLevel("login");
	}
}
