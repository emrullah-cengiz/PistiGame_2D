public class TableData
{
    public RoomData RoomData { get; set; }
    public int BetAmount { get; set; }
    public TableMode Mode { get; set; }
}

public enum TableMode
{
    TwoPlayers = 2,
    FourPlayers = 4
}