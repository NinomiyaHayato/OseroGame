using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField, Header("白駒の個数")] Text _whiteText;
    [SerializeField, Header("黒駒の個数")] Text _blackText;
    [SerializeField, Header("現在のターン数")] Text _turnText;
    // Start is called before the first frame update
    public void Situation(int whiteCount,int blackCount,int turnCount)
    {
        _whiteText.text = $"白駒:{whiteCount.ToString("00")}";
        _blackText.text = $"黒駒:{blackCount.ToString("00")}";
        _turnText.text = $"ターン数:{turnCount.ToString("00")}";
    }
}
