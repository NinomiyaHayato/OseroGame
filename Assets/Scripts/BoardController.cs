using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, Header("床")] GameObject _cell;
    [SerializeField] GridLayoutGroup _gridLayoutGroup;

    [SerializeField, Header("縦")] int _rows;
    [SerializeField, Header("横")] int _columns;

    GameObject[,] _bords;


    public PieceColor[,] _pieceColor;

    [SerializeField, Header("駒")] GameObject _piece;

    // Start is called before the first frame update
    void Start()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;
        _bords = new GameObject[_rows, _columns];
        _pieceColor = new PieceColor[_rows, _columns];
        var parent = _gridLayoutGroup.transform;
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                var floor = Instantiate(_cell, transform);
                _bords[i, j] = floor;
                _pieceColor[i, j] = PieceColor.Empty;
                if (i == 3 && j == 3 || i == 4 && j == 4)
                {
                    var piece = Instantiate(_piece, _bords[i, j].transform);
                    _pieceColor[i, j] = PieceColor.White;
                    piece.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                }
                else if (i == 3 && j == 4 || i == 4 && j == 3)
                {
                    var piece = Instantiate(_piece, _bords[i, j].transform);
                    _pieceColor[i, j] = PieceColor.Black;
                    piece.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        var target = eventData.pointerCurrentRaycast.gameObject;
        if(ClickCheck(target,out var row,out var column))
        {
            if(_pieceColor[row,column] == PieceColor.Empty)
            {
                var piece = Instantiate(_piece, _bords[row, column].transform);
                piece.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                _pieceColor[row, column] = PieceColor.White;
                
            }
        }
    }
    public bool ClickCheck(GameObject piece, out int row,out int column)
    {
        for(int i = 0; i < _rows; i++)
        {
            for(int j = 0; j < _columns; j++)
            {
                if (piece == _bords[i, j])
                {
                    row = i;
                    column = j;
                    return true;
                }

            }
        }
        row = 0;column = 0;
        return false;
    }
    public bool InstantiateCheck(int row,int column)
    {
        return false;
    }
    //public void EightDirectionsCheck(int row , int column)
    //{
    //    if (row >= 0 && column >= 0 && row < _rows && column < _columns)
    //    {
    //        var cellColor = _pieceColor[row, column];
    //    }
    //}
    //private void ExpandCell(int row, int column)
    //{
    //    if (row >= 0 && column >= 0 && row < _rows && column < _columns && _pieceColor[row,column] != PieceColor.Empty)
    //    {
    //        EightDirectionsCheck(row, column);
    //    }
    //}
}
public enum PieceColor //駒のenum
{
    Empty, //何もなし
    White,//ホワイト
    Black //ブラック
}
