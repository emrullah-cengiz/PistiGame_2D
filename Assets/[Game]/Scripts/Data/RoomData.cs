using System;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public class RoomData
{
    // public string Id = Guid.NewGuid().ToString();
    
    public string Name;

    [MinMaxSlider(0, 1000000)] public Vector2Int BetRange;

    public Color Color;
}