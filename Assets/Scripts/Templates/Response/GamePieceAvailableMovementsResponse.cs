using System;

using Newtonsoft.Json;

namespace Chess_Client.Templates.Response
{
	[Serializable]
	public class GamePieceAvailableMovementsResponse : BaseResponse
	{
		public int[] positions;

		[JsonConstructor]
		public GamePieceAvailableMovementsResponse(string clientUid, string command, int statusCode, string reason, int[] positions) : base(clientUid, command, statusCode, reason)
		{
			this.positions = positions;
		}
	}
}
