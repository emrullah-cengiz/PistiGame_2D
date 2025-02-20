using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public RectTransform RectTransform;
    public Image BGImage;

    private CanvasGroup _canvasGroup;

    private void Start()
    {
        BGImage = transform.GetComponent<Image>();
        RectTransform = transform as RectTransform;
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