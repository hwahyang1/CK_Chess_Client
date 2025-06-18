using System;

using Newtonsoft.Json;

namespace Chess_Client.Templates.Request
{
	[Serializable]
	public class RoomCreateRequest : BaseRequest
	{
		public string roomName;

		[JsonConstructor]
		public RoomCreateRequest(string clientUid, string command, string roomName) : base(clientUid, command)
		{
			this.roomName = roomName;
		}
	}
}
