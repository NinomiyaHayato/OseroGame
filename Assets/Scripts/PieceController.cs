using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceController : MonoBehaviour
{
    [SerializeField] public PieceColor _pieceColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum PieceColor //���enum
{
    None, //�����Ȃ�
    White,//�z���C�g
    Black //�u���b�N
}
