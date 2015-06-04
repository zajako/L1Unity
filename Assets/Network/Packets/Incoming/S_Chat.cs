using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class S_Chat : ServerPacketBase 
{
	static int chat = 0;
	
	public void process(byte[] data, int size)
	{
		Text stuff;
		Scrollbar scrl;
		rpckts = data;
		rpacket_length = size;
		stuff = GameObject.Find("ChatText").GetComponent<Text>();
		scrl = GameObject.Find("ChatScrollbar").GetComponent<Scrollbar>();
		byte opcode = get_byte();
		switch (opcode)
		{
			case 8:
				{
				byte type;
				uint sender;
				string msg;
				type = get_byte();
				sender = get_uint();
				msg = get_string();
				Debug.Log("CHAT: [" + msg + "] CHAT");
				stuff.text += "\n" + msg;
				scrl.value = 0;
				Debug.Log("Created S_Chat packet class instance");
				}
				break;
			default:
				chat++;
				stuff.text += "\nChat: " + chat;
				Debug.Log("Received chat " + chat);
				scrl.value = 0;
				break;
		}
	}
	
}
