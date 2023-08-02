using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, Header("��")] GameObject _cell;
    [SerializeField] GridLayoutGroup _gridLayoutGroup;

    [SerializeField, Header("�c")] int _rows;
    [SerializeField, Header("��")] int _columns;

    GameObject[,] _bords;


    public PieceColor[,] _pieceColor;

    [SerializeField, Header("��")] GameObject _piece;

    [SerializeField, Header("�ǂ���̃^�[��������")] bool _trunChange = true;

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
            PieceColor pieceColor = _trunChange ? PieceColor.White : PieceColor.Black;
            if (_pieceColor[row, column] == PieceColor.Empty && InstantiateCheck(row, column, pieceColor))
            {
                var piece = Instantiate(_piece, _bords[row, column].transform); //�}�X�ڂ���Ȃ琶��
                if (_trunChange)
                {
                    piece.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                    _pieceColor[row, column] = PieceColor.White;
                    FlipPieces(row, column, pieceColor);
                }
                else
                {
                    piece.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                    _pieceColor[row, column] = PieceColor.Black;
                    FlipPieces(row, column, pieceColor);
                }
                _trunChange = !_trunChange;
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
        // �������̃I�t�Z�b�g���`
        int[] dx = { 1, 1, 1, 0, 0, -1, -1, -1 };
        int[] dy = { 1, 0, -1, 1, -1, 1, 0, -1 };

        // ���ׂĂ̔������ɑ΂��ă`�F�b�N
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
        // 1�ׂ̃Z���̍��W���v�Z
        int newRow = row + dx;
        int newColumn = column + dy;

        // 1�ׂ̃Z�����Ֆʓ����m�F
        if (newRow >= 0 && newRow < _rows && newColumn >= 0 && newColumn < _columns)
        {
            // 1�ׂ̃Z������̏ꍇ�͋��߂Ȃ�
            if (_pieceColor[newRow, newColumn] == PieceColor.Empty)
            {
                return false;
            }
            // 1�ׂ̃Z��������̐F�̏ꍇ�A����ɒT�����s��
            if (_pieceColor[newRow, newColumn] != colorCheck)
            {
                _eightCheckCount++;
                return CheckInDirection(newRow, newColumn, dx, dy, colorCheck);
            }
            if (_pieceColor[newRow, newColumn] == colorCheck && _eightCheckCount != 0)
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
    public void FlipPieces(int row, int column, PieceColor colorCheck)
    {
        int[] dx = { 1, 1, 1, 0, 0, -1, -1, -1 };
        int[] dy = { 1, 0, -1, 1, -1, 1, 0, -1 };

        // ���ׂĂ̔������ɑ΂��ă`�F�b�N
        for (int i = 0; i < dx.Length; i++)
        {
            int count = _eightCheckCount;
            if (CheckInDirection(row, column, dx[i], dy[i], colorCheck))
            {
                int newRow = row + dx[i];
                int newColumn = column + dy[i];

                // ���Ԃ�����
                for (int j = 0; j < count; j++)
                {
                    if (_bords[newRow, newColumn] != null && _bords[newRow, newColumn].transform.childCount > 0)
                    {
                        _pieceColor[newRow, newColumn] = colorCheck;
                        _bords[newRow, newColumn].transform.GetChild(0).localRotation = _trunChange ? Quaternion.Euler(-90f, 0f, 0f) : Quaternion.Euler(90f, 0f, 0f);
                    }
                    newRow += dx[i];
                    newColumn += dy[i];
                }
            }
        }
    }

}
public enum PieceColor //���enum
{
    Empty, //�����Ȃ�
    White,//�z���C�g
    Black //�u���b�N
}
