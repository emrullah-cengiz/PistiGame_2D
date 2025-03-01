using System;
using System.Collections.Generic;
using UnityEngine;

public static class Event
{
    public static Action<GameState> OnGameStateChanged;

    public static Action OnGameLoadingStart;
    public static Action OnEnterLobby;
    public static Action OnTableSessionStart;
    public static Action<TableSessionResultData> OnTableSessionGameEnd;
    public static Action OnTableSessionGameExit;
    public static Action OnTableSessionEnd;
    
    public static Action OnPlayerDataLoaded;
    public static Action OnPlayerDataChanged;
    
    //UI Events
    public static Action<RoomData> OnShowCreateTablePopup_Click;
    public static Action<TableData> OnCreateTableButton_Click;
    
    public static Action OnFirstHiddenDiscardedCardsPanelClosed;
    
    public static Action OnBackToLobbyBtn_Click;
    public static Action OnBackToLobbyConfirmationBtn_Click;

    
    //Table Session
    public static Action<TableData> OnTableInitialized;
    public static Action<CardData> OnCardSelected;
    public static Action<TablePlayerData> OnTablePlayerDataChanged;
    public static Action<List<CardData>> OnFirstHiddenDiscardedCardsCollectedByUser;
    public static Action<int> OnDrawPileCardNUmberChanged;

}
