using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

public class TableSession
{
    [Inject] private readonly TableView View;
    [Inject] private readonly TableSessionSettings _tableSessionSettings;
    [Inject] public ScoreHandler ScoreHandler { get; private set; }
    [Inject] private Pool<CardView> _pool;
    [Inject] private CardSettings _cardSettings;
    [Inject] private IObjectResolver _objectResolver;

    private int _nextCardSortingOrder;
    public int NextCardSortingOrder => _nextCardSortingOrder++;

    public CardPile DrawPile { get; private set; }
    public CardPile DiscardPile { get; private set; }

    public UserPlayer UserPlayer;
    public TablePlayer[] Players;

    private TableData Data;
    private bool _isFirstClosedCardsCollected;
    private UniTaskCompletionSource _showFirstClosedCardsTask;

    public void Setup(TableData data)
    {
        Debug.Log("----TableSession_Setup -> Pool_Setup");

        _pool.Initialize(new Pool<CardView>.PoolProperties()
        {
            ExpansionSize = 5,
            FillOnInit = true,
            Prefab = _cardSettings.CardPrefab,
        });

        Data = data;

        CreatePlayers();

        PreparePiles();
    }

    private void CreatePlayers()
    {
        int playerCount = (int)Data.Mode;
        Players = new TablePlayer[playerCount];

        Players[0] = UserPlayer = new UserPlayer();
        UserPlayer.Setup(View.UserPlayerView);

        for (int i = 1; i < playerCount; i++)
        {
            Players[i] = new BotPlayer();

            var botViewIndex = (Data.Mode == TableMode.TwoPlayers) ? 2 : (i - 1);
            if (botViewIndex < View.BotPlayerViews.Length) 
                Players[i].Setup(View.BotPlayerViews[botViewIndex]);
        }
    }

    private void PreparePiles()
    {
        DiscardPile = new CardPile();
        DiscardPile.Setup(View.DiscardPileView);
        _objectResolver.Inject(DiscardPile);
        
        DrawPile = new CardPile();
        DrawPile.Setup(View.DrawPileView);
        _objectResolver.Inject(DrawPile);

        var cards = new List<CardData>();

        foreach (var cardType in (CardType[])Enum.GetValues(typeof(CardType)))
        foreach (var cardValue in (CardValue[])Enum.GetValues(typeof(CardValue)))
            cards.Add(new CardData(cardType, cardValue));

        cards.Shuffle();

        DrawPile.SetCards(cards);

        //log
        DrawPile.LogPile("DRAW_PILE");
    }

    public async UniTask DiscardFirstCards()
    {
        await DrawPile.TransferTo(DiscardPile, 3, new(true));
        await DrawPile.TransferTo(DiscardPile, 1, CardTransferOptions.Default);
    }

    public async UniTask DealCardsToPlayers()
    {
        foreach (var player in Players)
        {
            await DrawPile.TransferTo(player.Hand, _tableSessionSettings.HandLength, CardTransferOptions.Default);
            await UniTask.WaitForSeconds(_tableSessionSettings.WaitDurationBetweenDealingCards);
        }
    }

    public async UniTask ProcessTurnLoopsUntilCardsExhausted()
    {
        do
            foreach (var player in Players)
            {
                if (!player.HasCard())
                    return;

                var card = await player.PlayCard();

                var isCollected = await HandleDiscardPileCollection(card, player);

                if (!_isFirstClosedCardsCollected && isCollected && player.IsUser)
                {
                    _isFirstClosedCardsCollected = true;
                    await ShowFirstClosedCardsToUser(DiscardPile.Cards.GetRange(0, 3));
                }
            }
        while (Players[0].HasCard());
    }

    private async UniTask<bool> HandleDiscardPileCollection(CardData card, TablePlayer player)
    {
        var lastDiscardedCard = DiscardPile.LastAddedCard;

        if (!DiscardPile.HasAnyCard || (!card.IsJack && lastDiscardedCard!.Value.Value != card.Value))
            return false;

        int totalPilePoints = DiscardPile.GetTotalPilePoints();

        //Collect discard pile
        await DiscardPile.TransferAllTo(player.CollectedPile, new(isSequential: false, despawnView: true));

        ScoreHandler.AddScoreToPlayerByDiscardedCard(totalPilePoints, player, lastDiscardedCard.Value, card);

        return true;
    }

    private async Task ShowFirstClosedCardsToUser(List<CardData> cards)
    {
        Event.OnFirstClosedCardsCollectedByUser?.Invoke(cards);
        
        _showFirstClosedCardsTask = new UniTaskCompletionSource();
        
        Event.OnFirstClosedCardsPanelCloseBtn_Click += OnFirstClosedCardsPanelClosed; 
        
        await _showFirstClosedCardsTask.Task;
    }

    private void OnFirstClosedCardsPanelClosed()
    {
        _showFirstClosedCardsTask.TrySetResult();
        Event.OnFirstClosedCardsPanelCloseBtn_Click -= OnFirstClosedCardsPanelClosed;
    }

    public TableSessionUserResultData EndGame()
    {
        var winner = ScoreHandler.GetWinner();

        ScoreHandler.AddScoreByCollectedCardsMajority(winner);

        if (winner.IsUser)
            return new TableSessionUserResultData()
            {
                IsWon = true,
                Score = UserPlayer.CurrentScore,
                EarnedMoney = Data.BetAmount * Players.Length
            };

        return new TableSessionUserResultData();
    }
}