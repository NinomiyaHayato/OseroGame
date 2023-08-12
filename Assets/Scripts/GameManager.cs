using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField, Header("����̌�")] Text _whiteText;
    [SerializeField, Header("����̌�")] Text _blackText;
    [SerializeField, Header("���݂̃^�[����")] Text _turnText;
    // Start is called before the first frame update
    public void Situation(int whiteCount,int blackCount,int turnCount)
    {
        _whiteText.text = $"����:{whiteCount.ToString("00")}";
        _blackText.text = $"����:{blackCount.ToString("00")}";
        _turnText.text = $"�^�[����:{turnCount.ToString("00")}";
    }
}
