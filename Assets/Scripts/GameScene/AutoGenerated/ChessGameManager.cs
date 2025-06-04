#if FALSE

using System.Collections.Generic;
using TMPro;
using UnityEngine;

using Chess_Client;
using Chess_Client.GameScene.Pieces;

public class ChessGameManager : MonoBehaviour
{
    public Transform[] boardSquares;
    public GameObject[] whitePrefabs;
    public GameObject[] blackPrefabs;
    public Material highlightMaterial;
    public GameObject moveMarkerPrefab;
    public GameObject promotionPanel;
    public TextMeshProUGUI turnText;

    private PieceBased[] piecePositions = new PieceBased[64];
    private List<GameObject> moveMarkers = new List<GameObject>();
    private PieceBased selectedPiece = null;
    private DefineTeam currentTurn = DefineTeam.White;
    private int promotionIndex = -1;
    private DefineTeam promotionTeam;

    void Start()
    {
        SpawnAllPieces();
        turnText.text = $"{currentTurn} 턴";
    }

    void SpawnAllPieces()
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

    void CreatePiece(GameObject prefab, int index, DefineTeam team)
    {
        GameObject obj = Instantiate(prefab, boardSquares[index].position, Quaternion.identity);
        PieceBased cp = obj.GetComponent<PieceBased>();
        cp.Team = team;
        cp.CurrentIndex = index;
        piecePositions[inde	x] = cp;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                for (int i = 0; i < boardSquares.Length; i++)
                {
                    if (hit.transform == boardSquares[i])
                    {
                        HandleSquareClick(i);
                        break;
                    }
                }

                PieceBased cp = hit.transform.GetComponent<PieceBased>();
                if (cp != null && cp.Team == currentTurn)
                {
                    SelectPiece(cp);
                }
            }
        }
    }

    void HandleSquareClick(int index)
    {
        if (selectedPiece == null) return;
        TryMoveSelectedPiece(index);
    }

    public void SelectPiece(PieceBased piece)
    {
        if (piece == null || piece.Team != currentTurn) return;

        selectedPiece = piece;
        ClearMoveMarkers();

        List<int> moves = piece.GetAvailableMoves(boardSquares, piecePositions);
        foreach (int index in moves)
        {
            if (!SimulateMoveAndCheckSafety(piece, index)) continue;

            GameObject marker = Instantiate(moveMarkerPrefab, boardSquares[index].position + Vector3.up * 0.01f, Quaternion.identity);
            moveMarkers.Add(marker);
        }
    }

    private void ClearMoveMarkers()
    {
        foreach (var marker in moveMarkers)
            Destroy(marker);
        moveMarkers.Clear();
    }

    void TryMoveSelectedPiece(int index)
    {
        if (selectedPiece == null) return;

        List<int> legalMoves = selectedPiece.GetAvailableMoves(boardSquares, piecePositions);
        if (!legalMoves.Contains(index)) return;
        if (!SimulateMoveAndCheckSafety(selectedPiece, index)) return;

        MoveTo(selectedPiece, index);
        PromotePawnIfNeeded(selectedPiece);
    }

    void MoveTo(PieceBased piece, int index)
    {
        if (piecePositions[index] != null)
            Destroy(piecePositions[index].gameObject);

        piecePositions[piece.CurrentIndex] = null;
        piecePositions[index] = piece;
        piece.CurrentIndex = index;

        piece.transform.position = boardSquares[index].position;
        EndTurn();
    }

    public void EndTurn()
    {
        currentTurn = (currentTurn == DefineTeam.White) ? DefineTeam.Black : DefineTeam.White;
        selectedPiece = null;
        turnText.text = $"{currentTurn} 턴";
        ClearMoveMarkers();
        EvaluateGameState();
    }

    public void EvaluateGameState()
    {
        DefineTeam enemyTeam = currentTurn;
        DefineTeam friendlyTeam = (currentTurn == DefineTeam.White) ? DefineTeam.Black : DefineTeam.White;

        PieceBased king = FindKing(enemyTeam);
        if (king == null)
        {
            Debug.Log($"{friendlyTeam} 승리! (킹 없음)");
            return;
        }

        bool inCheck = IsSquareUnderThreat(king.CurrentIndex, friendlyTeam);
        bool hasMoves = false;

        foreach (PieceBased piece in piecePositions)
        {
            if (piece == null || piece.Team != enemyTeam) continue;
            List<int> moves = piece.GetAvailableMoves(boardSquares, piecePositions);
            foreach (int move in moves)
            {
                if (SimulateMoveAndCheckSafety(piece, move))
                {
                    hasMoves = true;
                    break;
                }
            }
            if (hasMoves) break;
        }

        if (inCheck && !hasMoves)
        {
            Debug.Log($"{friendlyTeam} 승리! (체크메이트)");
        }
        else if (!inCheck && !hasMoves)
        {
            Debug.Log("무승부 (스테일메이트)");
        }
        else if (inCheck)
        {
            Debug.Log($"{enemyTeam} 체크!");
        }
    }

    public PieceBased FindKing(DefineTeam team)
    {
        foreach (PieceBased piece in piecePositions)
        {
            if (piece != null && piece is KingPiece && piece.Team == team)
                return piece;
        }
        return null;
    }

    public bool IsSquareUnderThreat(int index, DefineTeam byTeam)
    {
        foreach (PieceBased piece in piecePositions)
        {
            if (piece == null || piece.Team != byTeam) continue;
            List<int> moves = piece.GetAvailableMoves(boardSquares, piecePositions);
            if (moves.Contains(index)) return true;
        }
        return false;
    }

    public bool SimulateMoveAndCheckSafety(PieceBased piece, int targetIndex)
    {
        int originalIndex = piece.CurrentIndex;
        PieceBased targetPiece = piecePositions[targetIndex];

        piecePositions[originalIndex] = null;
        piecePositions[targetIndex] = piece;
        piece.CurrentIndex = targetIndex;

        PieceBased king = FindKing(piece.Team);
        bool inCheck = IsSquareUnderThreat(king.CurrentIndex, (piece.Team == DefineTeam.White) ? DefineTeam.Black : DefineTeam.White);

        piecePositions[originalIndex] = piece;
        piecePositions[targetIndex] = targetPiece;
        piece.CurrentIndex = originalIndex;

        return !inCheck;
    }

    public void PromotePawnIfNeeded(PieceBased piece)
    {
        if (!(piece is PawnPiece)) return;

        int row = piece.CurrentIndex / 8;
        if ((piece.Team == DefineTeam.White && row == 0) || (piece.Team == DefineTeam.Black && row == 7))
        {
            promotionIndex = piece.CurrentIndex;
            promotionTeam = piece.Team;
            promotionPanel.SetActive(true);
        }
    }

    public void OnPromoteToQueen() => ReplacePromotedPiece(3);
    public void OnPromoteToRook() => ReplacePromotedPiece(0);
    public void OnPromoteToBishop() => ReplacePromotedPiece(2);
    public void OnPromoteToKnight() => ReplacePromotedPiece(1);

    private void ReplacePromotedPiece(int prefabIndex)
    {
        Destroy(piecePositions[promotionIndex].gameObject);

        GameObject prefab = (promotionTeam == DefineTeam.White) ? whitePrefabs[prefabIndex] : blackPrefabs[prefabIndex];
        GameObject newPiece = Instantiate(prefab, boardSquares[promotionIndex].position, Quaternion.identity);
        PieceBased cp = newPiece.GetComponent<PieceBased>();
        cp.Team = promotionTeam;
        cp.CurrentIndex = promotionIndex;
        piecePositions[promotionIndex] = cp;

        promotionPanel.SetActive(false);
        EndTurn();
    }
}
#endif
