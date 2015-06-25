using UnityEngine;
using System.Collections;

public class C_Logout : ClientPacketBase {

	public C_Logout()
	{
		base.Start();

		writeC(OpCodes.C_LOGOUT);
		writeD(0);
		writeD(0);
	}
}
