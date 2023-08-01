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

    [SerializeField, Header("どちらのターンか判定")] bool _trunChange = true;

    int _eightCheckCount = 0;

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
        if (ClickCheck(target, out var row, out var column))
        {
            if (_pieceColor[row, column] == PieceColor.Empty && InstantiateCheck(row,column,PieceColor.White))
            {
                var piece = Instantiate(_piece, _bords[row, column].transform); //マス目が空なら生成
                piece.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                _pieceColor[row, column] = PieceColor.White;

            }
        }
    }
    public bool ClickCheck(GameObject piece, out int row, out int column)
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                if (piece == _bords[i, j])
                {
                    row = i;
                    column = j;
                    return true;
                }

            }
        }
        row = 0; column = 0;
        return false;
    }
    public bool InstantiateCheck(int row, int column, PieceColor colorCheck)
    {
        // 八方向のオフセットを定義
        int[] dx = { 1, 1, 1, 0, 0, -1, -1, -1 };
        int[] dy = { 1, 0, -1, 1, -1, 1, 0, -1 };

        // すべての八方向に対してチェック
        for (int i = 0; i < dx.Length; i++)
        {
            _eightCheckCount = 0;
            if (CheckInDirection(row, column, dx[i], dy[i], colorCheck))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInDirection(int row, int column, int dx, int dy, PieceColor colorCheck)
    {
        // 1つ隣のセルの座標を計算
        int newRow = row + dx;
        int newColumn = column + dy;

        // 1つ隣のセルが盤面内か確認
        if (newRow >= 0 && newRow < _rows && newColumn >= 0 && newColumn < _columns)
        {
            // 1つ隣のセルが空の場合は挟めない
            if (_pieceColor[newRow, newColumn] == PieceColor.Empty)
            {
                return false;
            }
            // 1つ隣のセルが相手の色の場合、さらに探索を行う
            if (_pieceColor[newRow, newColumn] != colorCheck)
            {
                _eightCheckCount++;
                return CheckInDirection(newRow, newColumn, dx, dy, colorCheck);
            }
            if(_pieceColor[newRow,newColumn] == colorCheck && _eightCheckCount != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }
}
public enum PieceColor //駒のenum
{
    Empty, //何もなし
    White,//ホワイト
    Black //ブラック
}
