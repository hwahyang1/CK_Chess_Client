using System;

using Newtonsoft.Json;

using Chess_Client.Templates.Internal;

namespace Chess_Client.Templates.Response
{
	[Serializable]
	public class RoomListsResponse : BaseResponse
	{
		public RoomData[] rooms;

		[JsonConstructor]
		public RoomListsResponse(string clientUid, string command, int statusCode, string reason, RoomData[] rooms) : base(clientUid, command, statusCode, reason)
		{
			this.rooms = rooms;
		}
	}
}
