using UnityEngine;

static class OpCodes
{
	//OPCODES temporary file until config version made.
	//All temporary opcodes are for the tikal_antharas client


	//OUTGOING AKA Client Packets		CLIENT => Server
	public const int C_AUTH_LOGIN		=	12;
	public const int C_VERSION			=	71;
	public const int C_CHAT_NORMAL		=	104;
	public const int C_CHAT_WHISPER		=	13;
	public const int C_CHAT_GLOBAL		=	40;
	public const int C_MENUCLICK		= 	43;


	//Incoming AKA Server Packets		SERVER => Client
	public const int S_VERSION			=	10;
	public const int S_SERVER_MESSAGE	=	87;
	public const int S_CHAR_AMOUNT		=	113;
	public const int S_CHAR_PACKS		=	64;
	public const int S_CHAT_NORMAL		=	8;
	public const int S_CHAT_GLOBAL		=	105;
	public const int S_CHAT_WHISPER		=	91;
	public const int S_CHAT_SHOUT		=	42;
	public const int S_DISCONNECT		=	18;
	public const int S_LOGINRESULT		=	21;
	public const int S_NEWS				=	90;


}