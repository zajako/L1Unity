using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class char_select : MonoBehaviour
{

	NetCon _con;
	LoginVars _loginVars;

	int _totalChars = 0;

	bool[] _charSet = { false,false,false,false,false,false,false,false };
	Image[] _door = { null,null,null,null };
	Button[] _doorButton = { null,null,null,null };
	Sprite[][] _doorFrames  = { null,null,null,null,null,null,null,null };
	CharSelAnimation[] _doorAni = { null,null,null,null,null,null,null,null };
	SpriteState[] _doorState = { new SpriteState(),new SpriteState(),new SpriteState(),new SpriteState() };

	Button _okay;
	Button _cancel;
	Button _delete;

	int _selectedDoor = 0;
	int _currentFrame;
	int _delay = 0;
	int _page = 1;

	// Use this for initialization
	void Awake()
	{
		_loginVars = GameObject.Find("loginvars").GetComponent<LoginVars>();
		_con = Object.FindObjectOfType<NetCon>();

		_door[0]	=	GameObject.Find("Door1").GetComponent<Image>();
		_door[1]	=	GameObject.Find("Door2").GetComponent<Image>();
		_door[2]	=	GameObject.Find("Door3").GetComponent<Image>();
		_door[3]	=	GameObject.Find("Door4").GetComponent<Image>();
		_doorButton[0]	=	GameObject.Find("Door1").GetComponent<Button>();
		_doorButton[1]	=	GameObject.Find("Door2").GetComponent<Button>();
		_doorButton[2]	=	GameObject.Find("Door3").GetComponent<Button>();
		_doorButton[3]	=	GameObject.Find("Door4").GetComponent<Button>();
		_okay = GameObject.Find("OK").GetComponent<Button>();
		_cancel = GameObject.Find("Cancel").GetComponent<Button>();
		_delete = GameObject.Find("Delete").GetComponent<Button>();

		//Set all Doors to New Char accept 7 and 8
		_doorAni[0] = new CharSelAnimation(-1, -1);
		_doorFrames[0] = _doorAni[0].getFrames();
		_doorAni[1] = new CharSelAnimation(-1, -1);
		_doorFrames[1] = _doorAni[0].getFrames();
		_doorAni[2] = new CharSelAnimation(-1, -1);
		_doorFrames[2] = _doorAni[0].getFrames();
		_doorAni[3] = new CharSelAnimation(-1, -1);
		_doorFrames[3] = _doorAni[0].getFrames();


		_totalChars = _con.getNumChars();

		for(int x = 0; x < 4; x++)
		{
			idleDoor(x);
		}


		_doorButton[0].onClick.AddListener(() => {
			clickDoor(0);
		});

		_doorButton[1].onClick.AddListener(() => {
			clickDoor(1);
		});

		_doorButton[2].onClick.AddListener(() => {
			clickDoor(2);
		});

		_doorButton[3].onClick.AddListener(() => {
			clickDoor(3);
		});

		_okay.onClick.AddListener(() => {
			chooseCharacter(_selectedDoor);
		});

		_cancel.onClick.AddListener(() => {
			Debug.Log("Cancel Clicked");

			//TODO send logout packet

			Application.LoadLevel("opening");
		});

		_delete.onClick.AddListener(() => {
			Debug.Log("Delete Clicked");
			//send delete packet
		});


	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!_charSet[0])
		{
			if(_loginVars.getChar(0) != null)
			{
				_charSet[0] = true;
				_doorAni[0] = new CharSelAnimation(_loginVars.getChar(0).getType(), _loginVars.getChar(0).getSex());
				_doorFrames[0] = _doorAni[0].getFrames();
				idleDoor(0);
				selectCharacter(_loginVars.getChar(0));
			}
		}
		if(!_charSet[1])
		{
			if(_loginVars.getChar(1) != null)
			{
				_charSet[1] = true;
				
				_doorAni[1] = new CharSelAnimation(_loginVars.getChar(1).getType(), _loginVars.getChar(1).getSex());
				_doorFrames[1] = _doorAni[1].getFrames();
				idleDoor(1);
			}
		}		
	}

	public void OnGUI()
	{
		animateDoor(_selectedDoor);
	}

	private void animateDoor(int doorid)
	{
		if(_delay >= 3)
		{
			//Debug.Log("Animating Delay:"+_delay+" for door:"+doorid+" Current Frame:"+_currentFrame+"/"+_doorFrames[doorid].Length);
			
			if(_currentFrame >= (_doorFrames[doorid].Length - 1))
				_currentFrame = 0;
			else
				_currentFrame += 1;

			_door[doorid].sprite = _doorFrames[doorid][_currentFrame];
			Sprites.RemoveBlack(_door[doorid].sprite);

			_delay = 0;
		}
		else
		{
			_delay += 1;
		}
	}

	private void idleDoor(int doorid)
	{
		if(_door[doorid] == null)
		{
			return;
		}

		string idleState = _doorAni[doorid].getUnselectFrame().ToString();
		string hoverState = _doorAni[doorid].getHoverFrame().ToString();
		string deleteState = _doorAni[doorid].getDeleteFrame().ToString();

		_door[doorid].sprite = Sprites.getInstance().getPng(idleState);
		_doorState[doorid].disabledSprite = Sprites.getInstance().getPng(deleteState);
		_doorState[doorid].highlightedSprite = Sprites.getInstance().getPng(hoverState);
		_doorState[doorid].pressedSprite = Sprites.getInstance().getPng(hoverState);

		Sprites.RemoveBlack(_door[doorid].sprite);
		Sprites.RemoveBlack(_doorState[doorid].disabledSprite);
		Sprites.RemoveBlack(_doorState[doorid].highlightedSprite);
		Sprites.RemoveBlack(_doorState[doorid].pressedSprite);

		_doorButton[doorid].spriteState = _doorState[doorid]; 
	}

	private void clickDoor(int doorid)
	{
		if(doorid == _selectedDoor)
		{
			chooseCharacter(doorid);
		}


		for(int x = 0; x < 4; x++)
		{
			idleDoor(x);
		}

		_doorButton[doorid].spriteState = new SpriteState();

		_selectedDoor = doorid;
		Debug.Log("Door "+doorid+" Clicked");
		_currentFrame = 0;
		_delay = 0;


		selectCharacter(_loginVars.getChar(doorid));
	}




	public void selectCharacter(CharList c)
	{
		if(c != null)
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
		else
		{
			setCharName("");
			setClanName("");
			setClassName("");
			setAlignment(0);
			setHp(0);
			setMp(0);
			setAc(0);
			setLevel(0);
			setStr(0);
			setDex(0);
			setCon(0);
			setWis(0);
			setCha(0);
			setIntel(0);
			setBirthday("");
		}
		
	}

	public void chooseCharacter(int i)
	{
		CharList c = _loginVars.getChar(i);
		if(c != null)
		{
			Debug.Log("Character Chosen: "+c.getName());
			_con.send_packet(new C_LoginToServer(c.getName()));
			_con.setCharName(c.getName());
			Application.LoadLevel("login");
		}
		else
		{
			Debug.Log("Goto New Character Page");
		}
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

	private void generateAnimations()
	{





		//CharSelAnimation m_royal = new CharSelAnimation("Male Royal", CharTypes.S_MALE, CharTypes.T_ROYAL);
		//CharSelAnimation new_char = new CharSelAnimation("New Char", 0, 0);


		//new_char : 0 / 25
		//m_royal: 714/85 u:799 d:800 h:801
		//f_royal: 629/82 u: 711 d: 712 h: 713
		//f_knight : 315/60 u:375 d:376 h:377
		//m_knight : 378/71 u: 449 d: 450 h: 451
		//f_darkelf : 25/62 u: 87 d: 88 h: 89
		//m_darkelf : 90/73 u: 163 d: 164 h: 165
		//f_elf : 166/76 u: 242 d: 243 h: 244
		//m_elf : 245/67 u: 312 d: 313 h: 314
		//f_mage : 452/76 u:528 d: 529 h:530
		//m_mage : 531/95 u 626 d: 627 h: 628
		//m_dragonKnight : 841/64
		//f_dragonKNight : 908/58
		//m_illusionist : 968/69
		//f_illusionist : 1039/87
		
	}

}
