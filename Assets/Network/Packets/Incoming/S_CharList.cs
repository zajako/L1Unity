using UnityEngine;
using System.Collections;

public class S_CharList : ServerPacketBase
{
	public S_CharList(NetCon conn, byte[] data, int size) : base(data,size)
	{
		//Temporary Code to auto login as the first character
		conn.addCharsRcvd(1);
		if(conn.getCharsRcvd() == 1)
		{
			conn.setCharName(readS());
		}

		if(conn.getCharsRcvd() == conn.getNumChars())
		{
			Debug.Log("Logging in as ;" + conn.getCharName() + ";");

			//send C_LoginToServer Packet
			conn.send_packet(new C_LoginToServer(conn.getCharName()));
		}
		//End Temporary Code






	}
}
