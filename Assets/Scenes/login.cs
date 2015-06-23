using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class login : MonoBehaviour {
	// Use this for initialization
	NetCon _con;
	LoginVars _loginVars;
	ChatBox _chatInterface;

	void Awake () {
		Debug.Log ("Login screen, attempting to login");

		_loginVars = GameObject.Find("loginvars").GetComponent<LoginVars>();
		_con = Object.FindObjectOfType<NetCon>();

		_chatInterface = _con.getChatInterface();
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnApplicationQuit() {

    }
}
