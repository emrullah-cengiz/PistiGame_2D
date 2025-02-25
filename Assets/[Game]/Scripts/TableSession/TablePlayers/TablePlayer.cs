using System;
using System.Collections.Generic;
using System.Linq;
using MBehaviourTree;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public abstract class TablePlayer
{
    [Inject] protected readonly TableSession _tableSession;
    [Inject] protected readonly IObjectResolver _objectResolver;

    public TablePlayerData TablePlayerData;
    public TablePlayerView View { get; private set; }
    public CardPile Hand { get; private set; }
    public CardPile CollectedPile { get; private set; }

    public virtual bool IsUser => false;

    public virtual void Setup(int playerIndex, TablePlayerView view)
    {
        View = view;

        Hand = new CardPile();
        _objectResolver.Inject(Hand);
        Hand.Setup(View.HandPileView);

        CollectedPile = new CardPile();
        _objectResolver.Inject(CollectedPile);
        CollectedPile.Setup(View.CollectedPileView);
    }

    public bool HasCard() => Hand.Cards.Count > 0;

    public abstract UniTask<CardData> PlayCard();

    public void AddScore(int score)
    {
        TablePlayerData.Score += score;
        Event.OnTablePlayerDataChanged?.Invoke(TablePlayerData);
    }

    public void SetTurn(bool s) => View.SetTurn(s);

}