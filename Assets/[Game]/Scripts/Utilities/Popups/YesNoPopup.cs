using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Game.Scripts.View
{
    public class YesNoPopup : PopUpBase
    {
        private Action _callback;

        [SerializeField] private Button _yesBtn;
        [SerializeField] private Button _noBtn;
        [SerializeField] private TMP_Text _titleText, _messageText;

        private void OnEnable()
        {
            _yesBtn.onClick.AddListener(YesBtn_Click);
            _noBtn.onClick.AddListener(Hide);
        }

        private void OnDisable()
        {
            _yesBtn.onClick.RemoveAllListeners();
            _noBtn.onClick.RemoveAllListeners();
        }

        public void Show(Action yesCallback, string title, string message)
        {
            base.Show();
            
            _callback = yesCallback;
            _titleText.text = title;
            _messageText.text = message;
        }

        private void YesBtn_Click()
        {
            _callback?.Invoke();
            
            _yesBtn.onClick.RemoveAllListeners();

            base.Hide();
        }
    }
}