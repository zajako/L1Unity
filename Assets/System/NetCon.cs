using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.IO;


public class NetCon : MonoBehaviour {
	public Rigidbody player;
	public Text stuff;
	public InputField inp;
	public Scrollbar scrl;
	NetworkStream stream;
	string id;
	System.Byte[] pckts;
	int pckt_offset;
	int packet_length;
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
		pckt_offset = 0;
		pckts = new System.Byte[65536];
		has_key = false;
		rcv_key = new System.Byte[8];
		snd_key = new System.Byte[8];
		chat = 0;
		stuff.text = "Chat started: " + chat;
	}
	
	public static string ByteArrayToString(byte[] ba, int packet_length)
	{
	  StringBuilder hex = new StringBuilder(ba.Length * 2);
	  for (int i = 0; i < packet_length; i++)
	    hex.AppendFormat("{0:x2} ", ba[i]);
	  return hex.ToString();
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
		if (packet_length != 0)
		{
			System.Byte b3 = pckts[3];
			pckts[3] ^= rcv_key[2];
	
			System.Byte b2 = pckts[2];
			pckts[2] ^= (System.Byte)(b3 ^ rcv_key[3]);
	
			System.Byte b1 = pckts[1];
			pckts[1] ^= (System.Byte)(b2 ^ rcv_key[4]);
	
			System.Byte k = (System.Byte)(pckts[0] ^ b1 ^ rcv_key[5]);
			pckts[0] = (System.Byte)(k ^ rcv_key[0]);
	
			for (int i = 1; i < packet_length; i++) 
			{
				System.Byte t = pckts[i];
				pckts[i] ^= (System.Byte)(rcv_key[i & 7] ^ k);
				k = t;
			}
		}
	}
	
	void encrypt()
	{
		//Debug.Log("Encryption key " + ByteArrayToString(snd_key, 8));
		if (packet_length != 0)
		{
			pckts[2] ^= snd_key[0];
			
			for (int i = 3; i < packet_length; i++)
			{
				pckts[i] ^= (byte)(snd_key[(i-2) & 7] ^ pckts[i-1]);
			}	
			pckts[5] ^= (byte)(snd_key[2]);
			pckts[4] ^= (byte)(snd_key[3] ^ pckts[5]);
			pckts[3] ^= (byte)(snd_key[4] ^ pckts[4]);
			pckts[2] ^= (byte)(snd_key[5] ^ pckts[3]);
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
		data[0] = pckts[2];
		data[1] = pckts[3];
		data[2] = pckts[4];
		data[3] = pckts[5];
		
		//Debug.Log("Sending packet " + ByteArrayToString(pckts, packet_length));
		encrypt();
		change_snd_key(data);
		//Debug.Log("Sending Epacket " + ByteArrayToString(pckts, packet_length));
		stream.Write(pckts, 0, packet_length);
	}
	
	void login_packet()
	{
		Debug.Log("Sending login data");
		packet_length = 17;
		pckts[0] = (byte)17;
		pckts[1] = (byte)0;
		pckts[2] = (byte)12;
		pckts[3] = (byte)'s';
		pckts[4] = (byte)'t';
		pckts[5] = (byte)'u';
		pckts[6] = (byte)'p';
		pckts[7] = (byte)'i';
		pckts[8] = (byte)'d';
		pckts[9] = (byte)0;
		pckts[10] = (byte)'s';
		pckts[11] = (byte)'t';
		pckts[12] = (byte)'u';
		pckts[13] = (byte)'p';
		pckts[14] = (byte)'i';
		pckts[15] = (byte)'d';
		pckts[16] = (byte)0;
		send_packet();
	}
	
	void login_check()
	{
		switch (pckts[1])
		{
			case 3:
				packet_length = 11;
				pckts[0] = 11;
				pckts[1] = 0;
				pckts[2] = 57;	//alive packet
				pckts[3] = 0;
				pckts[4] = 0;
				pckts[5] = 0;
				pckts[6] = 0;
				pckts[7] = 0;
				pckts[8] = 0;
				pckts[9] = 0;
				pckts[10] = 0;
				send_packet();
				packet_length = 12;
				pckts[0] = 12;
				pckts[1] = 0;
				pckts[2] = 92;	//init game
				pckts[3] = 0;
				pckts[4] = 0;
				pckts[5] = 0;
				pckts[6] = 0;
				pckts[7] = 0;
				pckts[8] = 0;
				pckts[9] = 0;
				pckts[10] = 0;
				pckts[11] = 0;
				send_packet();
				break;
			default:
				break;
		}
	}
	
	public void chat_submit()
	{
		Debug.Log("Submitting chat");
	}
	
	void process_packet_contents()
	{
		switch(pckts[0])
		{
			case 18:	//disconnected by server
				Debug.Log("Disconnected");
				break;
			case 8: 
			case 42: 
			case 91: 
			case 105: //chat packets
				chat++;
				stuff.text += "Chat: " + chat;
				scrl.value = 1;
				break;			
			case 10:	//server version
				login_packet();
				break;
			case 21:	//login
				login_check();
				break;
			case 65:	//key packet
				uint seed = ((uint)pckts[1]) |
							((uint)pckts[2])<<8 |
							((uint)pckts[3])<<16 |
							((uint)pckts[4])<<24;
				init_key(seed);
				
				pckts[0] = 14;
				pckts[1] = 0;
				pckts[2] = 71;
				pckts[3] = 0x33;
				pckts[4] = 0;
				pckts[5] = 0xFF;
				pckts[6] = 0xFF;
				pckts[7] = 0xFF;
				pckts[8] = 0xFF;
				pckts[9] = 32;
				pckts[10] = (101101)&0xFF;
				pckts[11] = (101101)>>8&0xFF;
				pckts[12] = (101101)>>16&0xFF;
				pckts[13] = (101101)>>24&0xFF;
				packet_length = 14;
				//Debug.Log("Received encryption seed");
				send_packet();
				break;
			case 90:	//news packet
				packet_length = 11;
				pckts[0] = 11;
				pckts[1] = 0;
				pckts[2] = 43;	//client click packet
				pckts[3] = 0;
				pckts[4] = 0;
				pckts[5] = 0;
				pckts[6] = 0;
				pckts[7] = 0;
				pckts[8] = 0;
				pckts[9] = 0;
				pckts[10] = 0;
				send_packet();
				break;
			case 113:	//num char packet
				num_chars = pckts[1];
				chars_rcvd = 0;
				Debug.Log(num_chars + " chars total");
				break;
			case 99:	//login char packet
				int name_len = 0;
				chars_rcvd++;
				Debug.Log("Char info packet (" + pckts[0] + ") " + ByteArrayToString(pckts, packet_length));
				if (chars_rcvd == 1)
				{
					name_len = 0;
					for (; pckts[name_len+1] != 0; name_len++);
					Debug.Log("Char name length " + name_len);
					charname = Encoding.UTF8.GetString(pckts, 1, name_len);
				}
				if (chars_rcvd == num_chars)
				{
					//send first character
					Debug.Log("Logging in as ;" + charname + ";");
					pckts[0] = (byte)(10+charname.Length);
					pckts[1] = 0;
					pckts[2] = 83;	//use char packet
					for (int i = 0; i < charname.Length; i++)
						pckts[3+i] = (byte)charname[i];
					for (int i = 0; i < 8; i++)
						pckts[3+charname.Length+i] = 0;
					packet_length = 10+charname.Length;
					send_packet();
				}
				break;
			default:
				Debug.Log("Unknown packet (" + pckts[0] + ") " + ByteArrayToString(pckts, packet_length));
				break;
		}
		stream.BeginRead(pckts, 0, 2, new AsyncCallback(read_packet_contents), stream);
	}
	
	void decrypt_packet_contents(IAsyncResult rslt)
	{
		int rcvd = stream.EndRead(rslt);
		pckt_offset += rcvd;
		if (pckt_offset < packet_length)
		{
			stream.BeginRead(pckts, pckt_offset, packet_length-pckt_offset, 
				new AsyncCallback(decrypt_packet_contents), stream);
		}
		else
		{
			//Debug.Log(packet_length);
			//Debug.Log("packet " + ByteArrayToString(pckts, packet_length));
			if (has_key)
			{
				System.Byte[] data = new System.Byte[4];
				decrypt();
				data[0] = pckts[0];
				data[1] = pckts[1];
				data[2] = pckts[2];
				data[3] = pckts[3];
				change_rcv_key(data);
			}
			process_packet_contents();
		}
	}
	
	void read_packet_contents(IAsyncResult rslt)
	{
		if (rslt.IsCompleted)
		{
			int rcvd = stream.EndRead(rslt);
			if (rcvd < 2)
			{
				pckt_offset = rcvd;
				stream.BeginRead(pckts, pckt_offset, 2-rcvd, 
					new AsyncCallback(read_packet_contents), stream);
			}
			else
			{
				pckt_offset = 0;
				packet_length = (pckts[0] | pckts[1]<<8) - 2;
				stream.BeginRead(pckts, 0, packet_length, 
					new AsyncCallback(decrypt_packet_contents), stream);
			}
		}
	}

	public void connect() {
		Debug.Log("Connection");
		TcpClient client = new TcpClient ("207.192.73.73", 2000);
		stream = client.GetStream();
		
		stream.BeginRead(pckts, pckt_offset, 2, new AsyncCallback(read_packet_contents), stream);
	}
	
	public void handleEvent(Vector3 position, Quaternion rotation){
		// print (id);
		// JSONObject json = new JSONObject ();
		// json.AddField ("action", "move");
		// JSONObject pos = new JSONObject();

		// pos.AddField ("X", position.x.ToString());
		// pos.AddField ("Y", position.y.ToString());
		// pos.AddField ("Z", position.z.ToString());
		// json.AddField ("position", pos);
		// JSONObject rot = new JSONObject ();
		// rot.AddField ("X", rotation.x.ToString());
		// rot.AddField ("Y", rotation.y.ToString());
		// rot.AddField ("Z", rotation.z.ToString());
		// rot.AddField ("W", rotation.w.ToString());
		// json.AddField ("rotation", rot);
		// json.AddField ("id", id);
		// send (json.ToString());

	}

	public void sendMove(Vector3 move, bool crouch, bool jump){
		// JSONObject json = new JSONObject ();
		// json.AddField ("action", "moveChar");
		// JSONObject pos = new JSONObject();
		
		// pos.AddField ("X", move.x.ToString());
		// pos.AddField ("Y", move.y.ToString());
		// pos.AddField ("Z", move.z.ToString());
		// json.AddField ("position", pos);
		// json.AddField ("id", id);
		// json.AddField ("crouch", crouch);
		// json.AddField ("jump", jump);
		// send (json.ToString());
	}

	private void send(string json){
		//writer.Write (json);
		//writer.Flush ();
	}

	void Update () {
		readData ();
	}

	void readData(){
	//	if (stream.CanRead) {
	//		try{
	//			byte[] bLen = new Byte[4];
	//			int data = stream.Read(bLen, 0, 4);
	//			if(data > 0){
	//				int len = BitConverter.ToInt32( bLen, 0);
	//				print("len = " + len);
	//				Byte[] buff = new byte[1024];
	//				data = stream.Read (buff, 0, len);
	//				if (data > 0) {
	//					string result = Encoding.ASCII.GetString (buff, 0, data);
	//					stream.Flush();
	//					parseData(result);
	//				}
	//			}
	//		}catch (Exception ex){
	//			print (ex);
	//		}
	//	}
	}

	void parseData(string data){
		// JSONObject json = new JSONObject (data);
		// string action = json.GetField ("action").str;
		// print(action + "parse data" + data);
		// JSONObject pos = json.GetField("position");
		// Single pX = Convert.ToSingle (pos.GetField ("X").str);
		// Single pY = Convert.ToSingle (pos.GetField ("Y").str);
		// Single pZ = Convert.ToSingle (pos.GetField ("Z").str);
		// Vector3 position = new Vector3 (pX, pY, pZ);
		// print ("new vector = x-" + pos.GetField ("X").str + " y-" + pos.GetField ("Y").str);
		// JSONObject rot = json.GetField("rotation");
		// Single rX = Convert.ToSingle (rot.GetField ("X").str);
		// Single rY = Convert.ToSingle (rot.GetField ("Y").str);
		// Single rZ = Convert.ToSingle (rot.GetField ("Z").str);
		// Single rW = Convert.ToSingle (rot.GetField ("W").str);
		// Quaternion rotation = new Quaternion (rX, rY, rZ, rW);
		// switch (action) {
		// 	case "start":
		// 		this.id = json.GetField ("id").str;
		// 		createPlayer ();
		// 		break;
		// 	case "newPlayer":
		// 		createNewClient (json.GetField ("id").str, position, rotation);
		// 		break;
		// 	case "move":
		// 		moveClient (json.GetField ("id").str, position, rotation);
		// 		break;
		// }

	}

	void createPlayer(){
		Instantiate(player, new Vector3(0, 1, 0), new Quaternion());
	}
	
}
