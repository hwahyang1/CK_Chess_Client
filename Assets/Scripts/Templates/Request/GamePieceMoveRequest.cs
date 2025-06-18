using System;

using Newtonsoft.Json;

namespace Chess_Client.Templates.Request
{
	[Serializable]
	public class GamePieceMoveRequest : BaseRequest
	{
		public string roomId;
		public int fromPosition;
		public int toPosition;

		[JsonConstructor]
		public GamePieceMoveRequest(string clientUid, string command, string roomId, int fromPosition, int toPosition) : base(clientUid, command)
		{
			this.roomId = roomId;
			this.fromPosition = fromPosition;
			this.toPosition = toPosition;
		}
	}
}
