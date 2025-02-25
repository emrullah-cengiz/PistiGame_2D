using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public struct PlayerData
{
    public string Name;
    public int AccountBalance;
    public int WinCount;
    public int LostCount;
}


[Serializable]
public struct TablePlayerData
{
    public int PlayerIndex;
    public string Name;
    public int Score;
}