using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public class RoomData
{
    public string Name;
    
    [MinMaxSlider(0, 1000000)] 
    public Vector2Int BetRange;
}