using System;
using UnityEngine;
using UnityEngine.UI;


public abstract class PopUpBase : MonoBehaviour
{
    [SerializeField] protected Button _closeBtn;

    public virtual void Show()
    {
        _closeBtn.onClick.AddListener(Hide);
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        _closeBtn.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
