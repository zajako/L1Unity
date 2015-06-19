using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatBox : MonoBehaviour
{
	Text _ChatText;
	Scrollbar _scroll;
	
	public virtual void Awake()
	{
		_ChatText = GameObject.Find("ChatText").GetComponent<Text>();
		_scroll = GameObject.Find("ChatScrollbar").GetComponent<Scrollbar>();
	}

	public void display(string message, int scroll)
	{
		_ChatText.text += "\n" + message;
		_scroll.value = scroll;
	}

}