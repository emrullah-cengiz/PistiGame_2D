using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

public class TableSession
{
    [Inject] public readonly TableView View;
    [Inject] public ScoreHandler ScoreHandler { get; private set; }
    [Inject] private readonly TableSessionSettings _tableSessionSettings;
    [Inject] private Pool<CardView> _pool;
    [Inject] private CardSettings _cardSettings;
    [Inject] private IObjectResolver _objectResolver;

    public CardPile DrawPile { get; private set; }
    public CardPile DiscardPile { get; private set; }

    public UserPlayer UserPlayer;
    public TablePlayer[] Players;

    private int _nextCardSortingOrder;
    public int NextCardSortingOrder => _nextCardSortingOrder++;

    private TableData Data;
    private bool _isFirstHiddenDiscardedCardsCollected;
    private List<CardData> _firstHiddenDiscardedCards;

    private UniTaskCompletionSource _showFirstHiddenDiscardedCardsTask;
    private CancellationTokenSource _cancellationTokenSource;

    public void Setup(TableData data)
    {
        Debug.Log("TableSession_Setup");

        Data = data;

        View.Initialize(data);

        _pool.Initialize(new Pool<CardView>.PoolProperties()
        {
            ExpansionSize = 5,
            FillOnInit = true,
            Prefab = _cardSettings.CardPrefab,
        });

        ScoreHandler.Setup(this);

        Event.OnTableInitialized?.Invoke(data);

        CreatePlayers();

        PreparePiles();

        Event.OnBackToLobbyConfirmationBtn_Click += OnBackToLobbyConfirmationBtnClick;
    }

    private void CreatePlayers()
    {
        Players = new TablePlayer[Data.Mode == TableMode.TwoPlayers ? 2 : 4];

        UserPlayer = (UserPlayer)(Players[0] = SetupPlayer(0, new UserPlayer(), View.UserPlayerView));

        for (var i = 0; i < View.ActiveBotViews.Length; i++)
            Players[1 + i] = SetupPlayer(i + 1, new BotPlayer(), View.ActiveBotViews[i]);
    }

    private TablePlayer SetupPlayer(int index, TablePlayer player, TablePlayerView playerView)
    {
        _objectResolver.Inject(player);
        player.Setup(index, playerView);
        return player;
    }

    private void PreparePiles()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        DiscardPile = new CardPile();
        _objectResolver.Inject(DiscardPile);
        DiscardPile.Setup(View.DiscardPileView);

        DrawPile = new CardPile();
        _objectResolver.Inject(DrawPile);
        DrawPile.Setup(View.DrawPileView);

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
        _firstHiddenDiscardedCards = DrawPile.Cards.Take(3).ToList();

        await DrawPile.TransferTo(DiscardPile, 3, new(true));
        await DrawPile.TransferTo(DiscardPile, 1, CardTransferOptions.Default);
    }

    public async UniTask DealCardsToPlayers()
    {
        for (var i = 0; i < _tableSessionSettings.HandLength; i++)
        {
            foreach (var player in Players)
            {
                var options = new CardTransferOptions(
                    isClosed: !player.IsUser,
                    waitForCompletion: false,
                    isSequential: false,
                    worldPositionStaysOnStart: true,
                    initialWorldTransform: new(
                        DrawPile.View.transform.position, Vector3.zero, DrawPile.View.transform.localScale
                    )
                );

                DrawPile.TransferTo(player.Hand, 1, options).Forget();
                await UniTask.WaitForSeconds(_tableSessionSettings.GeneralDelayBetweenCardsOnSequentialMoves,
                                             cancellationToken: _cancellationTokenSource.Token);
            }

            Event.OnDrawPileCardNUmberChanged?.Invoke(DrawPile.Cards.Count);
        }
    }

    public async UniTask ProcessTurnLoopsUntilHandsEmpty()
    {
        while (Players[0].HasCard())
        {
            foreach (var player in Players)
            {
                if (!player.HasCard())
                    continue;

                player.SetTurn(true);

                var card = await player.PlayCard();
                //patch, some times can not wait the tween.. 
                await UniTask.WaitForSeconds(_cardSettings.GeneralMoveDuration, cancellationToken: _cancellationTokenSource.Token);

                player.SetTurn(false);

                if (!CanTheDiscardPileBeCollected(out ScoreActionType? collectionType))
                    continue;

#if UNITY_EDITOR
                DiscardPile.LogPile("DISCARD_PILE_ON_COLLECT-" + player.TablePlayerData.Name);
                player.View.LogCollection(DiscardPile.Cards);
#endif

                await CollectDiscardPile(player, collectionType);

                await HandleFirstHiddenCardsShowing(player);
            }
        }

        return;

        bool CanTheDiscardPileBeCollected(out ScoreActionType? collectionType)
        {
            collectionType = null;

            if (DiscardPile.Cards.Count < 2)
                return false;

            if (!DiscardPile.LastButOneCard.HasValue ||
                (DiscardPile.LastButOneCard.Value.Value !=
                 DiscardPile.LastAddedCard!.Value.Value &&
                 DiscardPile.LastAddedCard!.Value.Value != CardValue.Jack))
                return false;

            //It's just collection with jack
            if (DiscardPile.LastButOneCard.Value.Value !=
                DiscardPile.LastAddedCard!.Value.Value) return true;

            //It's Pisti
            collectionType = ScoreActionType.Pisti;

            //It's double Pisti
            if (DiscardPile.LastAddedCard!.Value.IsJack)
                collectionType = ScoreActionType.JackPisti;

            return true;
        }

        async UniTask HandleFirstHiddenCardsShowing(TablePlayer player)
        {
            if (_isFirstHiddenDiscardedCardsCollected)
                return;

            _isFirstHiddenDiscardedCardsCollected = true;

            if (!player.IsUser)
                return;

            await ShowFirstHiddenDiscardedCardsToUser(_firstHiddenDiscardedCards);
        }
    }

    private async UniTask CollectDiscardPile(TablePlayer player, ScoreActionType? scoreAction)
    {
        int totalPilePoints = DiscardPile.GetTotalPilePoints();

        //Collect discard pile
        await DiscardPile.TransferAllTo(player.CollectedPile, new(isSequential: false, waitForCompletion: true, despawnView: true));

        ScoreHandler.AddScoreToPlayerByDiscardedCard(totalPilePoints, player, scoreAction);
    }

    private async Task ShowFirstHiddenDiscardedCardsToUser(List<CardData> cards)
    {
        Event.OnFirstHiddenDiscardedCardsCollectedByUser?.Invoke(cards);
        Debug.Log("<color=yellow>First closed cards collected by user, waiting for click ok button...</color>");

        _showFirstHiddenDiscardedCardsTask = new UniTaskCompletionSource();
        Event.OnFirstHiddenDiscardedCardsPanelClosed += OnFirstHiddenDiscardedCardsPanelClosed;

        await _showFirstHiddenDiscardedCardsTask.Task;
        return;

        void OnFirstHiddenDiscardedCardsPanelClosed()
        {
            _showFirstHiddenDiscardedCardsTask.TrySetResult();
            Event.OnFirstHiddenDiscardedCardsPanelClosed -= OnFirstHiddenDiscardedCardsPanelClosed;

            Debug.Log("<color=yellow>First closed cards popup closed, continuing...</color>");
        }
    }

    public TableSessionResultData EndGame()
    {
        var winner = ScoreHandler.GetWinner();

        ScoreHandler.AddScoreByCollectedCardsMajority(winner);

        return new TableSessionResultData()
        {
            IsWon = winner.IsUser,
            Score = UserPlayer.TablePlayerData.Score,
            EarnedMoney = winner.IsUser ? Data.BetAmount * Players.Length : 0
        };
    }

    private void OnBackToLobbyConfirmationBtnClick()
    {
        _cancellationTokenSource.Cancel();
        Event.OnBackToLobbyConfirmationBtn_Click -= OnBackToLobbyConfirmationBtnClick;
    }
}