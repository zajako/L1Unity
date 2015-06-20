using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class opening : MonoBehaviour {

	EventSystem system;
	
	// Use this for initialization
	void Start () {
		Debug.Log("Startup screen");
		system = EventSystem.current;
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
	}

	public void OnGUI()
	{
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
