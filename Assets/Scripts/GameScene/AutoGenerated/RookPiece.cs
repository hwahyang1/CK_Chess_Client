using System.Collections.Generic;
using UnityEngine;

public class RookPiece : ChessPiece
{
    public override List<int> GetLegalMoves(Transform[] boardSquares, ChessPiece[] piecePositions)
    {
        return GetLinearMoves(new int[] { -8, 8, -1, 1 }, piecePositions);
    }

    private List<int> GetLinearMoves(int[] directions, ChessPiece[] pieces)
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
                    if (pieces[next].team != team) moves.Add(next);
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