using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, Header("°")] GameObject _cell;
    [SerializeField] GridLayoutGroup _gridLayoutGroup;

    [SerializeField, Header("c")] int _rows;
    [SerializeField, Header("‘")] int _column;

    GameObject[,] _bords;


    public PieceColor[,] _pieceColor;

    [SerializeField, Header("ξ")] GameObject _piece;

    // Start is called before the first frame update
    void Start()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _column;
        _bords = new GameObject[_rows, _column];
        _pieceColor = new PieceColor[_rows, _column];
        var parent = _gridLayoutGroup.transform;
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _column; j++)
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
            for(int j = 0; j < _column; j++)
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
}
public enum PieceColor //ξΜenum
{
    Empty, //½ΰΘ΅
    White,//zCg
    Black //ubN
}
