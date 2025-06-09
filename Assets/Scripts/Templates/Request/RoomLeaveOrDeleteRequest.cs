using System;

using Newtonsoft.Json;

namespace Chess_Client.Templates.Request
{
	[Serializable]
	public class RoomLeaveOrDeleteRequest : BaseRequest
	{
		public string roomId;

		[JsonConstructor]
		public RoomLeaveOrDeleteRequest(string clientUid, string command, string roomId) : base(clientUid, command)
		{
			this.roomId = roomId;
		}
	}
}
