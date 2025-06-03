using System.Collections.Generic;
using UnityEngine;

public class BishopPiece : ChessPiece
{
    public override List<int> GetLegalMoves(Transform[] boardSquares, ChessPiece[] piecePositions)
    {
        return GetDiagonalMoves(new int[] { -9, -7, 7, 9 }, piecePositions);
    }

    private List<int> GetDiagonalMoves(int[] directions, ChessPiece[] pieces)
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
        int fromCol = from % 8;
        int toCol = to % 8;
        return Mathf.Abs(fromCol - toCol) == Mathf.Abs((to / 8) - (from / 8));
    }
}