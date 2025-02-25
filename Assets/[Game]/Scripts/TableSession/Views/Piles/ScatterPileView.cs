using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

public class ScatterPileView : CardPileView
{
    [SerializeField] private float _scatterRange = 1f;
    [SerializeField] private float _angleOffset = 30f;
    [SerializeField] private int _maxLength = 8;

    protected override (Vector3 pos, Vector3 angles) CalculateCardTransform(int index)
    {
        float xOffset = Random.Range(-_scatterRange, _scatterRange);
        float yOffset = Random.Range(-_scatterRange, _scatterRange);

        float angleOffset = Mathf.PingPong(index, _maxLength * 2) * _angleOffset;

        return (new Vector3(xOffset, yOffset, 0), Vector3.forward * angleOffset);
    }
}