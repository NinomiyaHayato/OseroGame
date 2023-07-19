using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BoardController : MonoBehaviour,IPointerClickHandler
{
    [SerializeField, Header("床")] GameObject _cell;
    [SerializeField] GridLayoutGroup _gridLayoutGroup;

    [SerializeField, Header("縦")] int _rows;
    [SerializeField, Header("横")] int _column;

    GameObject[,] _bords;

    public CellState[,] _cellState;
    public PieceColor[,] _pieceColor;

    [SerializeField,Header("駒")] GameObject _piece; 

    // Start is called before the first frame update
    void Start()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _column;
        _bords = new GameObject[_rows, _column];
        _cellState = new CellState[_rows, _column];
        _pieceColor = new PieceColor[_rows, _column];
        var parent = _gridLayoutGroup.transform;
        for(int i = 0; i < _rows; i++)
        {
            for(int j = 0; j < _column; j++)
            {
                var piece = Instantiate(_cell,transform);
                _bords[i, j] = piece;
                _cellState[i, j] = CellState.Empty;
                _pieceColor[i, j] = PieceColor.None;
                if(i == 3 && j == 3 || i == 3 && i == 3 && i == 4)
                {

                }
            }
        }
        StartSet();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        var target = eventData.pointerCurrentRaycast.gameObject;
    }
    public void StartSet()
    {
        var num = 3;
        for (int i = num; i <= num + 1; i++)
        {
            var target = _bords[i, num].transform;
            var target2 = _bords[i, num + 1].transform;
            var piece = Instantiate(_piece, target);
            piece.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
            var piece2 = Instantiate(_piece, target2);
            piece2.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}
public enum CellState //置いてあるかどうか
{
    Empty, //空
    PiecePlaced,//置いてある
}
public enum PieceColor //駒のenum
{
    None, //何もなし
    White,//ホワイト
    Black //ブラック
}
