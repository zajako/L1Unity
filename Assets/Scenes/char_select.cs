using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class char_select : MonoBehaviour
{

	NetCon _con;
	LoginVars _loginVars;
	bool _char1set = false;
	bool _char2set = false;


	// Use this for initialization
	void Awake()
	{
		_loginVars = GameObject.Find("loginvars").GetComponent<LoginVars>();
		_con = Object.FindObjectOfType<NetCon>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!_char1set)
		{
			if(_loginVars.getChar1() != null)
			{
				_char1set = true;
				selectCharacter(_loginVars.getChar1());

			}
		}
	}

	public void selectCharacter(CharList c)
	{
		setCharName(c.getName());
		setClanName(c.getClanName());
		setClassName(c.getType());
		setAlignment(c.getLawful());
		setHp(c.getHp());
		setMp(c.getMp());
		setAc(c.getAc());
		setLevel(c.getLevel());
		setStr(c.getStr());
		setDex(c.getDex());
		setCon(c.getCon());
		setWis(c.getWis());
		setCha(c.getCha());
		setIntel(c.getIntel());
		setBirthday("");
	}

	public void setCharName(string s)
	{
		getTextObject("char_name").text = s;
	}

	public void setClanName(string s)
	{
		getTextObject("pledge_name").text = s;
	}

	public void setClassName(string s)
	{
		getTextObject("class_name").text = s;
	}

	public void setClassName(int i)
	{
		switch(i)
		{
			case CharTypes.T_ROYAL:
				setClassName("Royal");
			break;
			case CharTypes.T_KNIGHT:
				setClassName("Knight");
			break;
			case CharTypes.T_ELF:
				setClassName("Elf");
			break;
			case CharTypes.T_MAGE:
				setClassName("Mage");
			break;
			case CharTypes.T_DELF:
				setClassName("Dark Elf");
			break;
			case CharTypes.T_DKNIGHT:
				setClassName("Dragonknight");
			break;
			case CharTypes.T_ILL:
				setClassName("Illusionist");
			break;
			case CharTypes.T_DEL_ROYAL:
				setClassName("Royal");
			break;
			case CharTypes.T_DEL_KNIGHT:
				setClassName("Knight");
			break;
			case CharTypes.T_DEL_ELF:
				setClassName("Elf");
			break;
			case CharTypes.T_DEL_MAGE:
				setClassName("Mage");
			break;
			case CharTypes.T_DEL_DELF:
				setClassName("Dark Elf");
			break;
			case CharTypes.T_DEL_DKNIGHT:
				setClassName("Dragonknight");
			break;
			case CharTypes.T_DEL_ILL:
				setClassName("Illusionist");
			break;
			default:
				setClassName("");
			break;
		}
	}

	public void setAlignment(int i)
	{
		if(i >= 15000)
		{
			getTextObject("alignment").text = "Lawful";
		}
		else if(i < 15000 && i > -15000 )
		{
			getTextObject("alignment").text = "Neutral";
		}
		else
		{
			getTextObject("alignment").text = "Chaotic";
		}

		// getTextObject("alignment").color = Color.lightblue;
		// getTextObject("alignment").color = Color.red;
	}

	public void setHp(int i)
	{
		getTextObject("hp").text = i.ToString();
	}

	public void setMp(int i)
	{
		getTextObject("mp").text = i.ToString();
	}

	public void setAc(int i)
	{
		getTextObject("ac").text = i.ToString();
	}

	public void setLevel(int i)
	{
		getTextObject("level").text = i.ToString();
	}

	public void setStr(int i)
	{
		getTextObject("str").text = i.ToString();
	}

	public void setDex(int i)
	{
		getTextObject("dex").text = i.ToString();
	}

	public void setCon(int i)
	{
		getTextObject("con").text = i.ToString();
	}

	public void setWis(int i)
	{
		getTextObject("wis").text = i.ToString();
	}

	public void setCha(int i)
	{
		getTextObject("cha").text = i.ToString();
	}

	public void setIntel(int i)
	{
		getTextObject("int").text = i.ToString();
	}

	public void setBirthday(string i)
	{
		getTextObject("birthday").text = i.ToString();
	}

	private Text getTextObject(string name)
	{
		return GameObject.Find(name).GetComponent<Text>();
	}

}
