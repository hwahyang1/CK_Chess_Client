using System.Collections.Generic;
using UnityEngine;

public class QueenPiece : ChessPiece
{
    public override List<int> GetLegalMoves(Transform[] boardSquares, ChessPiece[] piecePositions)
    {
        List<int> moves = new List<int>();
        moves.AddRange(new RookPiece { team = team, currentIndex = currentIndex }.GetLegalMoves(boardSquares, piecePositions));
        moves.AddRange(new BishopPiece { team = team, currentIndex = currentIndex }.GetLegalMoves(boardSquares, piecePositions));
        return moves;
    }
}