using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

public class CardPileView : MonoBehaviour
{
    [Inject] private Pool<CardView> _pool;
    [SerializeField] public float CardScale = 0.225f;

    //Holds only viewable cards
    private Dictionary<CardData, CardView> _cardViews;

    public void Initialize()
    {
        _cardViews ??= new();

        Clear();
    }

    public async UniTask AddCard(CardData data, CardView cardView, CardTransferOptions options)
    {
        if (!cardView)
            cardView = _pool.Spawn(data, options.IsClosed);

        _cardViews.Add(data, cardView);

        var targetTransform = CalculateCardTransform(_cardViews.Count - 1);
        cardView.transform.SetParent(transform, options.WorldPositionStaysOnStart);
        cardView.transform.SetAsLastSibling();

        cardView.SetTransform(options.InitialWorldTransform);

        await cardView.MoveTo(targetTransform, isClosed: options.IsClosed, options.MoveDuration);

        if (options.DespawnViewAfterCompletion)
            RemoveCard(data, despawn: true);
    }

    protected virtual (Vector3 pos, Vector3 angles, Vector3 scale) CalculateCardTransform(int index) => (Vector3.zero, Vector3.zero, Vector3.one * CardScale);

    public CardView RemoveCard(CardData card, bool despawn = false)
    {
        _cardViews.Remove(card, out var cardView);

        if (despawn)
            _pool.Despawn(cardView);
        return cardView;
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        var tempCardViews = GetComponentsInChildren<CardView>();

        int i = 0;
        foreach (var card in tempCardViews)
            (card.transform.position, 
             card.transform.localEulerAngles, 
             card.transform.localScale) = CalculateCardTransform(i++);
    }

#endif

    public void SetCardsInteractable(bool b)
    {
        foreach (var cardView in _cardViews.Values)
            cardView.SetInteractable(b);
    }

    public void Clear()
    {
        var cardsToRemove = new List<CardData>(_cardViews.Keys);
        foreach (var card in cardsToRemove)
            RemoveCard(card, despawn: true);
    }
}