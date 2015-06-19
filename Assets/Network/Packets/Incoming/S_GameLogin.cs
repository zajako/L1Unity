using UnityEngine;
using System.Collections;

public class S_GameLogin : ServerPacketBase
{


	public S_GameLogin(byte[] data, int size) : base(data,size)
	{
		

		int lang = readC();
		int clan_member_id = readD();
		int unknown1 = readC();
		int unknown2 = readC();

		Debug.Log("Received GameLogin From Server Lang:"+lang
					+" clanmemberid:"+clan_member_id
					+" unknown1:"+unknown1
					+" unknown2:"+unknown2);
	}
}
