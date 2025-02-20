using System;
using UnityEngine;

/// <summary>
/// TODO: Find a better solution!
/// </summary>

public static class Event
{
    public static Action<GameState> OnGameStateChanged = delegate { };

    public static Action OnGameLoadingStart = delegate { };
    public static Action OnEnterLobby = delegate { };
    public static Action OnTableSessionStart = delegate { };
    public static Action OnTableSessionEnd = delegate { };
    
    public static Action OnPlayerDataLoaded = delegate { };
    public static Action OnPlayerDataChanged = delegate { };

}
