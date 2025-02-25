using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableUIPanel : UIPanel
{
    [SerializeField] private List<TablePlayerInfoBox> _playerInfoBoxes;
    [SerializeField] private Button _backToLobbyBtn;

    private TableData _data;
    
    private void Start() => _backToLobbyBtn.onClick.AddListener(()=> Event.OnBackToLobbyBtn_Click?.Invoke());

    private void OnEnable()
    {
        Event.OnTablePlayerDataChanged += OnTablePlayerDataChanged;
        Event.OnTableInitialized += OnTableInitialized;
    }

    private void OnDisable()
    {
        Event.OnTablePlayerDataChanged -= OnTablePlayerDataChanged;
        Event.OnTableInitialized -= OnTableInitialized;
        
        foreach (var playerInfoBox in _playerInfoBoxes) 
            playerInfoBox.gameObject.SetActive(false);
    }

    private void OnTableInitialized(TableData data)
    {
        _data = data;
        
        if (data.Mode == TableMode.TwoPlayers)
        {
            _playerInfoBoxes[0].gameObject.SetActive(true);
            _playerInfoBoxes[2].gameObject.SetActive(true);
        }
        else
            for (var i = 0; i < _playerInfoBoxes.Count; i++)
                _playerInfoBoxes[i].gameObject.SetActive(true);
    }

    private void OnTablePlayerDataChanged(TablePlayerData data)
    {
        Debug.Log("Player " + data.PlayerIndex + " score updated...");

        int index;
        if (data.PlayerIndex > 0)
            index = _data.Mode == TableMode.TwoPlayers ? 2 : data.PlayerIndex;
        else
            index = data.PlayerIndex;

        _playerInfoBoxes[index].RefreshView(data);
    }
}