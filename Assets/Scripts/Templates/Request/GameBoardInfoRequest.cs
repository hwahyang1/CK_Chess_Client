using System;

using Newtonsoft.Json;

namespace Chess_Client.Templates.Request
{
	[Serializable]
	public class GameBoardInfoRequest : BaseRequest
	{
		public string roomId;

		[JsonConstructor]
		public GameBoardInfoRequest(string clientUid, string command, string roomId) : base(clientUid, command)
		{
			this.roomId = roomId;
		}
	}
}
