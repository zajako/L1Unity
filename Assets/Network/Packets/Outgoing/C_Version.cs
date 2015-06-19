using UnityEngine;
using System.Collections;

public class C_Version : ClientPacketBase {

	public C_Version()
	{
		base.Start();
		int clientLanguage = 0;				//Country: 0.US 3.Taiwan 4.Janpan 5.China
		int clientVersion = 101101;		
		int serverType = 0x087f7dc2; 
		int npcVersion = 0x07cbf4d9;
		writeC(OpCodes.C_VERSION);
		writeH(0x33);
		writeC(0);
		writeD(clientLanguage);
		writeH(serverType);
		writeH(npcVersion);
		writeD(clientVersion);
	}
}
