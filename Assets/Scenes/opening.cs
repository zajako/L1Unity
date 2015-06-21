using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.ComponentModel;

public class opening : MonoBehaviour {

	EventSystem system;
	Image _bg;
	Sprite[] _bgFrames;
	int _currentFrame;
	int _lastFrame;
	int _delay = 0;
	NetCon _con;
	LoginVars _loginVars;

	bool loginstarted = false;

	// Use this for initialization
	void Awake () {
		Debug.Log("Startup screen");
		system = EventSystem.current;
		_bg = GameObject.Find("Background").GetComponent<Image>();
		_bgFrames = Sprites.getInstance().getPngRange(1825,18);
		_loginVars = GameObject.Find("loginvars").GetComponent<LoginVars>();
		_con = Object.FindObjectOfType<NetCon>();
	}
	
	// Update is called once per frame
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
        {
			
        	Selectable next = null;
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
				if(next==null)
					next = system.lastSelectedGameObject.GetComponent<Selectable>();
			}
			else
			{
				next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
				if(next==null)
					next = system.firstSelectedGameObject.GetComponent<Selectable>();
			}
			if (next != null)
			{
				InputField inputfield = next.GetComponent<InputField>();
				if(inputfield != null)
					inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret
				system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
			}
		}

		if(loginstarted)
		{
			switch(_loginVars.getResult())
			{
				case 0:
					_con.send_packet(new C_LoginOK());
					Application.LoadLevel("char_select");
				break;
			}
			
		}


	}

	public void OnGUI()
	{

		animateBG();
		
		InputField login = GameObject.Find("Login").GetComponent<InputField>();
		InputField password = GameObject.Find("Password").GetComponent<InputField>();

		if(login.isFocused && login.text != "" && Input.GetKey(KeyCode.Return))
		{
 			something();
		}

		if(password.isFocused && password.text != "" && Input.GetKey(KeyCode.Return))
		{
 			something();
		}
	}

	private void animateBG()
	{
		if(_delay >= 10)
		{
			if(_currentFrame >= (_bgFrames.Length - 1))
				_currentFrame = 0;
			else
				_currentFrame += 1;

			_bg.sprite = _bgFrames[_currentFrame];

			_delay = 0;
		}
		else
		{
			_delay += 1;
		}


		
	}
	
	public void something()
	{
		if(!loginstarted)
		{
			loginstarted = true;
			InputField login = GameObject.Find("Login").GetComponent<InputField>();
			InputField password = GameObject.Find("Password").GetComponent<InputField>();

			

			_loginVars.setValues(login.text,password.text);
			Debug.Log("Login Button Pressed U:" + login.text + " P:"+password.text);

			_con.send_packet(new C_AuthLogin(_loginVars.getLogin(),_loginVars.getPassword()));


			// 
		}

		
	}


	// 

}
