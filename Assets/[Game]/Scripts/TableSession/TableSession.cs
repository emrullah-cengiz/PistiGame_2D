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
    private TableData Data;

    public UserPlayer UserPlayer;
    private TablePlayerBase[] _players;

    public CardPile DrawPile { get; private set; }
    public CardPile DiscardPile { get; private set; }

    [Inject] private readonly TableView View;
    [Inject] private readonly TableSessionSettings _tableSessionSettings;

    public void Setup(TableData data)
    {
        Data = data;

        CreatePlayers();

        PreparePiles();
    }

    private void CreatePlayers()
    {
        _players = new TablePlayerBase[(int)Data.Mode];

        _players[0] = UserPlayer = new UserPlayer();
        UserPlayer.Setup(View.UserPlayerView);

        for (int i = 1; i < (int)Data.Mode; i++)
        {
            _players[i] = new BotPlayer();
            _players[i].Setup(View.BotPlayerViews[i - 1]);
        }
    }

    private void PreparePiles()
    {
        DiscardPile = new CardPile();
        DrawPile = new CardPile();

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
        foreach (var player in _players)
            await DrawPile.TransferTo(player.Hand, _tableSessionSettings.HandLength, CardTransferOptions.Default);
    }

    public async UniTask ExecuteTurnLoopsUntilCardsExhausted()
    {
        do
            foreach (var player in _players)
            {
                if (!player.HasCard())
                    return;

                var card = await player.PlayCard();

                await HandleDiscardPileCollection(card, player);
            }
        while (_players[0].HasCard());
    }

    private async Task HandleDiscardPileCollection(CardData card, TablePlayerBase player)
    {
        var lastDiscardedCard = DiscardPile.LastAddedCard;

        if (lastDiscardedCard.HasValue && (card.IsJack || lastDiscardedCard.Value.Value == card.Value))
        {
            int totalPilePoints = DiscardPile.GetTotalPilePoints();

            //Collect discard pile
            await DiscardPile.TransferAllTo(player.CollectedPile, new(isSequential: false, despawnView: true));

            player.ScoreHandler.AddScoreByCard(totalPilePoints, lastDiscardedCard.Value, card);
        }
    }

    public TableSessionUserResult GetTableSessionUserResult()
    {
        TablePlayerBase winner = _players.OrderByDescending(x => x.ScoreHandler.CurrentScore)
                                         .FirstOrDefault();

        return new TableSessionUserResult()
        {
            IsWon = winner.IsUser,
            Score = UserPlayer.ScoreHandler.CurrentScore,
            EarnedMoney = winner.IsUser ? Data.BetAmount * _players.Length : 0
        };
    }
}

public class TableSessionUserResult
{
    public bool IsWon;
    public int Score;
    public int EarnedMoney;
}