using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Res), true)]
public class ResDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect popRect = new Rect(position.x, position.y, position.width, 18);
        Rect foodRect = new Rect(position.x, position.y + 20, position.width, 18);
        Rect woodRect = new Rect(position.x, position.y + 40, position.width, 18);
        Rect stoneRect = new Rect(position.x, position.y + 60, position.width, 18);
        Rect coinRect = new Rect(position.x, position.y + 80, position.width, 18);

        EditorGUI.PropertyField(popRect, property.FindPropertyRelative("pop"));
        EditorGUI.PropertyField(foodRect, property.FindPropertyRelative("food"));
        EditorGUI.PropertyField(woodRect, property.FindPropertyRelative("wood"));
        EditorGUI.PropertyField(stoneRect, property.FindPropertyRelative("stone"));
        EditorGUI.PropertyField(coinRect, property.FindPropertyRelative("coin"));

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 20 * 5;
    }
}
