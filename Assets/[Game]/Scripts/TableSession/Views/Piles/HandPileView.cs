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
    
    [SerializeField] private float _pingPongFrequency = 0.5f;
    [SerializeField] private float _pingPongAmplitude = 0.2f;

    [Inject] private TableSessionSettings _settings;

    private int handLength;

    [Inject]
    private void OnInject() => handLength = _settings.HandLength;

    protected override (Vector3 pos, Vector3 angles, Vector3 scale) CalculateCardTransform(int index)
    {
        float spacing = _totalWidth / (handLength - 1);
        float xPos = -_totalWidth / 2 + index * spacing;
        float curveFactor = 1 - Mathf.Pow((xPos / (_totalWidth / 2)), 2);
        float yPos = _arcHeight * curveFactor;
        
        float pingPongOffset = Mathf.PingPong(index * _pingPongFrequency, _pingPongAmplitude);
        yPos += pingPongOffset; 

        float angle = Mathf.Lerp(-_maxAngle, _maxAngle, (float)index / (handLength - 1));

        return (new Vector3(xPos, yPos, 0), new Vector3(0, 0, angle), Vector3.one * CardScale);
    }
}