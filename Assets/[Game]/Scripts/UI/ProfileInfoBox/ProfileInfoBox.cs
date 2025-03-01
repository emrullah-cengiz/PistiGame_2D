using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

public class ProfileInfoBox : MonoBehaviour, IStartable
{
    [SerializeField] protected TMP_Text _playerName, _accountBalance;

    #region Injects

    [Inject] private readonly PlayerDataSaveSystem _saveSystem;

    #endregion

    private void OnEnable()
    {
        Event.OnPlayerDataLoaded += OnPlayerDataLoaded;
        Event.OnPlayerDataChanged += OnPlayerDataChanged;
        
        RefreshView();
    }

    private void OnDisable()
    {
        Event.OnPlayerDataLoaded -= OnPlayerDataLoaded;
        Event.OnPlayerDataChanged -= OnPlayerDataChanged;
    }

    public void Start() => RefreshView();

    private void OnPlayerDataLoaded() => RefreshView();

    private void OnPlayerDataChanged() => RefreshView();

    private void RefreshView()
    {
        var data = _saveSystem.Data;

        _playerName.text = data.Name;
        _accountBalance.text = data.AccountBalance.ToAbbreviated();
    }
}