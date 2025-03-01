using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class CardPile
{
    [Inject] private readonly TableSessionSettings _tableSessionSettings;
    [Inject] private readonly CardSettings _cardSettings;
    
    [SerializeField] protected PileType _pileType;

    public CardPileView View { get; set; }
    public List<CardData> Cards { get; set; }
    public bool HasAnyCard => Cards.Count > 0;
    public CardData? LastAddedCard => HasAnyCard ? Cards[^1] : null;
    public CardData? LastButOneCard => Cards.Count > 1 ? Cards[^2] : null;

    public void Setup(CardPileView view)
    {
        Cards = new List<CardData>();
        View = view;
        View.Initialize();
    }

    public void SetCards(List<CardData> cards) => Cards = cards;

    public async UniTask AddCard(CardData card, CardView cardView, CardTransferOptions options)
    {
        Cards.Add(card);

        await View.AddCard(card, cardView, options);
    }

    public async UniTask TransferTo(CardPile otherPile, CardData card, CardTransferOptions options)
    {
        Cards.Remove(card);
        var cardView = View.RemoveCard(card);

        var moveTask = otherPile.AddCard(card, cardView, options);

        if (options.WaitForCompletion)
            await moveTask;
        else
            moveTask.Forget();
    }

    public async UniTask TransferTo(CardPile otherPile, int number, CardTransferOptions options)
    {
        bool waitForCompletion = options.WaitForCompletion;
        for (var i = 0; i < number; i++)
        {
            options.WaitForCompletion = options.IsSequential;
            await TransferTo(otherPile, Cards[^1], options);
        }

        //wait 1 delay if not sequential
        if (!options.IsSequential && waitForCompletion)
            await UniTask.WaitForSeconds(options.MoveDuration ?? _cardSettings.GeneralMoveDuration);
    }

    public async UniTask TransferAllTo(CardPile pile, CardTransferOptions options) => await TransferTo(pile, Cards.Count, options);

    #region Card Queries

    public bool HasAnyJack(out CardData? card) => HasAny(CardValue.Jack, out card);

    public bool HasAny(CardValue cardValue, out CardData? card)
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            card = Cards[i];
            if (card.Value.Value == cardValue)
                return true;
        }

        card = null;
        return false;
    }

    public bool Contains(CardData cardData) => Cards.Exists(card => card.Equals(cardData));

    public CardData GetLowestSpecialOrAnyNonSpecialCard()
    {
        var specialCardPoints = _tableSessionSettings.OrderedSpecialCardPoints;

        CardData? lowestSpecial = null;
        CardData? anyNonSpecialCard = null;

        foreach (var handCard in Cards)
            if (specialCardPoints.TryGetValue(handCard, out var point))
            {
                if (!lowestSpecial.HasValue || specialCardPoints[lowestSpecial.Value] > point)
                    lowestSpecial = handCard;
            }
            else if (!anyNonSpecialCard.HasValue)
                anyNonSpecialCard = handCard;

        return anyNonSpecialCard ?? lowestSpecial!.Value;
    }

    #endregion

    public int GetTotalPilePoints()
    {
        int total = 0;
        for (var i = 0; i < Cards.Count; i++)
            if (_tableSessionSettings.OrderedSpecialCardPoints.TryGetValue(Cards[i], out var point))
                total += point;

        return total;
    }

    public void SetCardsInteractable(bool s) => View.SetCardsInteractable(s);

    public void LogPile(string message)
    {
#if UNITY_EDITOR
        Debug.Log($"<color=yellow>{message}:</color> {string.Join(",", Cards.Select(c => c.ToString()))}");
#endif
    }
}

public struct CardTransferOptions
{
    public static CardTransferOptions Default = new(
        isClosed: false,
        isSequential: true,
        despawnView: false,
        waitForCompletion: true,
        moveDuration: null,
        worldPositionStaysOnStart: true,
        initialWorldTransform: null
    );

    public CardTransferOptions(bool isClosed = false, bool isSequential = true, bool despawnView = false, bool waitForCompletion = true,
                               float? moveDuration = null, bool worldPositionStaysOnStart = true,
                               (Vector3? pos, Vector3? angles, Vector3? scale)? initialWorldTransform = null)
    {
        IsClosed = isClosed;
        IsSequential = isSequential;
        DespawnViewAfterCompletion = despawnView;
        WaitForCompletion = waitForCompletion;
        MoveDuration = moveDuration;
        WorldPositionStaysOnStart = worldPositionStaysOnStart;
        InitialWorldTransform = initialWorldTransform;
    }

    public bool IsClosed;
    public bool IsSequential;
    public bool WaitForCompletion;
    public bool DespawnViewAfterCompletion;
    public bool WorldPositionStaysOnStart;
    public float? MoveDuration;
    public (Vector3? pos, Vector3? angles, Vector3? scale)? InitialWorldTransform;
}