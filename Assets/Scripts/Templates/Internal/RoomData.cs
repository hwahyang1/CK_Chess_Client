using System;

using Newtonsoft.Json;

namespace Chess_Client.Templates.Internal
{
	[Serializable]
	public class RoomData
	{
		public string id;
		public string displayName;
		public string ownerId;
		public string[] participants;
		public bool isStarted;

		[JsonConstructor]
		public RoomData(string id, string displayName, string ownerId, string[] participants, bool isStarted)
		{
			this.id = id;
			this.displayName = displayName;
			this.ownerId = ownerId;
			this.participants = participants;
			this.isStarted = isStarted;
		}
	}
}
