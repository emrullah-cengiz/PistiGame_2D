using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerData
{
    public string Name = "Player";
    public int AccountBalance = 0;
    public int WinCount = 0;
    public int LostCount = 0;
}

public class PlayerViewData
{
    public readonly string Name;
    public readonly int CurrentMoney;
    public readonly int WinCount;
    public readonly int LostCount;

    public PlayerViewData(string name, int currentMoney, int winCount, int lostCount)
    {
        Name = name;
        CurrentMoney = currentMoney;
        WinCount = winCount;
        LostCount = lostCount;
    }
}