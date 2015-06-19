using UnityEngine;
using System.Collections;

public class C_KeepAlive : ClientPacketBase {

	public C_KeepAlive()
	{
		base.Start();
		Debug.Log("Sending KeepAlive Packet");
		writeC(OpCodes.C_KEEPALIVE);
		writeD(0);
		writeD(0);
	}
}