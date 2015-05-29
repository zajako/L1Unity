using UnityEngine;
using System.Collections;

public class login : MonoBehaviour {
	// Use this for initialization
	public NetCon con;
	void Start () {
		Debug.Log ("Login screen, attempting to login");
		con.connect ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
