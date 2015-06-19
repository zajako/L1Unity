using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using UnityEngine.EventSystems;

public class ChatBox : MonoBehaviour
{
	public InputField inp = null;

	Text _ChatText;
	Scrollbar _scroll;
	NetCon _netCon;

	
	public virtual void Awake()
	{
		_ChatText = GameObject.Find("ChatText").GetComponent<Text>();
		_scroll = GameObject.Find("ChatScrollbar").GetComponent<Scrollbar>();
		_netCon = GameObject.Find("netcon").GetComponent<NetCon>();
		inp = GameObject.Find("InputField").GetComponent<InputField>();


		 // Add listener to catch the submit (when set Submit button is pressed)
		InputField.SubmitEvent submitEvent = new InputField.SubmitEvent();
		submitEvent.AddListener((value) => chat_submit(value));
		inp.onEndEdit = submitEvent;

	}

	public void display(string message, int scroll)
	{
		_ChatText.text += "\n" + message;
		_scroll.value = scroll;
	}

	public void chat_submit(string message)
	{
		Debug.Log("Submitting chat " + message);
		switch(message[0])
		{
			default:
				{
				_netCon.send_packet(new C_Chat(message));

				// C_Chat temp = gameObject.AddComponent<C_Chat>();
				// temp.send(inp.text);
				// Destroy(temp);
				}
				break;
		}
		Debug.Log("Submitting chat " + message);
		inp.text = "";
		EventSystem.current.SetSelectedGameObject(inp.gameObject, null);
		inp.OnPointerClick(new PointerEventData(EventSystem.current));
	}

}