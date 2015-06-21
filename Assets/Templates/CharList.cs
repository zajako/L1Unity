using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class CharList
{
	string _charname = "";
	string _clanName = "";
	public int _type = -1;
	public int _sex;
	public int _lawful;
	public int _hp;
	public int _mp;
	public int _ac;
	public int _level;
	public int _str;
	public int _dex;
	public int _con;
	public int _wis;
	public int _cha;
	public int _intel;
	public int _accessLevel;

	public void setName(string name)
	{
		_charname = name;
	}

	public string getName()
	{
		return _charname;
	}

	public void setClanName(string name)
	{
		_clanName = name;
	}

	public string getClanName()
	{
		return _clanName;
	}

	public void setType(int i)
	{
		_type = i;
	}

	public int getType()
	{
		return _type;
	}

	public void setSex(int i)
	{
		_sex = i;
	}

	public int getSex()
	{
		return _sex;
	}

	public void setLawful(int i)
	{
		_lawful = i;
	}

	public int getLawful()
	{
		return _lawful;
	}

	public void setHp(int i)
	{
		_hp = i;
	}

	public int getHp()
	{
		return _hp;
	}

	public void setMp(int i)
	{
		_mp = i;
	}

	public int getMp()
	{
		return _mp;
	}

	public void setAc(int i)
	{
		_ac = i;
	}

	public int getAc()
	{
		return _ac;
	}

	public void setLevel(int i)
	{
		_level = i;
	}

	public int getLevel()
	{
		return _level;
	}

	public void setStr(int i)
	{
		_str = i;
	}

	public int getStr()
	{
		return _str;
	}

	public void setDex(int i)
	{
		_dex = i;
	}

	public int getDex()
	{
		return _dex;
	}

	public void setCon(int i)
	{
		_con = i;
	}

	public int getCon()
	{
		return _con;
	}

	public void setWis(int i)
	{
		_wis = i;
	}

	public int getWis()
	{
		return _wis;
	}

	public void setCha(int i)
	{
		_cha = i;
	}

	public int getCha()
	{
		return _cha;
	}

	public void setIntel(int i)
	{
		_intel = i;
	}

	public int getIntel()
	{
		return _intel;
	}

	public void setAccessLevel(int i)
	{
		_accessLevel = i;
	}

	public int getAccessLevel()
	{
		return _accessLevel;
	}
}