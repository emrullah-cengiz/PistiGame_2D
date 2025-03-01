using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public class RoomData
{
    public string Name;

    [MinMaxSlider(0, 100000, showFields: true)]
    public Vector2Int BetRange;
}