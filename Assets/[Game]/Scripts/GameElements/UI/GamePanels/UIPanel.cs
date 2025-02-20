using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public UIPanelType PanelType => _panelType;
    [SerializeField] private UIPanelType _panelType;
    
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _bGImage;
    [SerializeField] private CanvasGroup _canvasGroup;

    public virtual void Initialize()
    {
    }

    public void Open(float duration = 0) => Activate(duration, true).Forget();
    public void Close(float duration = 0) => Activate(duration, false).Forget();

    private async UniTask Activate(float duration, bool open)
    {
        if (duration <= 0)
        {
            gameObject.SetActive(open);
            return;
        }

        if (open)
            gameObject.SetActive(true);

        await _canvasGroup.TWFade(target: open ? 1 : 0, duration: duration);

        if (!open)
            gameObject.SetActive(false);
    }
}