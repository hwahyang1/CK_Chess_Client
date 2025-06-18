using System;

using Newtonsoft.Json;

namespace Chess_Client.Templates.Request
{
	[Serializable]
	public class RoomListsRequest : BaseRequest
	{
		// TODO: Filter & Sorting

		[JsonConstructor]
		public RoomListsRequest(string clientUid, string command) : base(clientUid, command)
		{
			//
		}
	}
}
