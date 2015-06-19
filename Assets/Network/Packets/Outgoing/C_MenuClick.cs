using UnityEngine;
using System.Collections;

public class C_MenuClick : ClientPacketBase {

	public C_MenuClick()
	{
		base.Start();

		writeC(OpCodes.C_MENUCLICK);
		writeD(0);
		writeD(0);
	}
}
