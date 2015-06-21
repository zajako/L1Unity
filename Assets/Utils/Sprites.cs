using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class Sprites
{
	public static Sprites _instance;
	private List<Sprite> _allPng;
	private List<Sprite> _allImg;

	static public Sprites getInstance()
	{
		if(_instance == null)
		{
			_instance = new Sprites();
		}
		return _instance;
	}

	public Sprites()
	{
		loadSprites();
	}

	private void loadSprites()
	{
		 Sprite[] pngs = Resources.LoadAll<Sprite>("png");
		 _allPng = new List<Sprite>(pngs);

		 Sprite[] imgs = Resources.LoadAll<Sprite>("img");
		 _allImg = new List<Sprite>(imgs);
	}

	public Sprite getPng(string name)
	{
		foreach(Sprite s in _allPng)
		{
			if(String.Equals(s.name, name))
			{
				return s;
			}
		}

		return null;
	}

	public Sprite getImg(string name)
	{
		foreach(Sprite s in _allImg)
		{
			if(String.Equals(s.name, name))
			{
				return s;
			}
		}

		return null;
	}

	public Sprite[] getPngRange(int start_int, int total)
	{
		Sprite[] range = new Sprite[total];
		int i = 0;
		while(i < total)
		{
			int sprid = start_int+i;

			range[i] = getPng(sprid.ToString());
			i++;
		}

		return range;
	}

	public Sprite[] getImgRange(int start_int, int total)
	{
		Sprite[] range = new Sprite[total];
		int i = 0;
		while(i < total)
		{
			int sprid = start_int+i;

			range[i] = getImg(sprid.ToString());
			i++;
		}

		return range;
	}
}