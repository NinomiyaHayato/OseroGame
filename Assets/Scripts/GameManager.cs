using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
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
    public int[] _chr = new int[64];
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
        _timeText.text = $"��������:{time.ToString("00")}�b";
    }
}
