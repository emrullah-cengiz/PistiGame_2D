using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using VContainer;
using Slider = UnityEngine.UI.Slider;

public class TableSessionResultPopup : InfoPopup
{
    [SerializeField] private TMP_Text _winTitleText, _loseTitle, _scoreText, _earnedMoneyText;

    private TableSessionResultData _data;

    public void Show(TableSessionResultData data, Action callback = null, string message = null)
    {
        _data = data;
        
        base.Show(callback, message);

        RefreshView();
        
        okBtn.onClick.AddListener(OkBtn_Click);
    }

    private void RefreshView()
    {
        _winTitleText.gameObject.SetActive(_data.IsWon);
        _loseTitle.gameObject.SetActive(!_data.IsWon);
            
        _scoreText.text = _data.Score.ToString();
        _earnedMoneyText.text = _data.EarnedMoney.ToAbbreviated(" $");
    }
}