using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO: Find a better solution!
/// </summary>

public static class Event
{
    public static Action<GameState> OnGameStateChanged;

    public static Action OnGameLoadingStart;
    public static Action OnEnterLobby;
    public static Action OnTableSessionStart;
    public static Action<TableSessionUserResultData> OnTableSessionEnd;
    
    public static Action OnPlayerDataLoaded;
    public static Action OnPlayerDataChanged;
    
    //UI Events
    public static Action<TableData> OnTableCreateButton_Click;
    public static Action OnFirstClosedCardsPanelCloseBtn_Click;
    public static Action<List<CardData>> OnFirstClosedCardsCollectedByUser;
    
    //Table Session
    public static Action<CardData> OnCardSelected;

}
