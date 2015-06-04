using UnityEngine;
using System.Collections;
using System.Text;

public class ServerPacketBase : MonoBehaviour {

	protected System.Byte[] rpckts;
	protected int rpacket_length;
	protected int rpckt_offset;
	
	protected byte get_byte()
	{
		return rpckts[rpckt_offset++];
	}
	
	protected short get_short()
	{
		short ret = 0;
		ret = (short)(rpckts[rpckt_offset] |
				rpckts[rpckt_offset+1]<<8);
		rpckt_offset += 2;
		return ret;
	}
	
	protected ushort get_ushort()
	{
		ushort ret = 0;
		ret = (ushort)(rpckts[rpckt_offset] |
				rpckts[rpckt_offset+1]<<8);
		rpckt_offset += 2;
		return ret;
	}
	
	protected int get_int()
	{
		int ret = 0;
		ret = rpckts[rpckt_offset] |
				rpckts[rpckt_offset+1]<<8 |
				rpckts[rpckt_offset+2]<<16 |
				rpckts[rpckt_offset+3]<<24;
		rpckt_offset += 4;
		return ret;
	}
	
	protected uint get_uint()
	{
		uint ret = 0;
		ret = (uint)(rpckts[rpckt_offset] |
				rpckts[rpckt_offset+1]<<8 |
				rpckts[rpckt_offset+2]<<16 |
				rpckts[rpckt_offset+3]<<24);
		rpckt_offset += 4;
		return ret;
	}
	
	protected string get_string()
	{
		int i;
		for (i = 0; rpckts[i+rpckt_offset] != 0; i++);
		rpckt_offset += i+1;
		return Encoding.UTF8.GetString(rpckts, rpckt_offset-i-1, i);
	}
	
	public static byte S_CHAT_NORMAL = 8;
	public static byte S_CHAT_GLOBAL = 105;
	public static byte S_CHAT_SHOUT = 42;
	public static byte S_CHAT_WHISPER = 91;
	
}
