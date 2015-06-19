using UnityEngine;
using System.Collections;

public class S_Version : ServerPacketBase
{
	public S_Version(NetCon conn, byte[] data, int size) : base(data,size)
	{
		Debug.Log("Received server version");

		//Should trigger the login screen to display here

		//Until login screen is being displayed
		conn.login_packet();
	}
}
