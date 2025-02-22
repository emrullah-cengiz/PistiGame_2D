using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class CardPileView : MonoBehaviour
{
    //Holds only viewable cards
    private Dictionary<CardData, CardView> _cardViews;

    [Inject] private Pool<CardView> _pool;

    public async UniTask AddCard(CardData data, CardView cardView, CardTransferOptions options)
    {
        if (!cardView)
            cardView = _pool.Spawn(data);
        
        _cardViews.Add(data, cardView);

        await cardView.MoveTo(transform.position);

        if (options.DespawnView) 
            RemoveCard(data);
    }

    public CardView RemoveCard(CardData card)
    {
        _cardViews.Remove(card, out var cardView);
        _pool.Despawn(cardView);

        return cardView;
    }
}