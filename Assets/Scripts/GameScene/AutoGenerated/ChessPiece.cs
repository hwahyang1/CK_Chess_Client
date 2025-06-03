using System.Collections.Generic;
using UnityEngine;

public enum Team { White, Black }

public abstract class ChessPiece : MonoBehaviour
{
    public Team team;
    public int currentIndex;

    public abstract List<int> GetLegalMoves(Transform[] boardSquares, ChessPiece[] piecePositions);
}