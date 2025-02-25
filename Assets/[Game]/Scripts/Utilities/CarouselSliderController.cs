using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CarouselSliderController : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup _roomLayoutGroup;

    [SerializeField] private Button _prevButton;
    [SerializeField] private Button _nextButton;

    private float _offset, _scrollStep;
    private int _index, _itemCount;
    private bool _disabled;

    public void Setup()
    {
        _prevButton.onClick.AddListener(() => Swipe(true).Forget());
        _nextButton.onClick.AddListener(() => Swipe(false).Forget());

        var child = ((RectTransform)_roomLayoutGroup.transform.GetChild(0).transform);
        _itemCount = _roomLayoutGroup.transform.childCount;

        _scrollStep = child.sizeDelta.x + _roomLayoutGroup.padding.left +
                      _roomLayoutGroup.padding.right - _roomLayoutGroup.spacing;

        UpdateButtons();
    }

    private async UniTask Swipe(bool isLeft)
    {
        if (_disabled) return;

        int newIndex = _index + (isLeft ? -1 : 1);
        if (newIndex < 0 || newIndex >= _itemCount) return;

        _index = newIndex;
        _disabled = true;

        try
        {
            await _roomLayoutGroup.transform.TWLocalMove(
                _roomLayoutGroup.transform.localPosition + _scrollStep * (isLeft ? Vector3.right : Vector3.left),
                0.3f, TweenExtensions.EaseType.EaseOutQuint
            );
        }
        finally
        {
            _disabled = false;
            UpdateButtons();
        }
    }

    private void UpdateButtons()
    {
        _prevButton.interactable = _index > 0;
        _nextButton.interactable = _index < _itemCount - 1;
    }

}