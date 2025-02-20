using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ProfileInfoBox : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerName, _accountBalance, _winCount, _lostCount;

    [Inject] private readonly PlayerDataSaveSystem _playerDataSaveSystem;

    private void OnEnable()
    {
        Event.OnPlayerDataLoaded += OnPlayerDataLoaded;
        Event.OnPlayerDataChanged += OnPlayerDataChanged;
    }

    private void OnDisable()
    {
        Event.OnPlayerDataLoaded -= OnPlayerDataLoaded;
        Event.OnPlayerDataChanged -= OnPlayerDataChanged;
    }

    private void OnPlayerDataLoaded() => RefreshView();
    private void OnPlayerDataChanged() => RefreshView();

    private void RefreshView()
    {
        var data = _playerDataSaveSystem.Get();

        _playerName.text = data.Name;
        _accountBalance.text = $"{data.AccountBalance} $";

        if (_winCount)
            _winCount.text = data.WinCount.ToString();
        if (_lostCount)
            _lostCount.text = data.LostCount.ToString();
    }
}