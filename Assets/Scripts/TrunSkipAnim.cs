using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TrunSkipAnim : MonoBehaviour
{
    [SerializeField] Text _skipText;
    [SerializeField] RectTransform _canvasRectTransform;
    [SerializeField, Header("何秒間かけてanimationするか")] float _animTime;
    [SerializeField, Header("スタートする位置")] Vector3 _startPos;
    [SerializeField, Header("終わりの位置")] Vector3 _endPos;
    public void SkipAnim()
    {
        _skipText.gameObject.SetActive(true);
        _skipText.rectTransform.anchoredPosition = _startPos;

        _skipText.rectTransform.DOAnchorPos(_endPos, _animTime).SetEase(Ease.OutQuint).OnComplete(() =>
        {
            _skipText.gameObject.SetActive(false);
        });
    }
}
