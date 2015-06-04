using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class S_Chat : ServerPacketBase 
{
	public void process(byte[] data, int size)
	{
		Text stuff;
		Scrollbar scrl;
		rpckts = data;
		rpacket_length = size;
		byte type;
		string msg;
		uint sender;
		stuff = GameObject.Find("ChatText").GetComponent<Text>();
		scrl = GameObject.Find("ChatScrollbar").GetComponent<Scrollbar>();
		byte opcode = get_byte();
		if (opcode == S_CHAT_NORMAL)
		{
			type = get_byte();
			sender = get_uint();
			msg = get_string();
			stuff.text += "\n" + msg;
			scrl.value = 0;
		}
		else if (opcode == S_CHAT_GLOBAL)
		{
			type = get_byte();
			switch (type)
			{
				case 9:	//return from a command like .help
					msg = get_string();
					stuff.text += "\n" + msg;
					scrl.value = 0;
					break;
				case 3:	//regular global chat
					msg = get_string();
					stuff.text += "\n" + msg;
					scrl.value = 0;
					break;
				default:
					break;
			}
		}
		else if (opcode == S_CHAT_SHOUT)
		{
			short x, y;
			type = get_byte();
			sender = get_uint();
			msg = get_string();
			x = get_short();
			y = get_short();
			stuff.text += "\n?" + msg;
			scrl.value = 0;
		}
		else if (opcode == S_CHAT_WHISPER)
		{
			string msg2;
			msg = get_string();
			msg2 = get_string();
			stuff.text += "\n(" + msg + ") " + msg2;
			scrl.value = 0;
		}
		else
		{
			type = get_byte();
			sender = get_uint();
			msg = get_string();
			Debug.Log("Received chat TYPE:" + type + ", SENDER: " + sender + ", MSG: " + msg);
		}
	}
	
}
