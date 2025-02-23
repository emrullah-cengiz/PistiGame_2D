using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class HandPileView : CardPileView
{
    [SerializeField] private float _arcHeight = 2;
    [SerializeField] private float _totalWidth = 5f;
    [SerializeField] private float _maxAngle = 30f;
    
    int totalCards = 4;
    
    protected override void GetCardTransform(int index, out Vector3 position, out Quaternion rotation)
    {
        float spacing = _totalWidth / (totalCards - 1);
        float xPos = -_totalWidth / 2 + index * spacing;
        float curveFactor = 1 - Mathf.Pow((xPos / (_totalWidth / 2)), 2);
        float yPos = _arcHeight * curveFactor;

        position = new Vector3(xPos, yPos, 0);

        float angle = Mathf.Lerp(-_maxAngle, _maxAngle, (float)index / (totalCards - 1));
        rotation = Quaternion.Euler(0, 0, angle);
    }
}