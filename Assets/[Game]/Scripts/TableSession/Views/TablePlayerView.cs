using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TablePlayerView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _turnHighlightSprite;
    public CardPileView HandPileView;
    public CardPileView CollectedPileView;

    public void Activate(bool s) => gameObject.SetActive(s);

    public void SetTurn(bool s) => _turnHighlightSprite.TWFade(s?1:0, .15f).Forget();
    

    //Log
    [SerializeField, Multiline] private string _collectionLogs;
    public void LogCollection(List<CardData> cards) => _collectionLogs += $"{Environment.NewLine}" +
                                                                        $"{string.Join(",", cards.Select(c => c.ToString()).ToList())}";
}