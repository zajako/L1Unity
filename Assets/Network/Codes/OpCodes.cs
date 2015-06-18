using UnityEngine;

public class OpCodes : MonoBehaviour
{
	//OPCODES temporary file until config version made.
	//All temporary opcodes are for the tikal_antharas client


	//OUTGOING AKA Client Packets		CLIENT => Server
	public static int C_AUTH_LOGIN		=	113;
	public static int C_VERSION			=	34;
	public static int C_CHAT_NORMAL		=	18;
	public static int C_CHAT_WHISPER	=	92;
	public static int C_CHAT_GLOBAL		=	115;


	//Incoming AKA Server Packets		SERVER => Client
	public static int S_VERSION			=	89;
	public static int S_SERVER_MESSAGE	=	98;
	public static int S_CHAR_AMOUNT		=	80;
	public static int S_CHAR_PACKS		=	102;
	public static int S_CHAT_NORMAL		=	71;
	public static int S_CHAT_GLOBAL		=	3;
	public static int S_CHAT_WHISPER	=	47;
	public static int S_DISCONNECT		=	41;
	public static int S_LOGINRESULT		=	63;


}