using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BoardController : MonoBehaviour,IPointerClickHandler
{
    [SerializeField, Header("ãÓ")] GameObject _piece;
    [SerializeField] GridLayoutGroup _gridLayoutGroup;

    [SerializeField, Header("èc")] int _rows;
    [SerializeField, Header("â°")] int _column;

    GameObject[,] _bords;

    public CellState[,] _cellState;

    // Start is called before the first frame update
    void Start()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _column;
        _bords = new GameObject[_rows, _column];
        _cellState = new CellState[_rows, _column];
        var parent = _gridLayoutGroup.transform;
        for(int i = 0; i < _rows; i++)
        {
            for(int j = 0; j < _column; j++)
            {
                var piece = Instantiate(_piece);
                piece.transform.SetParent(parent);
                _bords[i, j] = piece;
                _cellState[i, j] = CellState.Empty;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        var target = eventData.pointerCurrentRaycast.gameObject;
    }
}
public enum CellState
{
    Empty,
    PiecePlaced,
}
