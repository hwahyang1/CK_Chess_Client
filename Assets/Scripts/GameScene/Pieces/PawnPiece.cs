using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

namespace Chess_Client.GameScene.Pieces
{
	public class PawnPiece : PieceBased
	{
	    public override List<int> GetAvailableMoves(Transform[] boardSquares, PieceBased[] piecePositions)
	    {
	        List<int> moves = new List<int>();
	        int direction = (team == DefineTeam.White) ? -1 : 1;
	        int row = currentIndex / 8;
	        int col = currentIndex % 8;

	        int forwardIndex = currentIndex + direction * 8;
	        if (IsValid(forwardIndex) && piecePositions[forwardIndex] == null)
	        {
	            moves.Add(forwardIndex);

	            int startRow = (team == DefineTeam.White) ? 6 : 1;
	            if (row == startRow)
	            {
	                int doubleForward = currentIndex + direction * 16;
	                if (IsValid(doubleForward) && piecePositions[doubleForward] == null)
	                    moves.Add(doubleForward);
	            }
	        }

	        int[] diag = { -1, 1 };
	        foreach (int d in diag)
	        {
	            int newCol = col + d;
	            if (newCol >= 0 && newCol < 8)
	            {
	                int diagIndex = currentIndex + direction * 8 + d;
	                if (IsValid(diagIndex) && piecePositions[diagIndex] != null && piecePositions[diagIndex].Team != team)
	                    moves.Add(diagIndex);
	            }
	        }

	        return moves;
	    }

	    private bool IsValid(int index) => index >= 0 && index < 64;
	}
}
