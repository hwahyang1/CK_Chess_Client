using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

using Chess_Client.GameScene.Pieces;

namespace Chess_Client.GameScene
{
	/// <summary>
	/// Description
	/// </summary>
	public class GameManager : Singleton<GameManager>
	{
		[Header("GameManager - Config")]
		[SerializeField]
		private Transform[] boardMarkers;

		[SerializeField]
		private Transform pieceParent;
		
		[SerializeField]
		private GameObject[] whitePrefabs;
		
		[SerializeField]
		private GameObject[] blackPrefabs;
		
		[Header("GameManager - Status")]
		[SerializeField, ReadOnly]
		private PieceBased[] piecePositions = new PieceBased[64];
		
		private void Start()
		{
			SpawnAllPieces();
		}

		private void SpawnAllPieces()
		{
			int[] whiteLayout = { 0, 1, 2, 3, 4, 2, 1, 0 };
			int[] blackLayout = { 0, 1, 2, 3, 4, 2, 1, 0 };

			for (int i = 0; i < 8; i++)
			{
				CreatePiece(whitePrefabs[whiteLayout[i]], i, DefineTeam.White);
				CreatePiece(whitePrefabs[5], i + 8, DefineTeam.White);
				CreatePiece(blackPrefabs[5], i + 48, DefineTeam.Black);
				CreatePiece(blackPrefabs[blackLayout[i]], i + 56, DefineTeam.Black);
			}
		}

		private void CreatePiece(GameObject prefab, int index, DefineTeam team)
		{
			GameObject pieceObject = Instantiate(prefab, boardMarkers[index].position, Quaternion.identity, pieceParent);
			PieceBased piece = pieceObject.GetComponent<PieceBased>();
			piece.Set(team, index);
			piecePositions[index] = piece;
		}
	}
}
