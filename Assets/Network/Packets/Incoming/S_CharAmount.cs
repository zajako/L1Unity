using UnityEngine;
using System.Collections;

public class S_CharAmount : ServerPacketBase
{
	public S_CharAmount(NetCon conn, byte[] data, int size) : base(data,size)
	{
		int num_chars = readByte();

		conn.setNumChars(num_chars);
		conn.setCharsRcvd(0);

		Debug.Log(num_chars + " chars total");
	}
}
