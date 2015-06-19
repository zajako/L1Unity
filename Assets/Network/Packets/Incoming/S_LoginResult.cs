using UnityEngine;
using System.Collections;

public class S_LoginResult : ServerPacketBase
{
	const int REASON_OK = 0;
	const int REASON_INUSE = 22;
	const int REASON_EXISTS = 7;
	const int REASON_FAILED = 8;

	public S_LoginResult(NetCon conn, byte[] data, int size) : base(data,size)
	{
		int val = readByte();



		Debug.Log("Received login result: "+val);

		
		switch (val)
		{
			case REASON_OK:
				conn.send_packet(new C_LoginOK());
				break;
			case REASON_INUSE:
				//display an account already in use error
				break;
			case REASON_FAILED:
				//display a wrong username or password error
				break;
			default:
				break;
		}

	}
}
