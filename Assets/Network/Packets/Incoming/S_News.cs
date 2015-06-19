using UnityEngine;
using System.Collections;

public class S_News : ServerPacketBase
{
	public S_News(NetCon conn, byte[] data, int size) : base(data,size)
	{
		Debug.Log("Received server version");

		//Should trigger the News Window Popup to appear

		//Until news window is being displayed, just send the packet saying the news window is closed
		conn.send_packet(new C_MenuClick());
	}
}
