using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ScoreActionTypeIntDictionary))]
public class ScoreActionTypeIntDictionaryDrawer : DictionaryDrawerBase
{
    
}

[CustomPropertyDrawer(typeof(CardDataSpriteDictionary))]
public class CardDataSpriteDictionaryDrawer : DictionaryDrawerBase
{
}

[CustomPropertyDrawer(typeof(CardValueIntDictionary))]
public class CardValueIntDictionaryDrawer : DictionaryDrawerBase
{
}

[CustomPropertyDrawer(typeof(CardData))]
public class CardDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty typeProp = property.FindPropertyRelative(nameof(CardData.Type));
        SerializedProperty valueProp = property.FindPropertyRelative(nameof(CardData.Value));

        float halfWidth = position.width / 2;
        Rect typeRect = new(position.x, position.y, halfWidth - 5, position.height);
        Rect valueRect = new(position.x + halfWidth, position.y, halfWidth - 5, position.height);

        EditorGUI.PropertyField(typeRect, typeProp, GUIContent.none);
        EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);

        EditorGUI.EndProperty();
    }
}