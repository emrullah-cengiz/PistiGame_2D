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
public class CardSettings
{
    public CardView CardPrefab;
    public Sprite ClosedCardSprite;
    
    [Space] public CardValueIntDictionary OrderedSpecialCardPoints;
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
    public ScoreActionTypeIntDictionary ScoreActions;

    [Space]
    public int HandLength = 4;

    public float WaitDurationBeforeStartDiscards = .3f;

    public float WaitDurationBeforeCardDealing = .3f;
    public float WaitDurationBetweenDealingCards = .2f;

    public float WaitDurationBeforePlayLoopStart = .5f;
}

[Serializable]
public class ScoreActionTypeIntDictionary : SerializableDictionary<ScoreActionType, int>
{
}

[Serializable]
public class CardDataSpriteDictionary : SerializableDictionary<CardData, Sprite>
{
}

[Serializable]
public class CardValueIntDictionary : SerializableDictionary<CardData, int>
{
}

public enum ScoreActionType { JackPisti, Pisti, CollectedNumberMajority, EqualCardNumberToWinner }