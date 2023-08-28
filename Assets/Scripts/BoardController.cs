using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class BoardController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField, Header("床")] GameObject _cell;
    [SerializeField] GridLayoutGroup _gridLayoutGroup;

    [SerializeField, Header("縦")] public int _rows;
    [SerializeField, Header("横")] public int _columns;

    GameObject[,] _bords;


    public PieceColor[,] _pieceColor;

    [SerializeField, Header("駒")] GameObject _piece;

    [SerializeField, Header("どちらのターンか判定")] private bool _trunChange = true;

    public int _eightCheckCount = 0;

    GameManager _gameManager;

    public int[] _dx = { 1, 1, 1, 0, 0, -1, -1, -1 }; //八方向チェック

    public int[] _dy = { 1, 0, -1, 1, -1, 1, 0, -1 }; //八方向チェック


    public List<PieceColor[,]> _pieceColorList = new List<PieceColor[,]>();

    public int _turnCount; //現在何手目か
    public int _gameRecordCount; //棋譜表示時に利用するカウント

    [SerializeField, Header("持ち時間")] public float _timeUp; //持ち時間
    [SerializeField] public float _nowTime; //時間の計測

    [SerializeField] bool _timeStop; //一時停止のためのフラグ
    [SerializeField] bool _clickStop; //クリックできるか否かのフラグ
    [SerializeField]public bool _lordCheck; //棋譜の状態なのかどうか

    [SerializeField] Text _winnerText;

    public bool PlayerTurn
    {
        get { return _trunChange; }
    }
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;
        _bords = new GameObject[_rows, _columns];
        _pieceColor = new PieceColor[_rows, _columns];
        _winnerText.enabled = false;
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
        CountReturn();
        BordSave();
    }
    private void Update()
    {
        if (_timeUp > 0)
        {
            if (!_timeStop)
            {
                _nowTime += Time.deltaTime;
                if (_nowTime >= 1)
                {
                    _timeUp -= 1; // 1秒減少
                    if (_timeUp <= 0)
                    {
                        TurnChange();
                        _nowTime = 0;
                        _timeUp = 10;
                    }
                    else
                    {
                        _nowTime = 0;
                    }
                }
            }

        }
        _gameManager.TimeText(_timeUp);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_clickStop)
        {
            var target = eventData.pointerCurrentRaycast.gameObject;
            if (ClickCheck(target, out var row, out var column))
            {
                InstancePiece(row, column);
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
        // すべての八方向に対してチェック
        for (int i = 0; i < _dx.Length; i++)
        {
            _eightCheckCount = 0;
            if (CheckInDirection(row, column, _dx[i], _dy[i], colorCheck))
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
        // すべての八方向に対してチェック
        for (int i = 0; i < _dx.Length; i++)
        {
            int count = _eightCheckCount;
            if (CheckInDirection(row, column, _dx[i], _dy[i], colorCheck))
            {
                int newRow = row + _dx[i];
                int newColumn = column + _dy[i];

                // 裏返す処理
                for (int j = 0; j < count; j++)
                {
                    if (newRow >= 0 && newRow < _rows && newColumn >= 0 && newColumn < _columns)
                    {
                        if (_bords[newRow, newColumn] != null && _bords[newRow, newColumn].transform.childCount > 0)
                        {
                            _pieceColor[newRow, newColumn] = colorCheck;
                            Transform pieceTransform = _bords[newRow, newColumn].transform.GetChild(0);
                            float targetAngle = _trunChange ? -90f : 90f;
                            pieceTransform.DORotate(new Vector3(targetAngle, 0f, 0f), 0.3f);
                        }
                    }
                    newRow += _dx[i];
                    newColumn += _dy[i];
                }
            }
        }
    }
    public void CountReturn() //盤面の駒の状況
    {
        int whiteCount = 0;
        int blackCount = 0;
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                if (_pieceColor[i, j] == PieceColor.White)
                {
                    whiteCount++;
                }
                else if (_pieceColor[i, j] == PieceColor.Black)
                {
                    blackCount++;
                }
            }
        }
        _gameManager.Situation(whiteCount, blackCount, _turnCount);
    }
    public void InstancePiece(int row, int column)　//駒の生成
    {
        PieceColor pieceColor = _trunChange ? PieceColor.White : PieceColor.Black;
        if (_pieceColor[row, column] == PieceColor.Empty && InstantiateCheck(row, column, pieceColor))
        {
            var piece = Instantiate(_piece, _bords[row, column].transform); //マス目が空なら生成
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
            _turnCount++;
            StartCoroutine("TurnChanges");
        }
    }
    private bool CanPlacePiece(PieceColor colorCheck) //駒を置けるかどうか
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                if (_pieceColor[i, j] == PieceColor.Empty && InstantiateCheck(i, j, colorCheck))
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void BordSave() //盤面の保存
    {
        PieceColor[,] currentBoardState = new PieceColor[_rows, _columns];

        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                currentBoardState[i, j] = _pieceColor[i, j];
            }
        }
        _pieceColorList.Add(currentBoardState);
    }
    public void BordLord() //ボタン押されたときに呼ぶ(ロード)
    {
        _lordCheck = !_lordCheck;
        if (_lordCheck)
        {
            _timeStop = true; //経過時間を止める
            _clickStop = true; //クリックを止める
            _gameRecordCount = _turnCount;
            _gameManager.RecordCount(_gameRecordCount);
            _gameManager._recordCountText.enabled = true;
        }
        else
        {
            BordLordInstance(_turnCount);
            _gameManager._recordCountText.enabled = false;
            _timeStop = false; //経過時間を再開
            _clickStop = false; //クリックを再開
        }
    }
    public void BordLordInstance(int num) //棋譜の生成
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                if (_bords[i, j].gameObject.transform.childCount > 0)
                {
                    Transform child = _bords[i, j].transform.GetChild(0);
                    Destroy(child.gameObject);
                }
            }
        }
        var recordData = _pieceColorList[num];
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                if (recordData[i, j] == PieceColor.White)
                {
                    var piece = Instantiate(_piece, _bords[i, j].transform);
                    piece.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                }
                else if (recordData[i, j] == PieceColor.Black)
                {
                    var piece = Instantiate(_piece, _bords[i, j].transform);
                    piece.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                }
            }
        }
    }
    public void RightPlus()//棋譜のターンプラス
    {
        if (_gameRecordCount < _turnCount && _lordCheck)
        {
            _gameRecordCount++;
            BordLordInstance(_gameRecordCount);
            _gameManager.RecordCount(_gameRecordCount);
        }
    }
    public void LeftMinus()//棋譜のターンマイナス
    {
        if (_gameRecordCount > 0 && _lordCheck)
        {
            _gameRecordCount--;
            BordLordInstance(_gameRecordCount);
            _gameManager.RecordCount(_gameRecordCount);
        }
    }
    public void InputNumber() //棋譜をinputFieldから再現
    {
        if(_lordCheck && _gameManager.ReturnNums().Length == 64)
        {
            var data = _gameManager.ReturnNums();
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (_bords[i, j].gameObject.transform.childCount > 0)
                    {
                        Transform child = _bords[i, j].transform.GetChild(0);
                        Destroy(child.gameObject);
                    }
                }
            }
            PieceColor[,] pieceColors = new PieceColor[_rows, _columns];
            for (int i = 0; i < _rows; i++)
            {
                for(int j = 0; j < _columns; j++)
                {
                    if(data[i,j] == '1')
                    {
                        var piece = Instantiate(_piece, _bords[i, j].transform);
                        piece.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                        pieceColors[i, j] = PieceColor.White;
                    }
                    else if(data[i,j] == '2')
                    {
                        var piece = Instantiate(_piece, _bords[i, j].transform);
                        piece.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                        pieceColors[i, j] = PieceColor.Black;
                    }
                    else if(data[i,j] == '0')
                    {
                        pieceColors[i, j] = PieceColor.Empty;
                    }
                }
            }
            _lordCheck = !_lordCheck;
            _pieceColor = pieceColors;
            BordSave();
            _gameManager._recordCountText.enabled = false;
            _timeStop = false; //経過時間を再開
            _clickStop = false; //クリックを再開
        }
    }
    public bool GameSet() //駒が置けるかどうかの確認
    {
        if (CanPlacePiece(PieceColor.White) || CanPlacePiece(PieceColor.Black))
        {
            return false;
        }
        return true;
    }
    public PieceColor GetWinner()　//どちらが勝ちか判定
    {
        int whiteCount = 0;
        int blackCount = 0;

        // 盤面上の駒の数を数える
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                if (_pieceColor[i, j] == PieceColor.White)
                {
                    whiteCount++;
                }
                else if (_pieceColor[i, j] == PieceColor.Black)
                {
                    blackCount++;
                }
            }
        }

        // 駒の数で勝者を判定
        if (whiteCount > blackCount)
        {
            return PieceColor.White;
        }
        else if (blackCount > whiteCount)
        {
            return PieceColor.Black;
        }
        else
        {
            return PieceColor.Empty; // 引き分けの場合
        }
    }
    public void TurnChange() //AIと手番の切り替え
    {
        if (GameSet())
        {
            PieceColor winner = GetWinner();
            if (winner == PieceColor.Empty)
            {
                _winnerText.text = "引き分け";
                _winnerText.enabled = true;
            }
            else
            {
                Debug.Log(winner + "の勝利！");
                _winnerText.enabled = true;
                _winnerText.text = $"{winner}の勝利";
            }
        }
        else
        {
            CountReturn();
            BordSave();
            PieceColor pieceColor = _trunChange ? PieceColor.Black : PieceColor.White;
            Debug.Log($"{pieceColor}のターン");
            _nowTime = 0;
            _timeUp = 10;
            _gameManager.TimeText(_timeUp);
            if (CanPlacePiece(pieceColor))
            {
                _trunChange = !_trunChange;
            }
            else
            {
                TrunSkipAnim trunSkipAnim = FindObjectOfType<TrunSkipAnim>();
                if (trunSkipAnim != null)
                {
                    trunSkipAnim.SkipAnim();
                }
            }
            if (!_trunChange)
            {
                AIController aIController = FindObjectOfType<AIController>();
                if (aIController != null)
                {
                    aIController.AITurn();
                }
            }
        }
    }
    public IEnumerator TurnChanges()
    {
        yield return new WaitForSeconds(0.5f);
        TurnChange();
    }
}
public enum PieceColor //駒のenum
{
    Empty, //何もなし
    White,//ホワイト
    Black //ブラック
}