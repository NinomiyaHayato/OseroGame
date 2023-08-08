using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TrunSkipAnim : MonoBehaviour
{
    [SerializeField] Text _skipText;
    [SerializeField] RectTransform _canvasRectTransform;
    [SerializeField, Header("���b�Ԃ�����animation���邩")] float _animTime;
    [SerializeField, Header("�X�^�[�g����ʒu")] Vector3 _startPos;
    [SerializeField, Header("�I���̈ʒu")] Vector3 _endPos;
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
