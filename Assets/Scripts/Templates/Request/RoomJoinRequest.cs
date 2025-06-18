using System;

using Newtonsoft.Json;

namespace Chess_Client.Templates.Request
{
	[Serializable]
	public class RoomJoinRequest : BaseRequest
	{
		public string roomId;

		[JsonConstructor]
		public RoomJoinRequest(string clientUid, string command, string roomId) : base(clientUid, command)
		{
			this.roomId = roomId;
		}
	}
}
