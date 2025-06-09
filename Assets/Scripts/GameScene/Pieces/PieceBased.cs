using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

using Chess_Client.Templates.Internal;

namespace Chess_Client.GameScene.Pieces
{
	/// <summary>
	/// Description
	/// </summary>
	public abstract class PieceBased : MonoBehaviour
	{
		[Header("PieceBased - Config")]
		
		[Header("PieceBased - Status")]
		[SerializeField, ReadOnly]
		protected DefineTeam team;
		public DefineTeam Team => team;
		
		[SerializeField, ReadOnly]
		protected int currentIndex;
		public int CurrentIndex => currentIndex;

		public virtual void Set(DefineTeam team = DefineTeam.None, int index = -1)
		{
			if (team != DefineTeam.None) this.team = team;
			currentIndex = index;
		}

		public abstract List<int> GetAvailableMoves(Transform[] boardSquares, PieceBased[] piecePositions);
	}
}
