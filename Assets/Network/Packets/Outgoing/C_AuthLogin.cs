using UnityEngine;
using System.Collections;

public class C_AuthLogin : ClientPacketBase
{
	// Send the username and password to the client when the submit button is hit
	// Use by calling a netcon object to send the packet like this:
	// NetCon.send_packet(new C_AuthLogin(login,password));
	public C_AuthLogin(string login, string password)
	{
		base.Start();

		writeC(OpCodes.C_AUTH_LOGIN);
		writeS(login);
		writeS(password);
	}
}
