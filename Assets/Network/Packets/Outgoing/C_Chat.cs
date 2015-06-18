using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class C_Chat : ClientPacketBase
{

	public C_Chat(string msg)
	{
		base.Start();
		send(msg);
	}
	
	public void send(string msg)
	{
		switch (msg[0])
		{
			case '"':
				{
				int i;
				for (i = 1; i < msg.Length; i++)
				{
					if (msg[i] == ' ')
					{
						break;
					}
				}
				writeC(C_CHAT_WHISPER);
				writeS(msg.Substring(1, i-1));
				if (i < msg.Length)
				{
					writeS(msg.Substring(i+1));
				}
				else
				{
					writeS(" ");
				}
				}
				send_packet();
				break;
			case '@':
				writeC(C_CHAT_NORMAL);
				writeC(104);
				writeS(msg.Substring(1, msg.Length-1));
				send_packet();
				break;
			case '#':
				writeC(C_CHAT_NORMAL);
				writeC(11);
				writeS(msg.Substring(1, msg.Length-1));
				send_packet();
				break;
			case '$':
				writeC(C_CHAT_NORMAL);
				writeC(12);
				writeS(msg.Substring(1, msg.Length-1));
				send_packet();
				break;
			case '&':
				writeC(C_CHAT_NORMAL);
				writeC(3);
				writeS(msg.Substring(1, msg.Length-1));
				send_packet();
				break;
			case '%':
				writeC(C_CHAT_NORMAL);
				writeC(13);
				writeS(msg.Substring(1, msg.Length-1));
				send_packet();
				break;
			case '*':
				writeC(C_CHAT_NORMAL);
				writeC(14);
				writeS(msg.Substring(1, msg.Length-1));
				send_packet();
				break;
			case '/':
				// {
				// Text stuff;
				// Scrollbar scrl;
				// stuff = GameObject.Find("ChatText").GetComponent<Text>();
				// scrl = GameObject.Find("ChatScrollbar").GetComponent<Scrollbar>();
				// stuff.text += "\nInvalid command \"" + msg.Substring(1, msg.Length-1) + "\"";
				// scrl.value = 0;
				// }
				break;
			default:
				writeC(C_CHAT_NORMAL);
				writeC(0);
				writeS(msg);
				//send_packet();
				break;
		}
	}
}
