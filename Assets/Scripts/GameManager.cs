using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField, Header("����̌�")] Text _whiteText;
    [SerializeField, Header("����̌�")] Text _blackText;
    [SerializeField, Header("���݂̃^�[����")] Text _turnText;
    [SerializeField, Header("��������")] Text _timeText;
    [SerializeField, Header("�����Č����̃J�E���g")]public Text _recordCountText;
    private void Start()
    {
        _recordCountText.enabled = false;
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
    public void TimeText(float time)
    {
        _timeText.text = $"��������:{time.ToString("00")}�b";
    }
}
