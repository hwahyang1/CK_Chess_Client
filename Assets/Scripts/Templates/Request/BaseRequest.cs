using System;

using Newtonsoft.Json;

namespace Chess_Client.Templates.Request
{
	[Serializable]
	public class BaseRequest
	{
		public string clientUid;
		public string command;

		[JsonConstructor]
		public BaseRequest(string clientUid, string command)
		{
			this.clientUid = clientUid;
			this.command = command;
		}
	}
}
