using UnityEngine;
using System.Collections;
using System.Text;

public class ServerPacketBase
{
	protected System.Byte[] _bytes;
	protected int _length;
	protected int _off;
	protected int _opcode;
	StringBuilder _packetString;


	public ServerPacketBase(byte[] data, int length)
	{
		_bytes = data;
		_length = length;
		_opcode = readByte();
	}

	
	protected int readD()
	{
		int i =	_bytes[_off] |
				_bytes[_off+1]<<8 |
				_bytes[_off+2]<<16 |
				_bytes[_off+3]<<24;
		_off += 4;
		return i;
	}

	protected int readC()
	{
		int i = _bytes[_off++] & 0xff;
		return i;
	}

	protected int readH()
	{
		int i = _bytes[_off++] & 0xff;
		i |= _bytes[_off++] << 8 & 0xff00;
		return i;
	}

	protected int readCH()
	{
		int i = _bytes[_off++] & 0xff;
		i |= _bytes[_off++] << 8 & 0xff00;
		i |= _bytes[_off++] << 16 & 0xff0000;
		return i;
	}

	protected double readF()
	{
		// long l = _bytes[_off++] & 0xff;
		// l |= _bytes[_off++] << 8 & 0xff00;
		// l |= _bytes[_off++] << 16 & 0xff0000;
		// l |= _bytes[_off++] << 24 & 0xff000000;
		// l |= (long) _bytes[_off++] << 32 & 0xff00000000L;
		// l |= (long) _bytes[_off++] << 40 & 0xff0000000000L;
		// l |= (long) _bytes[_off++] << 48 & 0xff000000000000L;
		// l |= (long) _bytes[_off++] << 56 & 0xff00000000000000L;
		// return Double.LongBitsToDouble(l);

		return (double) 0;
	}

	protected string readS()
	{
		int i;
		for(i = 0; _bytes[i+_off] != 0; i++);
		_off += i+1;
		return Encoding.UTF8.GetString(_bytes, _off-i-1, i);
	}

	protected byte readByte()
	{
		return _bytes[_off++];
	}

	protected ushort readUShort()
	{
		ushort ret = 0;
		ret = (ushort)(_bytes[_off] |
				_bytes[_off+1]<<8);
		_off += 2;
		return ret;
	}
	
	protected uint readUInt()
	{
		uint ret = 0;
		ret = (uint)(_bytes[_off] |
				_bytes[_off+1]<<8 |
				_bytes[_off+2]<<16 |
				_bytes[_off+3]<<24);
		_off += 4;
		return ret;
	}	
}
