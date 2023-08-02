using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField, Header("����̌�")] Text _whiteText;
    [SerializeField, Header("����̌�")] Text _blackText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Situation(int whiteCount,int blackCount)
    {
        _whiteText.text = $"����:{whiteCount.ToString("00")}";
        _blackText.text = $"����:{blackCount.ToString("00")}";
    }
}
