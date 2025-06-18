using System;

namespace Chess_Client.Templates.Internal
{
	public enum DefineGameStatus
	{
		Unknown = -1,
		Prepare,
		Running,
		Check,
		Checkmate,
		End
	}
}
