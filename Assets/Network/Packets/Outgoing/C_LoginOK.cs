using UnityEngine;
using System.Collections;

public class C_LoginOK : ClientPacketBase {

	public C_LoginOK()
	{
		base.Start();
		Debug.Log("Sending Login OK Packet");

		//Temporary
		writeC(OpCodes.C_LOGINOK);
		writeD(0);
		writeD(0);


		//should write 2 Chars, a type and button

		/*
			Type 255 = whisper
				button 95 || button 127 = show world chat true, can whisper true
				button 91 || button 123 = show world chat true, can whisper false
				button 94 || button 126 = show world chat false, can whisper true
				button 90 || button 122 = show world chat false, can whisper false

			Type 0 = normal chat
				button 0 = show world chat false
				button 1 = show world chat true

			Type 2 = shout
				button 0 = can whisper false
				button 1 = can whisper true

			type 6 = trade
				button 0 = show trade chat false
				button 1 = show trade chat true
		*/
	}
}
