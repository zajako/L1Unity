using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class new_char : MonoBehaviour
{

	//NetCon _con;
	//LoginVars _loginVars;

	int _currentFrame;
	int _delay = 0;
	int _gender = 0;
	int _class = 0;

	Image _character;
	Sprite[] _charFrames;
	CharSelAnimation _charAni;

	Button _okay;
	Button _cancel;
	Button _male;
	Button _female;
	Button _royal;
	Button _elf;
	Button _darkelf;
	Button _illusionist;
	Button _knight;
	Button _mage;
	Button _dragonknight;


	void Awake()
	{
		//_loginVars = GameObject.Find("loginvars").GetComponent<LoginVars>();
		//_con = Object.FindObjectOfType<NetCon>();

		_character = GameObject.Find("Character").GetComponent<Image>();
		_charAni = new CharSelAnimation(0, 0);
		_charFrames = _charAni.getFrames();

		_okay = GameObject.Find("OK").GetComponent<Button>();
		_cancel = GameObject.Find("Cancel").GetComponent<Button>();
		_male = GameObject.Find("Male").GetComponent<Button>();
		_female = GameObject.Find("Female").GetComponent<Button>();

		_royal = GameObject.Find("Royal").GetComponent<Button>();
		_elf = GameObject.Find("Elf").GetComponent<Button>();
		_darkelf = GameObject.Find("DarkElf").GetComponent<Button>();
		_illusionist = GameObject.Find("Illusionist").GetComponent<Button>();
		_knight = GameObject.Find("Knight").GetComponent<Button>();
		_mage = GameObject.Find("Mage").GetComponent<Button>();
		_dragonknight = GameObject.Find("DragonKnight").GetComponent<Button>();

		_okay.onClick.AddListener(() => {
			Debug.Log("Okay Button Pressed, trigger error handling");
		});

		_cancel.onClick.AddListener(() => {
			Debug.Log("Cancel Button Pressed, go back to char select");
			Application.LoadLevel("char_select");
		});

		_male.onClick.AddListener(() => {
			Debug.Log("Change Animation to Male");
			changeGender(CharTypes.S_MALE);
		});

		_female.onClick.AddListener(() => {
			Debug.Log("Change Animation to Female");
			changeGender(CharTypes.S_FEMALE);
		});

		_royal.onClick.AddListener(() => {
			Debug.Log("Change Animation to Royal");
			changeClass(CharTypes.T_ROYAL);
		});

		_elf.onClick.AddListener(() => {
			Debug.Log("Change Animation to Elf");
			changeClass(CharTypes.T_ELF);
		});

		_darkelf.onClick.AddListener(() => {
			Debug.Log("Change Animation to Dark Elf");
			changeClass(CharTypes.T_DELF);
		});

		_illusionist.onClick.AddListener(() => {
			Debug.Log("Change Animation to Illusionist");
			changeClass(CharTypes.T_ILL);
		});

		_knight.onClick.AddListener(() => {
			Debug.Log("Change Animation to Knight");
			changeClass(CharTypes.T_KNIGHT);
		});

		_mage.onClick.AddListener(() => {
			Debug.Log("Change Animation to Mage");
			changeClass(CharTypes.T_MAGE);
		});

		_dragonknight.onClick.AddListener(() => {
			Debug.Log("Change Animation to DragonKnight");
			changeClass(CharTypes.T_DKNIGHT);
		});

	}

	void Update()
	{
		animateCharacter();
	}

	private void changeGender(int gender)
	{
		_gender = gender;
		_currentFrame = 0;
		_delay = 0;
		_charAni = new CharSelAnimation(_class, _gender);
		_charFrames = _charAni.getFrames();
	}

	private void changeClass(int cid)
	{
		_class = cid;
		_currentFrame = 0;
		_delay = 0;
		_charAni = new CharSelAnimation(_class, _gender);
		_charFrames = _charAni.getFrames();
	}

	public void OnGUI()
	{
		
	}


	private void animateCharacter()
	{
		if(_delay >= 3)
		{
			Debug.Log("Animating Delay:"+_delay+" Current Frame:"+_currentFrame+"/"+_charFrames.Length);
			
			if(_currentFrame >= (_charFrames.Length - 1))
				_currentFrame = 0;
			else
				_currentFrame += 1;

			_character.sprite = _charFrames[_currentFrame];
			Sprites.RemoveBlack(_character.sprite);

			_delay = 0;
		}
		else
		{
			_delay += 1;
		}
	}


}