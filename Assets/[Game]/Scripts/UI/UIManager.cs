using System;
using System.Collections.Generic;
using Assets._Game.Scripts.View;
using UnityEngine;
using VContainer;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class UIManager : SerializedMonoBehaviour
{
    [Inject] private PlayerDataSaveSystem _playerDataSaveSystem;

    [NonSerialized, OdinSerialize] private Dictionary<UIPanelType, UIPanel> _uiPanels;
    [NonSerialized, OdinSerialize] private Dictionary<PopupType, PopUpBase> _popups;

    [SerializeField] private UIPanel _loadingPanel;
    [SerializeField, Sirenix.OdinInspector.ReadOnly] private UIPanel _currentPanel;

    private void OnEnable()
    {
        Event.OnEnterLobby += OnEnterLobby;
        Event.OnTableSessionStart += OnTableSessionStart;
        Event.OnTableSessionGameEnd += OnTableSessionGameEnd;
        Event.OnTableSessionEnd += OnTableSessionEnd;

        Event.OnShowCreateTablePopup_Click += OnShowCreateTablePopup_Click;
        Event.OnFirstHiddenDiscardedCardsCollectedByUser += OnFirstHiddenDiscardedCardsCollectedByUser;
        Event.OnBackToLobbyBtn_Click += OnBackToLobbyBtnClick;
    }

    private void OnDisable()
    {
        Event.OnEnterLobby -= OnEnterLobby;
        Event.OnTableSessionStart -= OnTableSessionStart;
        Event.OnTableSessionGameEnd -= OnTableSessionGameEnd;
        Event.OnTableSessionEnd -= OnTableSessionEnd;
        Event.OnShowCreateTablePopup_Click -= OnShowCreateTablePopup_Click;
        Event.OnFirstHiddenDiscardedCardsCollectedByUser -= OnFirstHiddenDiscardedCardsCollectedByUser;
    }

    // private void Start()
    // {
    //     ((LobbyUIPanel)_uiPanels[UIPanelType.Lobby]).Initialize();
    // }

    private void OpenPanel(UIPanelType type)
    {
        if (_currentPanel)
            _currentPanel.Close();

        _currentPanel = _uiPanels[type];

        _currentPanel.Open(0.1f);

        _currentPanel.Initialize();
    }

    private void OnEnterLobby() => OpenPanel(UIPanelType.Lobby);

    private void OnTableSessionStart() => OpenPanel(UIPanelType.Table);

    private void OnBackToLobbyBtnClick()
    {
        var popup = (YesNoPopup)_popups[PopupType.WarningYesNo];
        popup.Show(() => Event.OnTableSessionGameExit?.Invoke(), "Leave table?", "You will lose your wager.");
    }

    private void OnTableSessionGameEnd(TableSessionResultData result)
    {
        var popup = (TableSessionResultPopup)_popups[PopupType.TableSessionResult];
        popup.Show(result, () => Event.OnBackToLobbyConfirmationBtn_Click?.Invoke());
    }

    private void OnTableSessionEnd() => OpenPanel(UIPanelType.Lobby);

    //Popups
    public void ShowProfilePopup()
    {
        var popup = (ProfileInfoPopup)_popups[PopupType.Profile];
        popup.Show(_playerDataSaveSystem.Data);
    }

    private void OnShowCreateTablePopup_Click(RoomData data)
    {
        var popup = (CreateTablePopup)_popups[PopupType.CreateTable];
        popup.Show(data, d => Event.OnCreateTableButton_Click?.Invoke(d));
    }

    private void OnFirstHiddenDiscardedCardsCollectedByUser(List<CardData> cards)
    {
        var popup = (FirstHiddenCardsPopup)_popups[PopupType.FirstHiddenCards];
        popup.Show(cards, () => Event.OnFirstHiddenDiscardedCardsPanelClosed?.Invoke());
    }
}

public enum UIPanelType
{
    Lobby,
    Loading,
    Table
}

public enum PopupType
{
    Profile,
    CreateTable,
    FirstHiddenCards,
    TableSessionResult,
    WarningYesNo,
}