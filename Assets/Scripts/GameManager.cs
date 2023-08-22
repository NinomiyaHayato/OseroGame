using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField, Header("����̌�")] Text _whiteText;
    [SerializeField, Header("����̌�")] Text _blackText;
    [SerializeField, Header("���݂̃^�[����")] Text _turnText;
    [SerializeField, Header("��������")] Text _timeText;
    [SerializeField, Header("�����Č����̃J�E���g")]public Text _recordCountText;
    [SerializeField, Header("�R�s�[��������̃R�[�h")] public Text _copyCount;
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
        _whiteText.text = $"����:{whiteCount.ToString("00")}";
        _blackText.text = $"����:{blackCount.ToString("00")}";
        _turnText.text = $"�^�[����:{turnCount.ToString("00")}";
    }
    public void RecordCount(int count)
    {
        _recordCountText.text = $"�����Č���\n  {count}�^�[����";
    }
    public void CopyTextBottun() //�����̃R�s�[������֐�
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
        _timeText.text = $"��������:{time.ToString("00")}�b";
    }
}
