using System;

using Newtonsoft.Json;

using Chess_Client.Templates.Internal;

namespace Chess_Client.Templates.Response
{
	[Serializable]
	public class RoomInfoResponse : BaseResponse
	{
		public RoomData room;
		public DefineTeam yourTeam;

		[JsonConstructor]
		public RoomInfoResponse(string clientUid, string command, int statusCode, string reason, RoomData room, DefineTeam yourTeam) : base(clientUid, command, statusCode, reason)
		{
			this.room = room;
			this.yourTeam = yourTeam;
		}
	}
}
