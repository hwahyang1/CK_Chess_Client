using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
			
		[SerializeField]
		private GameObject visibleMarkerPrefab;

		[SerializeField]
		private Transform visibleMarkerParent;

		[Header("GameManager - Status")]
		[SerializeField, ReadOnly]
		private DefineTeam currentTurn;
		
		[SerializeField, ReadOnly]
		private PieceBased[] piecePositions = new PieceBased[64];
		
		[SerializeField, ReadOnly]
		private PieceBased selectedPiece;
		
		[SerializeField, ReadOnly]
		private List<GameObject> activeVisibleMarkers = new List<GameObject>();
		
		[SerializeField, ReadOnly]
		private List<GameObject> inactiveVisibleMarkers = new List<GameObject>();
		
		private void Start()
		{
			currentTurn = DefineTeam.White;
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

		protected override void Update()
		{
			base.Update();
			
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out RaycastHit hit))
				{
					PieceBased piece = hit.transform.GetComponent<PieceBased>();
					if (piece != null && piece.Team == currentTurn)
					{
						OnPieceClicked(piece);
					}
					else
					{
						for (int i = 0; i < boardMarkers.Length; i++)
						{
							if (hit.transform == boardMarkers[i])
							{
								OnMarkerClicked(i);
								break;
							}
						}
					}
				}
			}
		}

		private void OnMarkerClicked(int index)
		{
			if (index >= 0 && index < 64) return;

			if (selectedPiece == null)
			{
				if (piecePositions[index] != null)
				{
					OnPieceClicked(piecePositions[index]);
				}
			}
			else
			{
				//TODO
			}
		}

		private void OnPieceClicked(PieceBased piece)
		{
			if (piece.Team != currentTurn) return;
			
			selectedPiece = piece;
			
			RemoveAllVisibleMarker();
			
			List<int> moves = piece.GetAvailableMoves(boardMarkers, piecePositions);
			foreach (int index in moves)
			{
				if (!IsSafetyPosition(piece, index)) continue;

				GetVisibleMarker(boardMarkers[index].position, Quaternion.identity);
			}
		}

		private GameObject GetVisibleMarker(Vector3 position, Quaternion rotation)
		{
			GameObject marker;
			
			if (inactiveVisibleMarkers.Count > 0)
			{
				marker = inactiveVisibleMarkers[0];
				inactiveVisibleMarkers.RemoveAt(0);
				marker.transform.position = position;
				marker.transform.rotation = rotation;
			}
			else
			{
				marker = Instantiate(visibleMarkerPrefab, position, rotation, visibleMarkerParent);
			}
			marker.gameObject.SetActive(true);
			activeVisibleMarkers.Add(marker);
			
			return marker;
		}

		private void RemoveAllVisibleMarker()
		{
			foreach (GameObject marker in activeVisibleMarkers.ToList())
			{
				marker.SetActive(false);
				activeVisibleMarkers.Remove(marker);
				inactiveVisibleMarkers.Add(marker);
			}
		}
		
		private PieceBased FindKing(DefineTeam team)
		{
			foreach (PieceBased piece in piecePositions)
			{
				if (piece != null && piece is KingPiece && piece.Team == team)
					return piece;
			}
			return null;
		}

		private bool IsPositionUnderThreat(int index, DefineTeam byTeam)
		{
			foreach (PieceBased piece in piecePositions)
			{
				if (piece == null || piece.Team != byTeam) continue;
				List<int> moves = piece.GetAvailableMoves(boardMarkers, piecePositions);
				if (moves.Contains(index)) return true;
			}
			return false;
		}
		
		private bool IsSafetyPosition(PieceBased piece, int targetIndex)
		{
			int originalIndex = piece.CurrentIndex;
			PieceBased targetPiece = piecePositions[targetIndex];

			piecePositions[originalIndex] = null;
			piecePositions[targetIndex] = piece;
			piece.Set(index: targetIndex);

			PieceBased king = FindKing(piece.Team);
			bool inCheck = IsPositionUnderThreat(king.CurrentIndex, (piece.Team == DefineTeam.White) ? DefineTeam.Black : DefineTeam.White);

			piecePositions[originalIndex] = piece;
			piecePositions[targetIndex] = targetPiece;
			piece.Set(index: originalIndex);

			return !inCheck;
		}
	}
}
