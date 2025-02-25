using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class FirstHiddenCardsPopup : InfoPopup
{
    [Inject] private CardSettings _cardSettings;
    [SerializeField] private List<Image> _cards;

    public void Show(List<CardData> cards, Action callback = null)
    {
        base.Show(callback);

        int i = 0;
        foreach (var img in _cards) 
            img.sprite = _cardSettings.CardDataSprites[cards[i++]];
    }
}