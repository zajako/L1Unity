using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class ClientPacketBase : MonoBehaviour {

	protected NetCon netcon;
	MemoryStream _byteStream;
	StringBuilder _packetString;

	protected void send_packet()
	{
		netcon.send_packet(_byteStream.ToArray());
	}
	
	public virtual void Awake()
	{
		_byteStream = new MemoryStream();
		_packetString = new StringBuilder();
		netcon = GameObject.Find("netcon").GetComponent<NetCon>();
		_packetString.Append("Sending Packet: "+getType()+" ");
		writeH(0);	//packet size
	}

	protected void writeD(int value)
	{
		addToStream((byte)(value & 0xFF));
		addToStream((byte)((value >> 8) & 0xFF));
		addToStream((byte)((value >> 16) & 0xFF));
		addToStream((byte)((value >> 24) & 0xFF));

		_packetString.Append("[D: "+value+"]");
	}

	protected void writeH(int value)
	{
		addToStream((byte)(value & 0xFF));
		addToStream((byte)((value >> 8) & 0xFF));

		_packetString.Append("[H: "+value+"]");
	}

	protected void writeC(int value)
	{
		addToStream((byte)(value & 0xFF));

		_packetString.Append("[C: "+value+"]");
	}

	protected void writeP(int value)
	{
		addToStream((byte)(value));

		_packetString.Append("[P: "+value+"]");
	}

	protected void writeL(long value)
	{
		addToStream((byte)(value & 0xFF));

		_packetString.Append("[L: "+value+"]");
	}

	protected void writeF(double value)
	{
		// addToStream((byte)(value & 0xFF));
		// addToStream((byte)((value >> 8) & 0xFF));
		// addToStream((byte)((value >> 16) & 0xFF));
		// addToStream((byte)((value >> 24) & 0xFF));
		// addToStream((byte)((value >> 32) & 0xFF));
		// addToStream((byte)((value >> 40) & 0xFF));
		// addToStream((byte)((value >> 48) & 0xFF));
		// addToStream((byte)((value >> 56) & 0xFF));

		_packetString.Append("[L: "+value+"]");
	}

	protected void writeS(string value)
	{
		if(value != null)
		{
			addToStream(System.Text.Encoding.UTF8.GetBytes(value));
		}
		addToStream(0);

		_packetString.Append("[S: "+value+"]");
	}

	protected void writeBytes(byte[] value)
	{
		if(value != null)
		{
			addToStream(value);
		}
		
		_packetString.Append("[Byte: "+value+"]");
	}

	public int getLength()
	{
		return (int)(_byteStream.Length + 2);
	}

	public byte[] getBytes()
	{
		int padding = (int)_byteStream.Length % 4;
		
		if(padding != 0)
		{
			for(int i = padding; i < 4; i++)
			{
				writeC(0x00);
			}
		}
		
		return _byteStream.ToArray();
	}


	public string getType()
	{
		return "[C] " + ((object) this).GetType().Name;
	}

	public void printPacket()
	{
		Debug.Log(_packetString.ToString());
	}

	private void addToStream(byte b)
	{
		Debug.Log("adding " + b + " to stream");
		_byteStream.WriteByte(b);
	}

	private void addToStream(byte[] b)
	{
		for(int i = 0; i < b.Length; i++)
		{
			_byteStream.WriteByte(b[i]);
		}
	}
	
	public static byte C_CHAT_WHISPER = 13;
	public static byte C_CHAT_GLOBAL = 40;
	public static byte C_CHAT_NORMAL = 104;
}
