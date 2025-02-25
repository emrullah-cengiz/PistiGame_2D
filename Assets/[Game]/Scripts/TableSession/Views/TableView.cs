using System;
using TMPro;
using UnityEngine;

public class TableView : MonoBehaviour
{
    public TablePlayerView UserPlayerView;
    public TablePlayerView[] _BotPlayerViews;
    public TablePlayerView[] ActiveBotViews;

    public CardPileView DrawPileView;
    public CardPileView DiscardPileView;
    
    [SerializeField] private TMP_Text _drawPileCardCount;

    private void OnEnable() => Event.OnDrawPileCardNUmberChanged += OnDrawPileCardNUmberChanged;
    private void OnDisable() => Event.OnDrawPileCardNUmberChanged -= OnDrawPileCardNUmberChanged;

    public void Initialize(TableData data)
    {
        gameObject.SetActive(true);

        for (var i = 0; i < _BotPlayerViews.Length; i++) 
            _BotPlayerViews[i].Activate(false);

        ActiveBotViews = data.Mode == TableMode.TwoPlayers ? new[] { _BotPlayerViews[1] } : _BotPlayerViews;
        for (var i = 0; i < ActiveBotViews.Length; i++) 
            ActiveBotViews[i].Activate(true);
    }

    private void OnDrawPileCardNUmberChanged(int discardPileCount) => 
        _drawPileCardCount.text = discardPileCount.ToString();
}