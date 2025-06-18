using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

using Chess_Client.Templates.Internal;

namespace Chess_Client.GameScene.Pieces
{
	public class PawnPiece : PieceBased
	{
		public override List<int> GetAvailableMoves(Transform[] boardSquares, PieceBased[] piecePositions)
		{
			List<int> moves = new List<int>();
			int direction = (team == DefineTeam.White) ? 8 : -8;
			int startRow = (team == DefineTeam.White) ? 1 : 6;
			int row = currentIndex / 8;
			int col = currentIndex % 8;

			int forwardOne = currentIndex + direction;
			if (IsValid(forwardOne) && piecePositions[forwardOne] == null)
			{
				moves.Add(forwardOne);

				int forwardTwo = currentIndex + direction * 2;
				if (row == startRow && piecePositions[forwardTwo] == null)
				{
					moves.Add(forwardTwo);
				}
			}

			// diagonals
			int[] diagonals = { direction - 1, direction + 1 };
			foreach (int offset in diagonals)
			{
				int target = currentIndex + offset;
				if (IsValid(target))
				{
					int targetCol = target % 8;
					if (Mathf.Abs(targetCol - col) == 1 &&
					    piecePositions[target] != null &&
					    piecePositions[target].Team != team)
					{
						moves.Add(target);
					}
				}
			}

			return moves;
		}

	    private bool IsValid(int index) => index >= 0 && index < 64;
	}
}
