using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class LoginVars : MonoBehaviour {

	public string _login;
	public string _password;
	public int _loginResult = -1;

	public void Awake()
	{
		// Do not destroy this game object:
		DontDestroyOnLoad(this);
	}

	public void setValues(string login, string password)
	{
		_login = login;
		_password = password;
	}

	public string getLogin()
	{
		return _login;
	}

	public string getPassword()
	{
		return _password;
	}

	public void clearPassword()
	{
		_password = "";
	}

	public void setResult(int i)
	{
		_loginResult = i;
		Debug.Log("Result set to "+i);
	}

	public int getResult()
	{
		return _loginResult;
	}
}