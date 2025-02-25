using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

public class ProfileInfoPopup : InfoPopup
{
    [SerializeField] protected TMP_Text _playerName, _accountBalance;
    [SerializeField] private TMP_Text _winCount, _lostCount;

    private PlayerData _data;
    
    public void Show(PlayerData data)
    {
        base.Show();
        
        _data = data;
        
        RefreshView();
    }

    private void RefreshView()
    {
        _playerName.text = _data.Name;
        _accountBalance.text = _data.AccountBalance.ToAbbreviated("$");
        _winCount.text = _data.WinCount.ToString();
        _lostCount.text = _data.LostCount.ToString();
    }
}