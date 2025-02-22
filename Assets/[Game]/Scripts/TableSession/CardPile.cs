using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class CardPile
{
    [Inject] private readonly TableSessionSettings _tableSessionSettings;

    public CardPileView View { get; set; }
    public List<CardData> Cards { get; set; }
    public bool HasAnyCard => Cards.Count > 0;
    public CardData? LastAddedCard => HasAnyCard ? Cards[^1] : null;
    public CardData? PreviousAddedCard => Cards.Count > 1 ? Cards[^2] : null;

    public void Setup(CardPileView view) => View = view;

    public void SetCards(List<CardData> cards) => Cards = cards;

    public async UniTask AddCard(CardData card, CardView cardView, CardTransferOptions options)
    {
        Cards.Add(card);

        await View.AddCard(card, cardView, options);
    }

    public async UniTask TransferTo(CardPile otherPile, int number, CardTransferOptions options)
    {
        for (var i = 0; i < number; i++)
        {
            var card = Cards[^1];

            await TransferTo(otherPile, card, options);
        }
    }

    public async UniTask TransferTo(CardPile otherPile, CardData card, CardTransferOptions options)
    {
        Cards.Remove(card);
        var cardView = View.RemoveCard(card);

        otherPile.AddCard(card, cardView, options).Forget();

        if (options.IsSequential)
            await UniTask.WaitForSeconds(_tableSessionSettings.WaitDurationBetweenDealingCards);
    }

    public async UniTask TransferAllTo(CardPile pile, CardTransferOptions options) => await TransferTo(pile, Cards.Count, options);

    public int GetTotalPilePoints()
    {
        int total = 0;
        for (var i = 0; i < Cards.Count; i++)
            if (_tableSessionSettings.OrderedSpecialCardPoints.TryGetValue(Cards[i], out var point))
                total += point;
        
        return total;
    }

    public void LogPile(string pileName)
    {
#if UNITY_EDITOR
        Debug.Log($"<color=yellow>{pileName}:</color> {string.Join(",", Cards.Select(c => c.ToString()))}");
#endif
    }

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
}

public struct CardTransferOptions
{
    public static CardTransferOptions Default = new();

    public CardTransferOptions(bool isClosed = false, bool isSequential = true, bool despawnView = false)
    {
        IsClosed = isClosed;
        IsSequential = isSequential;
        DespawnView = despawnView;
    }

    public bool IsClosed;
    public bool IsSequential;
    public bool DespawnView;
}