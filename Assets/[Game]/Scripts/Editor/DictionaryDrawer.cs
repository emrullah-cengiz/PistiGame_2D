using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class DictionaryDrawerBase : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty keysProp = property.FindPropertyRelative(nameof(SerializableDictionary<object, object>._keys));
        SerializedProperty valuesProp = property.FindPropertyRelative(nameof(SerializableDictionary<object, object>._values));

        float spacing = EditorGUIUtility.standardVerticalSpacing;
        float labelHeight = EditorGUIUtility.singleLineHeight;
        float buttonHeight = EditorGUIUtility.singleLineHeight;
        float rowHeight = EditorGUI.GetPropertyHeight(keysProp);
        float halfWidth = position.width / 2;

        Rect labelRect = new Rect(position.x, position.y, position.width, labelHeight);
        EditorGUI.LabelField(labelRect, label, new GUIStyle(EditorStyles.boldLabel)); 

        Rect buttonRect = new Rect(position.x, position.y + labelHeight + spacing, position.width, buttonHeight);
        if (GUI.Button(buttonRect, "CLEAR DUPLICATES")) 
            ClearDuplicates(keysProp, valuesProp);

        float yOffset = position.y + labelHeight + buttonHeight + spacing * 2;

        Rect keysRect = new Rect(position.x, yOffset, halfWidth - 5, rowHeight);
        Rect valuesRect = new Rect(position.x + halfWidth, yOffset, halfWidth - 5, rowHeight);

        EditorGUI.PropertyField(keysRect, keysProp, GUIContent.none, true);
        EditorGUI.PropertyField(valuesRect, valuesProp, GUIContent.none, true);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty keysProp = property.FindPropertyRelative(nameof(SerializableDictionary<object, object>._keys));
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        float labelHeight = EditorGUIUtility.singleLineHeight;
        float buttonHeight = EditorGUIUtility.singleLineHeight;
        float listHeight = EditorGUI.GetPropertyHeight(keysProp);

        return labelHeight + buttonHeight + spacing * 2 + listHeight;
    }

    protected  void ClearDuplicates(SerializedProperty keysProp, SerializedProperty valuesProp)
    {
        HashSet<object> seenKeys = new HashSet<object>();
        List<int> duplicateIndexes = new List<int>();

        for (int i = 0; i < keysProp.arraySize; i++)
        {
            SerializedProperty keyProp = keysProp.GetArrayElementAtIndex(i);
            object keyValue = GetPropertyValue(keyProp);

            if (!seenKeys.Add(keyValue)) 
                duplicateIndexes.Add(i);
        }

        for (int i = duplicateIndexes.Count - 1; i >= 0; i--)
        {
            keysProp.DeleteArrayElementAtIndex(duplicateIndexes[i]);
            valuesProp.DeleteArrayElementAtIndex(duplicateIndexes[i]);
        }

        keysProp.serializedObject.ApplyModifiedProperties();
    }

    protected  static object GetPropertyValue(SerializedProperty property)
    {
        return property.propertyType switch
        {
            SerializedPropertyType.Integer => property.intValue,
            SerializedPropertyType.Boolean => property.boolValue,
            SerializedPropertyType.Float => property.floatValue,
            SerializedPropertyType.String => property.stringValue,
            SerializedPropertyType.Enum => property.enumValueIndex,
            SerializedPropertyType.ObjectReference => property.objectReferenceValue,
            SerializedPropertyType.Generic => property.boxedValue,
            _ => property.ToString()
        };
    }
}
