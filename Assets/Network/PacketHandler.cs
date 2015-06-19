using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class PacketHandler
{


	public PacketHandler(NetCon conn, int opcode, byte[] data, int length)
	{
		switch(opcode)
		{
			case OpCodes.S_VERSION:
				new S_Version(conn, data, length);
				break;

			case OpCodes.S_SERVER_MESSAGE:
				// new S_ServerMessage(data, length);
				break;

			case OpCodes.S_CHAR_AMOUNT:
				new S_CharAmount(conn, data,length);
				break;

			case OpCodes.S_CHAR_LIST:
				new S_CharList(conn, data, length);
				break;

			case OpCodes.S_CHAT_NORMAL:
			case OpCodes.S_CHAT_GLOBAL:
			case OpCodes.S_CHAT_WHISPER:
			case OpCodes.S_CHAT_SHOUT:
				new S_Chat(conn, opcode, data, length);
				break;

			case OpCodes.S_DISCONNECT:
				//new S_Disconnect(data, length);
				Debug.Log("Disconnected");
				break;

			case OpCodes.S_LOGINRESULT:
				new S_LoginResult(conn, data, length);
				break;

			case OpCodes.S_NEWS:
				new S_News(conn, data, length);
				break;

			case OpCodes.S_GAMELOGIN:
				new S_GameLogin(data, length);
				break;

			default:
				Debug.Log("Unknown packet (" + data[0] + ") " + NetCon.ByteArrayToString(data, length));
				break;

		}
	}
}
