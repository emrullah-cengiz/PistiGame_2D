using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

public class ScatterPileView : CardPileView
{
    [SerializeField] private float _scatterRange = 1f;
    [SerializeField] private float _maxScatterAngle = 30f;

    protected  override void GetCardTransform(int index, out Vector3 position, out Quaternion rotation)
    {
        float xOffset = Random.Range(-_scatterRange, _scatterRange);
        float yOffset = Random.Range(-_scatterRange, _scatterRange);

        position = new Vector3(xOffset, yOffset, 0);

        float angleOffset = Mathf.PingPong(index * 10f, _maxScatterAngle * 2) - _maxScatterAngle;
        rotation = Quaternion.Euler(0, 0, angleOffset);
    }
}