using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameSettings), menuName = "Settings/" + nameof(GameSettings), order = 0)]
public class GameSettings : ScriptableObject
{
    [Header("Player Settings")]
    public int PlayerStartMoney;
    
    [Header("Room Settings")]
    public List<RoomData> Rooms = new List<RoomData>();
}
