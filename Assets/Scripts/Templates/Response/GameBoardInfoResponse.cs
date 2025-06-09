using System;

using Newtonsoft.Json;

using Chess_Client.Templates.Internal;

namespace Chess_Client.Templates.Response
{
	[Serializable]
	public class GameBoardInfoResponse : BaseResponse
	{
		public Block[] board;

		[JsonConstructor]
		public GameBoardInfoResponse(string clientUid, string command, int statusCode, string reason, Block[] board) : base(clientUid, command, statusCode, reason)
		{
			this.board = board;
		}
	}
}
