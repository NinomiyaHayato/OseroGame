using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private BoardController _boardController;
    public void AITurn()
    {
        int bestRow = -1;
        int bestColumn = -1;
        int maxCount = 0;
        _boardController = FindObjectOfType<BoardController>();
        for (int i = 0; i < _boardController._rows; i++)
        {
            for (int j = 0; j < _boardController._columns; j++)
            {
                if (_boardController._pieceColor[i, j] == PieceColor.Empty && _boardController.InstantiateCheck(i, j, PieceColor.Black))
                {
                    int pieceCount = CountPieces(i, j, PieceColor.Black);
                    if (pieceCount > maxCount)
                    {
                        bestRow = i;
                        bestColumn = j;
                        maxCount = pieceCount;
                    }
                }
            }
        }
        if (bestRow != -1 && bestColumn != -1)
        {
            _boardController.InstancePiece(bestRow, bestColumn);
        }
        else
        {
            _boardController.StartCoroutine("TurnChanges");
        }
    }

    public int CountPieces(int row, int column, PieceColor pieceColor)
    {
        int pieceCount = 0;
        _boardController = FindObjectOfType<BoardController>();
        for (int i = 0; i < _boardController._dx.Length; i++)
        {
            int count = 0;
            if (_boardController.CheckInDirection(row, column, _boardController._dx[i], _boardController._dy[i], pieceColor))
            {
                count = _boardController._eightCheckCount;

                for (int j = 0; j < count; j++)
                {
                    pieceCount++;
                }
            }
        }
        return pieceCount;
    }
}