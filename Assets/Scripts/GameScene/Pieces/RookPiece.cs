using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

namespace Chess_Client.GameScene.Pieces
{
	public class RookPiece : PieceBased
	{
	    public override List<int> GetAvailableMoves(Transform[] boardSquares, PieceBased[] piecePositions)
	    {
	        return GetLinearMoves(new int[] { -8, 8, -1, 1 }, piecePositions);
	    }

	    private List<int> GetLinearMoves(int[] directions, PieceBased[] pieces)
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
	        if (dir == -1 && to % 8 == 7) return false;
	        if (dir == 1 && to % 8 == 0) return false;
	        return true;
	    }
	}
}
