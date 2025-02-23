using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

public enum PileCardViewType
{
    Scattered,
    Hand
}

public class CardPileView : MonoBehaviour
{
    [Inject] private Pool<CardView> _pool;

    //Holds only viewable cards
    private Dictionary<CardData, CardView> _cardViews;

    private void Awake()
    {
        _cardViews = new();
    }

    public async UniTask AddCard(CardData data, CardView cardView, CardTransferOptions options)
    {
        if (!cardView)
            cardView = _pool.Spawn(data, options.IsClosed);

        _cardViews.Add(data, cardView);

        GetCardTransform(_cardViews.Count - 1, out var pos, out var rot);
        cardView.SetRotation(rot).Forget();
        await cardView.MoveTo(transform.position + pos);

        if (options.DespawnView)
            RemoveCard(data);
    }

    protected virtual void GetCardTransform(int index, out Vector3 position, out Quaternion rotation)
    {
        position = transform.position;
        rotation = Quaternion.identity;
    }

    public CardView RemoveCard(CardData card)
    {
        if (_cardViews.Remove(card, out var cardView))
            _pool.Despawn(cardView);

        return cardView;
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        _cardViews ??= new();
        
        int i = 0;
        foreach (var card in _cardViews.Values)
        {
            GetCardTransform(i++, out var pos, out var rot);
            card.SetRotation(rot).Forget();
            card.MoveTo(transform.position + pos);
        }
    }

#endif
}