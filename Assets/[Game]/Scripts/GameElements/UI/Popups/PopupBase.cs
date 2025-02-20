using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public abstract class PopupBase : MonoBehaviour
{
    [SerializeField] private Button _closeBtn;
    
    private Action _onClose;

    private void Close()
    {
        _onClose?.Invoke();
        Destroy(gameObject);
        Destroy(this);
    }
}
