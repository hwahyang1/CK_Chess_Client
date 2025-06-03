using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChessGameManager : MonoBehaviour
{
    public Transform[] boardSquares;
    public GameObject[] whitePrefabs;
    public GameObject[] blackPrefabs;
    public Material highlightMaterial;
    public GameObject moveMarkerPrefab;
    public GameObject promotionPanel;
    public TextMeshProUGUI turnText;

    private ChessPiece[] piecePositions = new ChessPiece[64];
    private List<GameObject> moveMarkers = new List<GameObject>();
    private ChessPiece selectedPiece = null;
    private Team currentTurn = Team.White;
    private int promotionIndex = -1;
    private Team promotionTeam;

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
            CreatePiece(whitePrefabs[whiteLayout[i]], i, Team.White);
            CreatePiece(whitePrefabs[5], i + 8, Team.White);
            CreatePiece(blackPrefabs[5], i + 48, Team.Black);
            CreatePiece(blackPrefabs[blackLayout[i]], i + 56, Team.Black);
        }
    }

    void CreatePiece(GameObject prefab, int index, Team team)
    {
        GameObject obj = Instantiate(prefab, boardSquares[index].position, Quaternion.identity);
        ChessPiece cp = obj.GetComponent<ChessPiece>();
        cp.team = team;
        cp.currentIndex = index;
        piecePositions[index] = cp;
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

                ChessPiece cp = hit.transform.GetComponent<ChessPiece>();
                if (cp != null && cp.team == currentTurn)
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

    public void SelectPiece(ChessPiece piece)
    {
        if (piece == null || piece.team != currentTurn) return;

        selectedPiece = piece;
        ClearMoveMarkers();

        List<int> moves = piece.GetLegalMoves(boardSquares, piecePositions);
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

        List<int> legalMoves = selectedPiece.GetLegalMoves(boardSquares, piecePositions);
        if (!legalMoves.Contains(index)) return;
        if (!SimulateMoveAndCheckSafety(selectedPiece, index)) return;

        MoveTo(selectedPiece, index);
        PromotePawnIfNeeded(selectedPiece);
    }

    void MoveTo(ChessPiece piece, int index)
    {
        if (piecePositions[index] != null)
            Destroy(piecePositions[index].gameObject);

        piecePositions[piece.currentIndex] = null;
        piecePositions[index] = piece;
        piece.currentIndex = index;

        piece.transform.position = boardSquares[index].position;
        EndTurn();
    }

    public void EndTurn()
    {
        currentTurn = (currentTurn == Team.White) ? Team.Black : Team.White;
        selectedPiece = null;
        turnText.text = $"{currentTurn} 턴";
        ClearMoveMarkers();
        EvaluateGameState();
    }

    public void EvaluateGameState()
    {
        Team enemyTeam = currentTurn;
        Team friendlyTeam = (currentTurn == Team.White) ? Team.Black : Team.White;

        ChessPiece king = FindKing(enemyTeam);
        if (king == null)
        {
            Debug.Log($"{friendlyTeam} 승리! (킹 없음)");
            return;
        }

        bool inCheck = IsSquareUnderThreat(king.currentIndex, friendlyTeam);
        bool hasMoves = false;

        foreach (ChessPiece piece in piecePositions)
        {
            if (piece == null || piece.team != enemyTeam) continue;
            List<int> moves = piece.GetLegalMoves(boardSquares, piecePositions);
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

    public ChessPiece FindKing(Team team)
    {
        foreach (ChessPiece piece in piecePositions)
        {
            if (piece != null && piece is KingPiece && piece.team == team)
                return piece;
        }
        return null;
    }

    public bool IsSquareUnderThreat(int index, Team byTeam)
    {
        foreach (ChessPiece piece in piecePositions)
        {
            if (piece == null || piece.team != byTeam) continue;
            List<int> moves = piece.GetLegalMoves(boardSquares, piecePositions);
            if (moves.Contains(index)) return true;
        }
        return false;
    }

    public bool SimulateMoveAndCheckSafety(ChessPiece piece, int targetIndex)
    {
        int originalIndex = piece.currentIndex;
        ChessPiece targetPiece = piecePositions[targetIndex];

        piecePositions[originalIndex] = null;
        piecePositions[targetIndex] = piece;
        piece.currentIndex = targetIndex;

        ChessPiece king = FindKing(piece.team);
        bool inCheck = IsSquareUnderThreat(king.currentIndex, (piece.team == Team.White) ? Team.Black : Team.White);

        piecePositions[originalIndex] = piece;
        piecePositions[targetIndex] = targetPiece;
        piece.currentIndex = originalIndex;

        return !inCheck;
    }

    public void PromotePawnIfNeeded(ChessPiece piece)
    {
        if (!(piece is PawnPiece)) return;

        int row = piece.currentIndex / 8;
        if ((piece.team == Team.White && row == 0) || (piece.team == Team.Black && row == 7))
        {
            promotionIndex = piece.currentIndex;
            promotionTeam = piece.team;
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

        GameObject prefab = (promotionTeam == Team.White) ? whitePrefabs[prefabIndex] : blackPrefabs[prefabIndex];
        GameObject newPiece = Instantiate(prefab, boardSquares[promotionIndex].position, Quaternion.identity);
        ChessPiece cp = newPiece.GetComponent<ChessPiece>();
        cp.team = promotionTeam;
        cp.currentIndex = promotionIndex;
        piecePositions[promotionIndex] = cp;

        promotionPanel.SetActive(false);
        EndTurn();
    }
}