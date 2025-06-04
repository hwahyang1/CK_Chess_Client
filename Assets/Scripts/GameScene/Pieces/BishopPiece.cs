using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

namespace Chess_Client.GameScene.Pieces
{
	public class BishopPiece : PieceBased
	{
		public override List<int> GetAvailableMoves(Transform[] boardSquares, PieceBased[] piecePositions)
		{
			return GetDiagonalMoves(new int[] { -9, -7, 7, 9 }, piecePositions);
		}

		private List<int> GetDiagonalMoves(int[] directions, PieceBased[] pieces)
		{
			List<int> moves = new List<int>();
			foreach (int dir in directions)
			{
				int index = currentIndex;
				while (true)
				{
					int next = index + dir;
					if (!IsValidMove(index, next, dir)) break;
					if (pieces[next] != null)
					{
						if (pieces[next].Team != team) moves.Add(next);
						break;
					}
					moves.Add(next);
					index = next;
				}
			}
			return moves;
		}

		private bool IsValidMove(int from, int to, int dir)
		{
			if (to < 0 || to >= 64) return false;
			int fromCol = from % 8;
			int toCol = to % 8;
			return Mathf.Abs(fromCol - toCol) == Mathf.Abs((to / 8) - (from / 8));
		}
	}
}
