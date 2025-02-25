using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;
using UnityEditor;

[CreateAssetMenu(fileName = nameof(GameSettings), menuName = "Settings/" + nameof(GameSettings), order = 0)]
public class GameSettings : ScriptableObject
{
    public PlayerSettings PlayerSettings;
    [Space] public LobbySettings LobbySettings;
    [Space] public TableSessionSettings TableSessionSettings;
    [Space] public CardSettings CardSettings;
}

[Serializable]
public class PlayerSettings
{
    public int PlayerStartMoney = 10000;
}

[Serializable]
public class LobbySettings
{
    public List<RoomData> Rooms = new List<RoomData>();
}

[Serializable]
public class TableSessionSettings
{
    public ScoreActionTypeIntDictionary ScoreActionPoints;
    
    [Space] 
    public CardValueIntDictionary OrderedSpecialCardPoints;

    [Space]
    public int HandLength = 4;

    public float WaitDurationBeforeDealingCards = .3f;
    public float GeneralDelayBetweenCardsOnSequentialMoves = .2f;
    public float BotWaitDurationBeforePlay = .2f;
    public float WaitDurationBeforePlayLoopStart = .5f;
    public float WaitDurationBeforeResultPopup = .3f;
}

[Serializable]
public class CardSettings
{
    public float GeneralMoveDuration = .25f;
    public CardView CardPrefab;
    public Sprite ClosedCardSprite;
    
    [Space] public CardDataSpriteDictionary CardDataSprites;
    
    [Button(nameof(GenerateMissingKeyCombinations))]
    void GenerateMissingKeyCombinations()
    {
        CardData missingKey;
        foreach (var cardType in (CardType[])Enum.GetValues(typeof(CardType)))
        foreach (var cardValue in (CardValue[])Enum.GetValues(typeof(CardValue)))
            if (!CardDataSprites.ContainsKey(missingKey = new CardData(cardType, cardValue)))
                CardDataSprites.Add(missingKey, null);
    }
}


