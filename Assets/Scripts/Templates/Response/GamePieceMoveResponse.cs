using System;

using Newtonsoft.Json;

using Chess_Client.Templates.Internal;

namespace Chess_Client.Templates.Response
{
	[Serializable]
	public class GamePieceMoveResponse : BaseResponse
	{
		public DefineGameStatus gameStatus;
		public DefineTeam dominantTeam;
		public Block[] board;

		[JsonConstructor]
		public GamePieceMoveResponse(string clientUid, string command, int statusCode, string reason, DefineGameStatus gameStatus, DefineTeam dominantTeam, Block[] board) : base(clientUid, command, statusCode, reason)
		{
			this.gameStatus = gameStatus;
			this.dominantTeam = dominantTeam;
			this.board = board;
		}
	}
}
