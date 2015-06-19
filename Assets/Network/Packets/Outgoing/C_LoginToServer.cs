using UnityEngine;
using System.Collections;

public class C_LoginToServer : ClientPacketBase {

	public C_LoginToServer(string name)
	{
		base.Start();

		writeC(OpCodes.C_LOGINTOSERVER);
		writeS(name);
		writeD(0);
		writeD(0);
	}
}