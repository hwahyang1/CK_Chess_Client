using System;

namespace Chess_Client.Templates.Response
{
	[Serializable]
	public class ErrorResponse : BaseResponse
	{
		//

		public ErrorResponse(string clientUid, int statusCode, string reason) : base(clientUid, "ERROR", statusCode, reason)
		{
			//
		}
	}
}
