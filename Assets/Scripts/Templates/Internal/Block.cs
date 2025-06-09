using System;

using Newtonsoft.Json;

namespace Chess_Client.Templates.Internal
{
	[Serializable]
	public struct Block
	{
		public DefinePieces piece;
		public DefineTeam team;

		[JsonConstructor]
		public Block(DefinePieces piece = DefinePieces.None, DefineTeam team = DefineTeam.None)
		{
			this.piece = piece;
			this.team = team;
		}
		
		public bool IsEmpty => piece == DefinePieces.None;
	}
}
