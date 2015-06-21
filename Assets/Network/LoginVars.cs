using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class LoginVars : MonoBehaviour {

	public string _login;
	public string _password;
	public int _loginResult = -1;
	public CharList _char1;
	public CharList _char2;
	public CharList _char3;
	public CharList _char4;
	public CharList _char5;
	public CharList _char6;
	public CharList _char7;
	public CharList _char8;

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

	public void setChar1(CharList c)
	{
		_char1 = c;
	}

	public CharList getChar1()
	{
		return _char1;
	}

	public void setChar2(CharList c)
	{
		_char2 = c;
	}

	public CharList getChar2()
	{
		return _char2;
	}


	public void addChar(CharList c)
	{
		if(_char1 == null)
			setChar1(c);
		else if(_char2 == null)
			setChar2(c);
	}








}