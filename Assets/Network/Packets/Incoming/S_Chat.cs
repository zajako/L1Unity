using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class S_Chat : ServerPacketBase 
{
	NetCon _conn;

	public S_Chat(NetCon conn, int opcode, byte[] data, int size) : base(data,size)
	{
		_conn = conn;

		int type;
		string message;
		uint sender;

		switch(opcode)
		{
			case OpCodes.S_CHAT_NORMAL:
				type = readByte();
				sender = readUInt();
				message = readS();
				displayChat(message, 0);
				break;
			
			case OpCodes.S_CHAT_GLOBAL:
				type = readByte();
				switch (type)
				{
					case 9:	//return from a command like .help
						message = readS();
						break;
					case 3:	//regular global chat
						message = readS();
						break;
					default:
						message = "";
						break;
				}
				displayChat(message, 0);
				break;

			case OpCodes.S_CHAT_SHOUT:
				int x, y;
				type = readByte();
				sender = readUInt();
				message = readS();
				x = readH();
				y = readH();
				displayChat("?"+message, 0);
				break;

			case OpCodes.S_CHAT_WHISPER:
				string senderName;
				senderName = readS();
				message = readS();
				displayChat("(" + senderName + ") " + message, 0);
				break;

			default:
				type = readByte();
				sender = readUInt();
				message = readS();
				Debug.Log("Received chat TYPE:" + type + ", SENDER: " + sender + ", MSG: " + message);
				break;
		}
	}

	private void displayChat(string message, int scrollval)
	{
		ChatBox chat = _conn.getChatInterface();
		chat.display(message, scrollval);
	}

}
