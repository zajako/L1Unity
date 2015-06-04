using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using System.IO;


public class NetCon : MonoBehaviour {
	public Rigidbody player;
	public Text stuff;
	public InputField inp;
	public Scrollbar scrl;
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
	
	string charname;
	int chat;
	
	int num_chars;
	int chars_rcvd;
	// Use this for initialization
	void Start () {
		Debug.Log("Starting EventListener");
		rpckt_offset = 0;
		rpckts = new System.Byte[65536];
		spckts = new System.Byte[65536];
		has_key = false;
		rcv_key = new System.Byte[8];
		snd_key = new System.Byte[8];
		chat = 0;
		stuff.text = "Chat started: " + chat;
		scrl.value = 0;
		send_packets = new Mutex();
		rcv_packets = new Mutex(true);
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
	
	public byte get_byte()
	{
		return rpckts[rpckt_offset++];
	}
	
	public short get_short()
	{
		short ret = 0;
		ret = (short)(rpckts[rpckt_offset] |
				rpckts[rpckt_offset+1]<<8);
		rpckt_offset += 2;
		return ret;
	}
	
	public ushort get_ushort()
	{
		ushort ret = 0;
		ret = (ushort)(rpckts[rpckt_offset] |
				rpckts[rpckt_offset+1]<<8);
		rpckt_offset += 2;
		return ret;
	}
	
	public int get_int()
	{
		int ret = 0;
		ret = rpckts[rpckt_offset] |
				rpckts[rpckt_offset+1]<<8 |
				rpckts[rpckt_offset+2]<<16 |
				rpckts[rpckt_offset+3]<<24;
		rpckt_offset += 4;
		return ret;
	}
	
	public uint get_uint()
	{
		uint ret = 0;
		ret = (uint)(rpckts[rpckt_offset] |
				rpckts[rpckt_offset+1]<<8 |
				rpckts[rpckt_offset+2]<<16 |
				rpckts[rpckt_offset+3]<<24);
		rpckt_offset += 4;
		return ret;
	}
	
	public string get_string()
	{
		int i;
		for (i = 0; rpckts[i+rpckt_offset] != 0; i++);
		rpckt_offset += i+1;
		return Encoding.UTF8.GetString(rpckts, rpckt_offset-i-1, i);
	}
	
	public void add(byte a)
	{
		spckts[spckt_offset] = a;
		spckt_offset++;
	}
	
	public void add(short a)
	{
		spckts[spckt_offset] = (byte)(a&0xFF);
		spckts[spckt_offset+1] = (byte)((a>>8)&0xFF);
		spckt_offset += 2;
	}
	
	public void add(ushort a)
	{
		spckts[spckt_offset] = (byte)(a&0xFF);
		spckts[spckt_offset+1] = (byte)((a>>8)&0xFF);
		spckt_offset += 2;
	}
	
	public void add(int a)
	{
		spckts[spckt_offset] = (byte)(a&0xFF);
		spckts[spckt_offset+1] = (byte)((a>>8)&0xFF);
		spckts[spckt_offset+2] = (byte)((a>>16)&0xFF);
		spckts[spckt_offset+3] = (byte)((a>>24)&0xFF);
		spckt_offset += 4;
	}
	
	public void add(uint a)
	{
		spckts[spckt_offset] = (byte)(a&0xFF);
		spckts[spckt_offset+1] = (byte)((a>>8)&0xFF);
		spckts[spckt_offset+2] = (byte)((a>>16)&0xFF);
		spckts[spckt_offset+3] = (byte)((a>>24)&0xFF);
		spckt_offset += 4;
	}
	
	public void add(string a)
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
	
	void login_packet()
	{
		reset();
		add((byte)12);	//login packet
		add("stupid");
		add("stupid");
		send_packet();
	}
	
	void login_check()
	{
		switch (rpckts[1])
		{
			case 3:
				reset();
				add((byte)57);	//alive packet
				add((int)0);
				add((int)0);
				send_packet();
				reset();
				add((byte)92);	//init game
				add((int)0);
				add((int)0);
				send_packet();
				break;
			default:
				break;
		}
	}
	
	public void chat_submit()
	{
		string temp = inp.text;
		Debug.Log("Submitting chat " + inp.text);
		switch('a')
		{
			default:
				reset();
				add((byte)104);	//client chat
				add((byte)0);	//regular chat
				add(temp);
				send_packet();
				break;
		}
		inp.text = "";
		Debug.Log("Submitting chat " + temp);
	}
	
	public void process_packet_contents()
	{
		byte opcode = get_byte();
		switch(opcode)
		{
			case 18:	//disconnected by server
				Debug.Log("Disconnected");
				break;
			case 42: 
			case 91: 
			case 105: //chat packets
				Debug.Log("Chat packet (" + rpckts[0] + ") " + ByteArrayToString(rpckts, rpacket_length));
				chat++;
				stuff.text += "\nChat: " + chat;
				Debug.Log("Received chat " + chat);
				scrl.value = 0;
				break;
			case 8: //normal chat
				{
					byte type;
					uint sender;
					string msg;
					type = get_byte();
					sender = get_uint();
					msg = get_string();
					stuff.text += "\n" + msg;
					scrl.value = 0;
				}
				break;
			case 10:	//server version
				Debug.Log("Received server version");
				login_packet();
				break;
			case 21:	//login
				login_check();
				break;
			case 65:	//key packet
				uint seed = ((uint)rpckts[1]) |
							((uint)rpckts[2])<<8 |
							((uint)rpckts[3])<<16 |
							((uint)rpckts[4])<<24;
				init_key(seed);
				Debug.Log("Received encryption seed");
				reset();
				add((byte)71);
				add((short)0x33);
				add((int)-1);
				add((byte)32);
				add((int)101101);
				send_packet();
				break;
			case 90:	//news packet
				reset();
				add((byte)43);	//client click packet
				add((int)0);
				add((int)0);
				send_packet();
				break;
			case 113:	//num char packet
				num_chars = rpckts[1];
				chars_rcvd = 0;
				Debug.Log(num_chars + " chars total");
				break;
			case 99:	//login char packet
				int name_len = 0;
				chars_rcvd++;
				Debug.Log("Char info packet (" + rpckts[0] + ") " + ByteArrayToString(rpckts, rpacket_length));
				if (chars_rcvd == 1)
				{
					name_len = 0;
					for (; rpckts[name_len+1] != 0; name_len++);
					Debug.Log("Char name length " + name_len);
					charname = Encoding.UTF8.GetString(rpckts, 1, name_len);
				}
				if (chars_rcvd == num_chars)
				{
					//send first character
					Debug.Log("Logging in as ;" + charname + ";");
					reset();
					add((byte)83);	//use char packet
					add(charname);
					add((int)0);
					add((int)0);
					send_packet();
				}
				break;
			default:
				//Debug.Log("Unknown packet (" + rpckts[0] + ") " + ByteArrayToString(rpckts, rpacket_length));
				break;
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
	}

}
