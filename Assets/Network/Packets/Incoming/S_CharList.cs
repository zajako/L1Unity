using UnityEngine;
using System.Collections;

public class S_CharList : ServerPacketBase
{
	public S_CharList(NetCon conn, byte[] data, int size) : base(data,size)
	{
		//Temporary Code to auto login as the first character
		// conn.addCharsRcvd(1);
		// if(conn.getCharsRcvd() == 1)
		// {
		// 	conn.setCharName(readS());
		// }

		// if(conn.getCharsRcvd() == conn.getNumChars())
		// {
		// 	Debug.Log("Logging in as ;" + conn.getCharName() + ";");

		// 	send C_LoginToServer Packet
		// 	 conn.send_packet(new C_LoginToServer(conn.getCharName()));
		// }
		//End Temporary Code

		LoginVars loginVars = GameObject.Find("loginvars").GetComponent<LoginVars>();



		CharList c = new CharList();

		string name = readS();
		string clanName = readS();
		int type = readC();
		int sex = readC();
		int lawful = readH();
		int hp = readH();
		int mp = readH();
		int ac = readC();
		int level = readC();
		int str = readC();
		int dex = readC();
		int con = readC();
		int wis = readC();
		int cha = readC();
		int intel = readC();
		int access = readC();

		c.setName(name);
		c.setClanName(clanName);
		c.setType(type);
		c.setSex(sex);
		c.setLawful(lawful);
		c.setHp(hp);
		c.setMp(mp);
		c.setAc(ac);
		c.setLevel(level);
		c.setStr(str);
		c.setDex(dex);
		c.setCon(con);
		c.setWis(wis);
		c.setCha(cha);
		c.setIntel(intel);
		c.setAccessLevel(access);

		Debug.Log("Storing Char: "+name);

		loginVars.addChar(c);



		// writeC(Opcodes.S_OPCODE_CHARLIST);
		// writeS(name);
		// writeS(clanName);
		// writeC(type);
		// writeC(sex);
		// writeH(lawful);
		// writeH(hp);
		// writeH(mp);
		// writeC(ac);
		// writeC(lv);
		// writeC(str);
		// writeC(dex);
		// writeC(con);
		// writeC(wis);
		// writeC(cha);
		// writeC(intel);
		// writeC(0);








	}
}
