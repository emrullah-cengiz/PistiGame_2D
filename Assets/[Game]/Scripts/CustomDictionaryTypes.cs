using System;
using UnityEngine;

[Serializable]
public class ScoreActionTypeIntDictionary : SerializableDictionary<ScoreActionType, int>
{
}

[Serializable]
public class CardDataSpriteDictionary : SerializableDictionary<CardData, Sprite>
{
}

[Serializable]
public class CardValueIntDictionary : SerializableDictionary<CardData, int>
{
}