using System;
using UnityEngine;

[Serializable]
public class PanelTypeUIPanelDictionary : SerializableDictionary<UIPanelType, UIPanel>
{
}

[Serializable]
public class PopupTypePopupDictionary : SerializableDictionary<PopupType, PopUpBase>
{
}

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