using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;

[CreateAssetMenu(fileName = nameof(GameSettings), menuName = "Settings/" + nameof(GameSettings), order = 0)]
public class GameSettings : SerializedScriptableObject
{
    [OdinSerialize, NonSerialized] public PlayerSettings PlayerSettings;
    [Space, OdinSerialize, NonSerialized] public LobbySettings LobbySettings;
    [Space, OdinSerialize, NonSerialized] public TableSessionSettings TableSessionSettings;
    [Space, OdinSerialize, NonSerialized] public CardSettings CardSettings;
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
    public Dictionary<ScoreActionType, int> ScoreActionPoints;

    [Space, OdinSerialize, NonSerialized] public Dictionary<CardData, int> OrderedSpecialCardPoints;

    [Space] public int HandLength = 4;

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

    [Space] public Dictionary<CardData, Sprite> CardDataSprites;

    [Space, FolderPath, SerializeField] private string CardSpritesPath;

    [Button(nameof(ReadCardSpritesFromFolder))]
    private void ReadCardSpritesFromFolder()
    {
        (CardDataSprites ??= new()).Clear();

        foreach (var cardType in (CardType[])Enum.GetValues(typeof(CardType)))
        foreach (var cardValue in (CardValue[])Enum.GetValues(typeof(CardValue)))
        {
            var cardTypeChar = cardType switch
            {
                CardType.Diamonds => 'd',
                CardType.Spades => 's',
                CardType.Hearts => 'h',
                CardType.Clubs => 'c',
                _ => '\0'
            };

            string path = Path.Combine(CardSpritesPath, cardType.ToString(), $"{(int)cardValue}{cardTypeChar}.png");

            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

            CardDataSprites.Add(new(cardType, cardValue), sprite);
        }
    }

    // [NaughtyAttributes.Button(nameof(GenerateMissingKeyCombinations))]
    // void GenerateMissingKeyCombinations()
    // {
    //     CardData missingKey;
    //     foreach (var cardType in (CardType[])Enum.GetValues(typeof(CardType)))
    //     foreach (var cardValue in (CardValue[])Enum.GetValues(typeof(CardValue)))
    //         if (!CardDataSprites.ContainsKey(missingKey = new CardData(cardType, cardValue)))
    //             CardDataSprites.Add(missingKey, null);
    // }
}