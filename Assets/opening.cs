using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class opening : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	Debug.Log("Startup screen");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void something () {
	Application.LoadLevel ("login"); 
	}
}
