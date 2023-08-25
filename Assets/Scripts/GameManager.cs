using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    [SerializeField, Header("白駒の個数")] Text _whiteText;
    [SerializeField, Header("黒駒の個数")] Text _blackText;
    [SerializeField, Header("現在のターン数")] Text _turnText;
    [SerializeField, Header("持ち時間")] Text _timeText;
    [SerializeField, Header("棋譜再現中のカウント")]public Text _recordCountText;
    [SerializeField, Header("コピーする棋譜のコード")] public Text _copyCount;
    BoardController _bordController;
    [SerializeField] InputField _inputField;
    public int[] _chr = new int[64];
    private void Start()
    {
        _recordCountText.enabled = false;
        _bordController = FindObjectOfType<BoardController>();
        _inputField = FindObjectOfType<InputField>();
    }
    public void Situation(int whiteCount, int blackCount, int turnCount)
    {
        _whiteText.text = $"白駒:{whiteCount.ToString("00")}";
        _blackText.text = $"黒駒:{blackCount.ToString("00")}";
        _turnText.text = $"ターン数:{turnCount.ToString("00")}";
    }
    public void RecordCount(int count)
    {
        _recordCountText.text = $"棋譜再現中\n  {count}ターン目";
    }
    public void CopyTextBottun() //棋譜のコピーをする関数
    {
        if(_bordController._lordCheck)
        {
            int count = 0;
            for(int i = 0; i < _bordController._rows; i++)
            {
                for(int j = 0; j < _bordController._columns; j++)
                {
                    if (_bordController._pieceColor[i, j] == PieceColor.White)
                    {
                        _chr[count] = 1;
                        count++;
                    }
                    else if (_bordController._pieceColor[i, j] == PieceColor.Black)
                    {
                        _chr[count] = 2;
                        count++;
                    }
                    else
                    {
                        _chr[count] = 0;
                        count++;
                    }
                }
            }
            GUIUtility.systemCopyBuffer = string.Join("", _chr.Select(num => num.ToString()));
        }
    }
    public void TimeText(float time)
    {
        _timeText.text = $"持ち時間:{time.ToString("00")}秒";
    }
}
