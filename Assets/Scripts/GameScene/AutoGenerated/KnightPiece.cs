using System.Collections.Generic;
using UnityEngine;

public class KnightPiece : ChessPiece
{
    public override List<int> GetLegalMoves(Transform[] boardSquares, ChessPiece[] piecePositions)
    {
        List<int> moves = new List<int>();
        int[] offsets = { -17, -15, -10, -6, 6, 10, 15, 17 };

        foreach (int offset in offsets)
        {
            int target = currentIndex + offset;
            if (target < 0 || target >= 64) continue;

            int rowDiff = Mathf.Abs((target / 8) - (currentIndex / 8));
            int colDiff = Mathf.Abs((target % 8) - (currentIndex % 8));
            if ((rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2))
            {
                if (piecePositions[target] == null || piecePositions[target].team != team)
                    moves.Add(target);
            }
        }

        return moves;
    }
}