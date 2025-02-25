using System;

public static class Helpers
{
    public static char ToIconChar(this CardType type) =>
        type switch
        {
            CardType.Hearts => '\u2665',
            CardType.Diamonds => '\u2666',
            CardType.Clubs => '\u2663',
            CardType.Spades => '\u2660',
        };
}