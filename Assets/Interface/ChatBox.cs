using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class ChatBox : MonoBehaviour
{
	public InputField inp;

	Text _ChatText;
	Scrollbar _scroll;
	NetCon _netCon;

	
	public virtual void Awake()
	{
		_ChatText = GameObject.Find("ChatText").GetComponent<Text>();
		_scroll = GameObject.Find("ChatScrollbar").GetComponent<Scrollbar>();
		_netCon = GameObject.Find("netcon").GetComponent<NetCon>();
	}

	public void display(string message, int scroll)
	{
		_ChatText.text += "\n" + message;
		_scroll.value = scroll;
	}

	public void chat_submit()
	{
		Debug.Log("Submitting chat " + inp.text);
		switch(inp.text[0])
		{
			default:
				{
				_netCon.send_packet(new C_Chat(inp.text));

				// C_Chat temp = gameObject.AddComponent<C_Chat>();
				// temp.send(inp.text);
				// Destroy(temp);
				}
				break;
		}
		Debug.Log("Submitting chat " + inp.text);
		inp.text = "";
	}

}