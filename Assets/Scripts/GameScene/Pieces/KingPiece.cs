using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

namespace Chess_Client.GameScene.Pieces
{
	public class KingPiece : PieceBased
	{
	    public override List<int> GetAvailableMoves(Transform[] boardSquares, PieceBased[] piecePositions)
	    {
	        List<int> moves = new List<int>();
	        int[] directions = { -9, -8, -7, -1, 1, 7, 8, 9 };

	        foreach (int dir in directions)
	        {
	            int target = currentIndex + dir;
	            if (target < 0 || target >= 64) continue;
	            int rowDiff = Mathf.Abs((target / 8) - (currentIndex / 8));
	            int colDiff = Mathf.Abs((target % 8) - (currentIndex % 8));
	            if (rowDiff <= 1 && colDiff <= 1)
	            {
	                if (piecePositions[target] == null || piecePositions[target].Team != team)
	                    moves.Add(target);
	            }
	        }

	        return moves;
	    }
	}
}
