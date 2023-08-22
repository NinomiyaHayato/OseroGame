using UnityEngine;
using UnityEngine.UI;

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
            GUIUtility.systemCopyBuffer = _bordController._gameRecordCount.ToString();
        }
    }
    public void GetInputNumber()
    {
        if(_bordController._lordCheck)
        {
            var number = int.Parse(_inputField.text);
            Debug.Log(number);
        }
    }
    public void TimeText(float time)
    {
        _timeText.text = $"持ち時間:{time.ToString("00")}秒";
    }
}
