using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

namespace Chess_Client.GameScene.Pieces
{
	public class QueenPiece : PieceBased
	{
		public override List<int> GetAvailableMoves(Transform[] boardSquares, PieceBased[] piecePositions)
		{
			List<int> moves = new List<int>();

			int[] directions = { -9, -8, -7, -1, 1, 7, 8, 9 }; // 대각선 + 수직/수평

			foreach (int dir in directions)
			{
				int index = currentIndex;
				while (true)
				{
					int next = index + dir;
					if (!IsValidMove(index, next, dir)) break;

					if (piecePositions[next] != null)
					{
						if (piecePositions[next].Team != team)
							moves.Add(next);
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

			int fromRow = from / 8;
			int fromCol = from % 8;
			int toRow = to / 8;
			int toCol = to % 8;

			// 수평 이동 시, 줄이 바뀌면 안 됨
			if ((dir == -1 || dir == 1) && fromRow != toRow) return false;

			// 대각선 이동 시, 열과 행의 차이 절댓값이 같아야 함
			if ((dir == -9 || dir == -7 || dir == 7 || dir == 9) && Mathf.Abs(toRow - fromRow) != Mathf.Abs(toCol - fromCol))
				return false;

			return true;
		}
	}
}
