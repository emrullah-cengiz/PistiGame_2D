using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct CardData : IPoolableInitializationData, IEquatable<CardData>
{
    public CardType Type;
    public CardValue Value;

    public CardData(CardType type, CardValue value)
    {
        Type = type;
        Value = value;
    }

    public bool CheckCard(CardValue val, CardType type) => Value == val && Type == type;
    public bool IsJack => Value == CardValue.Jack;
    public override string ToString() => $"{Type.ToIconChar()} {Value}";


    public override bool Equals(object obj) => obj is CardData other && Equals(other);
    public bool Equals(CardData other) => Type == other.Type && Value == other.Value;
    public override int GetHashCode() => HashCode.Combine(Type, Value);
}
