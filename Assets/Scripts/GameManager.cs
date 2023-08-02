using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField, Header("”’‹î‚ÌŒÂ”")] Text _whiteText;
    [SerializeField, Header("•‹î‚ÌŒÂ”")] Text _blackText;
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
        _whiteText.text = $"”’‹î:{whiteCount.ToString("00")}";
        _blackText.text = $"•‹î:{blackCount.ToString("00")}";
    }
}
