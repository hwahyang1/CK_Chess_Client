using System;

using Newtonsoft.Json;

namespace Chess_Client.Templates.Response
{
	[Serializable]
	public class BaseResponse
	{
		public string clientUid;
		public string command;
		public int statusCode;
		public string reason;

		[JsonConstructor]
		public BaseResponse(string clientUid, string command, int statusCode, string reason)
		{
			this.clientUid = clientUid;
			this.command = command;
			this.statusCode = statusCode;
			this.reason = reason;
		}
	}
}
