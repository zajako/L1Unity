using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class LoginVars : MonoBehaviour {

	public string _login;
	public string _password;
	public int _loginResult = -1;
	public CharList[] _char = { null,null,null,null,null,null,null,null };


	public void Awake()
	{
		// Do not destroy this game object:
		DontDestroyOnLoad(this);
	}

	public void reset()
	{
		_login = "";
		_password = "";
		_loginResult = -1;
		_char = new CharList[]{ null,null,null,null,null,null,null,null };
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

	public void setChar(int i, CharList c)
	{
		_char[i] = c;
	}

	public CharList getChar(int i)
	{
		return _char[i];
	}

	public void addChar(CharList c)
	{
		if(_char[0] == null)
			setChar(0,c);
		else if(_char[1] == null)
			setChar(1,c);
		else if(_char[2] == null)
			setChar(2,c);
		else if(_char[3] == null)
			setChar(3,c);
		else if(_char[4] == null)
			setChar(4,c);
		else if(_char[5] == null)
			setChar(5,c);
		else if(_char[6] == null)
			setChar(6,c);
		else if(_char[7] == null)
			setChar(7,c);
	}





}