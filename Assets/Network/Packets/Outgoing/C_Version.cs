using UnityEngine;
using System.Collections;

public class C_Version : ClientPacketBase {

	public override void Awake()
	{
		base.Awake();

		int clientLanguage = 0;				//Country: 0.US 3.Taiwan 4.Janpan 5.China
		int clientVersion = 0x07cbf4dd;		
		int serverType = 0x087f7dc2; 
		int npcVersion = 0x07cbf4d9;

		writeH(0);
		writeC(0);
		writeD(clientLanguage);
		writeH(serverType);
		writeH(npcVersion);
		writeD(clientVersion);

	}
}
