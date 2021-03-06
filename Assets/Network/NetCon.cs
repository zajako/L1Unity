﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using System.IO;
using UnityEngine.EventSystems;
using System.ComponentModel;

public class NetCon : MonoBehaviour {
	public Rigidbody player;
	public InputField inp;
	NetworkStream stream;
	string id;
	System.Byte[] rpckts;
	int rpckt_offset;
	int rpacket_length;
	Mutex send_packets;
	Mutex rcv_packets;
	
	System.Byte[] spckts;
	int spckt_offset;
	
	bool has_key;
	System.Byte[] rcv_key;
	System.Byte[] snd_key;

	public static NetCon _instance;

	public static NetCon instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<NetCon>();
 
                //Tell unity not to destroy this object when loading a new scene!
                DontDestroyOnLoad(_instance.gameObject);
            }
 
            return _instance;
        }
    }
 

	private BackgroundWorker _backgroundWorker;
	bool worker_running;

	void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		read_packet();
		worker_running = false;
	}

	public void Awake()
	{
		if(_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if(this != _instance)
                Destroy(this.gameObject);
        }

        Debug.Log("Starting EventListener");
		rpckt_offset = 0;
		rpckts = new System.Byte[65536];
		spckts = new System.Byte[65536];
		has_key = false;
		rcv_key = new System.Byte[8];
		snd_key = new System.Byte[8];
		send_packets = new Mutex();
		rcv_packets = new Mutex(true);

		

		_backgroundWorker = new BackgroundWorker();
		_backgroundWorker.DoWork += backgroundWorker_DoWork;
		worker_running = true;
		_backgroundWorker.RunWorkerAsync();
		connect();
	}
	
	public static string ByteArrayToString(byte[] ba, int packet_length)
	{
	  StringBuilder hex = new StringBuilder(ba.Length * 2);
	  for (int i = 0; i < packet_length; i++)
	    hex.AppendFormat("{0:x2} ", ba[i]);
	  return hex.ToString();
	}
	
	public void setsize()
	{
		if (spckt_offset >= 2)
		{
			spckts[0] = (byte)(spckt_offset&0xFF);
			spckts[1] = (byte)((spckt_offset>>8)&0xFF);
		}
	}
	
	public void reset()
	{
		send_packets.WaitOne();
		spckt_offset = 2;
	}
	
	byte get_byte()
	{
		return rpckts[rpckt_offset++];
	}
	
	short get_short()
	{
		short ret = 0;
		ret = (short)(rpckts[rpckt_offset] |
				rpckts[rpckt_offset+1]<<8);
		rpckt_offset += 2;
		return ret;
	}
	
	ushort get_ushort()
	{
		ushort ret = 0;
		ret = (ushort)(rpckts[rpckt_offset] |
				rpckts[rpckt_offset+1]<<8);
		rpckt_offset += 2;
		return ret;
	}
	
	int get_int()
	{
		int ret = 0;
		ret = rpckts[rpckt_offset] |
				rpckts[rpckt_offset+1]<<8 |
				rpckts[rpckt_offset+2]<<16 |
				rpckts[rpckt_offset+3]<<24;
		rpckt_offset += 4;
		return ret;
	}
	
	uint get_uint()
	{
		uint ret = 0;
		ret = (uint)(rpckts[rpckt_offset] |
				rpckts[rpckt_offset+1]<<8 |
				rpckts[rpckt_offset+2]<<16 |
				rpckts[rpckt_offset+3]<<24);
		rpckt_offset += 4;
		return ret;
	}
	
	string get_string()
	{
		int i;
		for (i = 0; rpckts[i+rpckt_offset] != 0; i++);
		rpckt_offset += i+1;
		return Encoding.UTF8.GetString(rpckts, rpckt_offset-i-1, i);
	}
		
	public void add_byte(byte a)
	{
		spckts[spckt_offset] = a;
		spckt_offset++;
	}
	
	public void add_short(short a)
	{
		spckts[spckt_offset] = (byte)(a&0xFF);
		spckts[spckt_offset+1] = (byte)((a>>8)&0xFF);
		spckt_offset += 2;
	}
	
	public void add_ushort(ushort a)
	{
		spckts[spckt_offset] = (byte)(a&0xFF);
		spckts[spckt_offset+1] = (byte)((a>>8)&0xFF);
		spckt_offset += 2;
	}
	
	public void add_int(int a)
	{
		spckts[spckt_offset] = (byte)(a&0xFF);
		spckts[spckt_offset+1] = (byte)((a>>8)&0xFF);
		spckts[spckt_offset+2] = (byte)((a>>16)&0xFF);
		spckts[spckt_offset+3] = (byte)((a>>24)&0xFF);
		spckt_offset += 4;
	}
	
	public void add_uint(uint a)
	{
		spckts[spckt_offset] = (byte)(a&0xFF);
		spckts[spckt_offset+1] = (byte)((a>>8)&0xFF);
		spckts[spckt_offset+2] = (byte)((a>>16)&0xFF);
		spckts[spckt_offset+3] = (byte)((a>>24)&0xFF);
		spckt_offset += 4;
	}
	
	public void add_string(string a)
	{
		for (int i = 0; i < a.Length; i++)
			spckts[spckt_offset+i] = (byte)a[i];
		spckt_offset += a.Length;
		spckts[spckt_offset] = 0;
		spckt_offset++;
	}
	
	void init_key(uint seed)
	{
		uint key = 0x930FD7E2;
		uint[] big_key = new uint[2];
		big_key[0] = seed;
		big_key[1] = key;
		
		uint rotrParam = big_key[0] ^ 0x9C30D539;
		big_key[0] = (rotrParam>>13) | (rotrParam<<19);	//rotate right by 13 bits
		big_key[1] = big_key[0] ^ big_key[1] ^ 0x7C72E993;
		
		snd_key[0] = (Byte)((big_key[0]) & 0xFF);
		snd_key[1] = (Byte)((big_key[0]>>8) & 0xFF);
		snd_key[2] = (Byte)((big_key[0]>>16) & 0xFF);
		snd_key[3] = (Byte)((big_key[0]>>24) & 0xFF);
		snd_key[4] = (Byte)((big_key[1]) & 0xFF);
		snd_key[5] = (Byte)((big_key[1]>>8) & 0xFF);
		snd_key[6] = (Byte)((big_key[1]>>16) & 0xFF);
		snd_key[7] = (Byte)((big_key[1]>>24) & 0xFF);
	
		Array.Copy(snd_key, rcv_key, 8);
	
		has_key = true;
	}
	
	void decrypt()
	{
		if (rpacket_length != 0)
		{
			System.Byte b3 = rpckts[3];
			rpckts[3] ^= rcv_key[2];
	
			System.Byte b2 = rpckts[2];
			rpckts[2] ^= (System.Byte)(b3 ^ rcv_key[3]);
	
			System.Byte b1 = rpckts[1];
			rpckts[1] ^= (System.Byte)(b2 ^ rcv_key[4]);
	
			System.Byte k = (System.Byte)(rpckts[0] ^ b1 ^ rcv_key[5]);
			rpckts[0] = (System.Byte)(k ^ rcv_key[0]);
	
			for (int i = 1; i < rpacket_length; i++) 
			{
				System.Byte t = rpckts[i];
				rpckts[i] ^= (System.Byte)(rcv_key[i & 7] ^ k);
				k = t;
			}
		}
	}
	
	void encrypt(byte[] pckt)
	{
		//Debug.Log("Encryption key " + ByteArrayToString(snd_key, 8));
		if (pckt.Length != 0)
		{
			pckt[2] ^= snd_key[0];
			
			for (int i = 3; i < pckt.Length; i++)
			{
				pckt[i] ^= (byte)(snd_key[(i-2) & 7] ^ pckt[i-1]);
			}
			pckt[5] ^= (byte)(snd_key[2]);
			pckt[4] ^= (byte)(snd_key[3] ^ pckt[5]);
			pckt[3] ^= (byte)(snd_key[4] ^ pckt[4]);
			pckt[2] ^= (byte)(snd_key[5] ^ pckt[3]);
		}
	}
	
	void encrypt()
	{
		//Debug.Log("Encryption key " + ByteArrayToString(snd_key, 8));
		if (spckt_offset != 0)
		{
			spckts[2] ^= snd_key[0];
			
			for (int i = 3; i < spckt_offset; i++)
			{
				spckts[i] ^= (byte)(snd_key[(i-2) & 7] ^ spckts[i-1]);
			}
			spckts[5] ^= (byte)(snd_key[2]);
			spckts[4] ^= (byte)(snd_key[3] ^ spckts[5]);
			spckts[3] ^= (byte)(snd_key[4] ^ spckts[4]);
			spckts[2] ^= (byte)(snd_key[5] ^ spckts[3]);
		}
	}
	
	void change_rcv_key(System.Byte[] data)
	{
		rcv_key[0] ^= data[0];
		rcv_key[1] ^= data[1];
		rcv_key[2] ^= data[2];
		rcv_key[3] ^= data[3];
	
		uint temp = ((uint)rcv_key[4]) |
					(uint)rcv_key[5]<<8 |
					(uint)rcv_key[6]<<16 |
					(uint)rcv_key[7]<<24;
		temp += 0x287EFFC3;
		rcv_key[4] = (System.Byte)(temp&0xFF);
		rcv_key[5] = (System.Byte)(temp>>8&0xFF);
		rcv_key[6] = (System.Byte)(temp>>16&0xFF);
		rcv_key[7] = (System.Byte)(temp>>24&0xFF);
	}
	
	void change_snd_key(System.Byte[] data)
	{
		snd_key[0] ^= data[0];
		snd_key[1] ^= data[1];
		snd_key[2] ^= data[2];
		snd_key[3] ^= data[3];
	
		uint temp = ((uint)snd_key[4]) |
					(uint)snd_key[5]<<8 |
					(uint)snd_key[6]<<16 |
					(uint)snd_key[7]<<24;
		temp += 0x287EFFC3;
		snd_key[4] = (System.Byte)(temp&0xFF);
		snd_key[5] = (System.Byte)(temp>>8&0xFF);
		snd_key[6] = (System.Byte)(temp>>16&0xFF);
		snd_key[7] = (System.Byte)(temp>>24&0xFF);
	}

	public void send_packet(ClientPacketBase pckt)
	{
		send_packet(pckt.getBytes());
	}
	
	public void send_packet(byte[] pckt)
	{
		System.Byte[] data = new System.Byte[4];
		pckt[0] = (byte)(pckt.Length&0xFF);
		pckt[1] = (byte)((pckt.Length>>8)&0xFF);
		data[0] = pckt[2];
		data[1] = pckt[3];
		data[2] = pckt[4];
		data[3] = pckt[5];
		
		//Debug.Log("Sending packet " + ByteArrayToString(pckt, pckt.Length));
		send_packets.WaitOne();
		encrypt(pckt);
		
		change_snd_key(data);
		//Debug.Log("Sending Epacket " + ByteArrayToString(pckt, pckt.Length));
		stream.Write(pckt, 0, pckt.Length);
		send_packets.ReleaseMutex();
	}
	
	void send_packet()
	{
		System.Byte[] data = new System.Byte[4];
		setsize();
		data[0] = spckts[2];
		data[1] = spckts[3];
		data[2] = spckts[4];
		data[3] = spckts[5];
		
		//Debug.Log("Sending packet " + ByteArrayToString(spckts, spckt_offset));
		encrypt();
		change_snd_key(data);
		//Debug.Log("Sending Epacket " + ByteArrayToString(spckts, spckt_offset));
		stream.Write(spckts, 0, spckt_offset);
		send_packets.ReleaseMutex();
	}
	
	void cancel_packet()
	{
		send_packets.ReleaseMutex();
	}
	
	public void process_packet_contents()
	{
		byte opcode = get_byte();

		if (opcode == 65) //key packet
		{
			uint seed = get_uint();
			init_key(seed);
			Debug.Log("Received encryption seed");
			send_packet(new C_Version());
		}
		else
		{
			new PacketHandler(this, opcode, rpckts, rpacket_length);
		}
		
	}
	
	public void connect() {
		Debug.Log("Connection");
		TcpClient client = new TcpClient ("207.192.73.73", 2000);
		stream = client.GetStream();
		Debug.Log("Connected");
		rcv_packets.ReleaseMutex();
	}
	
	public void disconnect() {
		stream.Dispose();
	}
	
	public void read_packet()
	{
		rcv_packets.WaitOne();
		int bytes_read;
		rpckt_offset = 0;
		while (rpckt_offset < 2)
		{
			bytes_read = stream.Read(rpckts, rpckt_offset, 2-rpckt_offset);
			if (bytes_read > 0)
				rpckt_offset += bytes_read;
		}
		rpckt_offset = 0;
		rpacket_length = (rpckts[0] | rpckts[1]<<8) - 2;
		
		while (rpckt_offset < rpacket_length)
		{
			bytes_read = stream.Read(rpckts, rpckt_offset, rpacket_length-rpckt_offset);
			if (bytes_read > 0)
				rpckt_offset += bytes_read;
		}
		
		if (has_key)
		{
			System.Byte[] data = new System.Byte[4];
			decrypt();
			data[0] = rpckts[0];
			data[1] = rpckts[1];
			data[2] = rpckts[2];
			data[3] = rpckts[3];
			change_rcv_key(data);
		}
		rpckt_offset = 0;
		rcv_packets.ReleaseMutex();
	}

	void Update () {
		if(!worker_running && !_backgroundWorker.IsBusy)
		{
			process_packet_contents();
			worker_running = true;
			_backgroundWorker.RunWorkerAsync();
		}
	}


	ChatBox _chat_interface;

	public ChatBox getChatInterface()
	{
		if(!_chat_interface)
		{
			_chat_interface = gameObject.AddComponent<ChatBox>();
		}

		return _chat_interface;
	}

	int _num_chars = 0;

	public void setNumChars(int i)
	{
		_num_chars = i;
	}

	public int getNumChars()
	{
		return _num_chars;
	}

	int _chars_rcvd = 0;

	public void addCharsRcvd(int i)
	{
		_chars_rcvd += i;
	}

	public void setCharsRcvd(int i)
	{
		_chars_rcvd = i;
	}

	public int getCharsRcvd()
	{
		return _chars_rcvd;
	}

	string _char_name = "";

	public void setCharName(string name)
	{
		_char_name = name;
	}

	public string getCharName()
	{
		return _char_name;
	}

	void OnApplicationQuit()
	{
		disconnect();
    }
}
