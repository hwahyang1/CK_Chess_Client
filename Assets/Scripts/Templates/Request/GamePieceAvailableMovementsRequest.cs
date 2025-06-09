using System;

using Newtonsoft.Json;

namespace Chess_Client.Templates.Request
{
	[Serializable]
	public class GamePieceAvailableMovementsRequest : BaseRequest
	{
		public string roomId;
		public int position;

		[JsonConstructor]
		public GamePieceAvailableMovementsRequest(string clientUid, string command, string roomId, int position) : base(clientUid, command)
		{
			this.roomId = roomId;
			this.position = position;
		}
	}
}
