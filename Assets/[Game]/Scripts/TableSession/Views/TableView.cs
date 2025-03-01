using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

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
//
// #if UNITY_EDITOR
//
//     [Title("Editor")] [SerializeField] private CardView _cardPrefab;
//     [SerializeField] private GameSettings _settings;
//     private List<GameObject> _tempCards;
//
//     [Button]
//     private void FillCards()
//     {
//         foreach (var card in _tempCards) 
//             DestroyImmediate(card);
//         
//         _tempCards.Clear();
//         
//         int i = 0;
//
//         CreateCard(UserPlayerView);
//         foreach (var playerView in _BotPlayerViews) 
//             CreateCard(playerView);
//
//         return;
//
//         void CreateCard(TablePlayerView playerView)
//         {
//             var obj = (CardView)PrefabUtility.InstantiatePrefab(_cardPrefab, playerView.HandPileView.transform);
//
//             var data = new CardData((CardType)Random.Range(0, 4), (CardValue)Random.Range(0, 13));
//             obj.GetComponent<SpriteRenderer>().sprite = _settings.CardSettings.CardDataSprites[data];
//             
//             (obj.transform.position, obj.transform.localEulerAngles, obj.transform.localScale) = 
//                 playerView.HandPileView.CalculateCardTransform(i++);
//
//             _tempCards.Add(obj.gameObject);
//         }
//     }
//
// #endif
}