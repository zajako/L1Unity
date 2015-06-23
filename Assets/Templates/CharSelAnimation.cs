using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Xml;
using System;

public class CharSelAnimation
{
	private string _name = "";
	private string _imgType = "png";
	private int _startFrame;
	private int _totalFrames;
	private int _type;
	private int _gender;
	private int _deleteFrame;
	private int _unselectFrame;
	private int _hoverFrame;

	public CharSelAnimation(string name, int type, int gender)
	{
		_name = name;
		_type = type;
		_gender = gender;
	}

	public CharSelAnimation(int classType, int gender)
	{
		//Load Xml File
		TextAsset textAsset = (TextAsset)Resources.Load("data/ani/xml/charselect");  
		XmlDocument xmldoc = new XmlDocument();
		xmldoc.LoadXml(textAsset.text);
		foreach (XmlNode node in xmldoc.SelectNodes("animations/animation"))
		{
			int check_type = Convert.ToInt16(node.Attributes.GetNamedItem("class").Value);
			int check_gender = Convert.ToInt16(node.Attributes.GetNamedItem("gender").Value);

			if(check_gender == gender)
			{
				if(check_type == classType)
				{
					_name = node.Attributes.GetNamedItem("name").Value;
					_type = check_type;
					_gender = check_gender;
					_startFrame = Convert.ToInt16(node.SelectSingleNode("start").InnerText);
					_totalFrames = Convert.ToInt16(node.SelectSingleNode("total").InnerText);
					_deleteFrame = Convert.ToInt16(node.SelectSingleNode("delete").InnerText);
					_unselectFrame = Convert.ToInt16(node.SelectSingleNode("unselect").InnerText);
					_hoverFrame = Convert.ToInt16(node.SelectSingleNode("hover").InnerText);
					Debug.Log("Animation Found: "+_name);
				}
			}
		}
	}

	public void setName(string s)
	{
		_name = s;
	}

	public string getName()
	{
		return _name;
	}

	public void setImgType(string s)
	{
		_imgType = s;
	}

	public string getImgType()
	{
		return _imgType;
	}

	public void setStartFrame(int i)
	{
		_startFrame = i;
	}

	public int getStartFrame()
	{
		return _startFrame;
	}

	public void setTotalFrames(int i)
	{
		_totalFrames = i;
	}

	public int getTotalFrames()
	{
		return _totalFrames;
	}

	public void setType(int i)
	{
		_type = i;
	}

	public int getType()
	{
		return _type;
	}

	public void setGender(int i)
	{
		_gender = i;
	}

	public int getGender()
	{
		return _gender;
	}

	public void setDeleteFrame(int i)
	{
		_deleteFrame = i;
	}

	public int getDeleteFrame()
	{
		return _deleteFrame;
	}

	public void setUnselectFrame(int i)
	{
		_unselectFrame = i;
	}

	public int getUnselectFrame()
	{
		return _unselectFrame;
	}

	public void setHoverFrame(int i)
	{
		_hoverFrame = i;
	}

	public int getHoverFrame()
	{
		return _hoverFrame;
	}


	public Sprite[] getFrames()
	{
		return Sprites.getInstance().getPngRange(_startFrame,_totalFrames);
	}
}

