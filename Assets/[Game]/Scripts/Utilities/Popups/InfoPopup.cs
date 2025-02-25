using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPopup : PopUpBase
{
     protected Action _callback;

    [SerializeField] protected Button okBtn;
    [SerializeField] private TMP_Text txtMessage;

    public virtual void Show(Action callback = null, string message = null)
    {
        base.Show();

        okBtn?.onClick.AddListener(OkBtn_Click);

        _callback = callback;
        if (txtMessage)
            txtMessage.text = message;
    }

    protected virtual void OkBtn_Click()
    {
        _callback?.Invoke();

        okBtn?.onClick.RemoveAllListeners();

        base.Hide();
    }
}