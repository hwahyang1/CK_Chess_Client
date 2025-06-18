using System;

using Newtonsoft.Json;

using Chess_Client.Templates.Internal;

namespace Chess_Client.Templates.Response
{
	[Serializable]
	public class RoomLeaveOrDeleteResponse : BaseResponse
	{
		public RoomData? room;

		[JsonConstructor]
		public RoomLeaveOrDeleteResponse(string clientUid, string command, int statusCode, string reason, RoomData? room) : base(clientUid, command, statusCode, reason)
		{
			this.room = room;
		}
	}
}
