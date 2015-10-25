using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CrossMenuButton))]
public class CrossMenuButtonDrawer : PropertyDrawer
{
    const float letterWidth = 10f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        EditorGUI.indentLevel = 0;
        Rect contentPosition = EditorGUI.PrefixLabel(position, label);

        float yOffset = 18f;
        position.height = 16f;

        contentPosition = EditorGUI.IndentedRect(position);
        contentPosition.y += yOffset;
        EditorGUIUtility.labelWidth = letterWidth * 2;
        contentPosition.x += contentPosition.width * 0.36f - EditorGUIUtility.labelWidth;
        contentPosition.width = contentPosition.width * 0.3f + EditorGUIUtility.labelWidth;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("Up"), new GUIContent("Up"));

        yOffset += 18f;

        contentPosition = EditorGUI.IndentedRect(position);
        contentPosition.y += yOffset;
        EditorGUIUtility.labelWidth = letterWidth * 4;
        contentPosition.width = contentPosition.width * 0.33f;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("Left"), new GUIContent("Left"));

        contentPosition = EditorGUI.IndentedRect(position);
        contentPosition.y += yOffset;
        EditorGUIUtility.labelWidth = letterWidth * 5;
        contentPosition.x += contentPosition.width * 0.66f;
        contentPosition.width = contentPosition.width * 0.33f;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("Right"), new GUIContent("Right"));

        yOffset += 18f;

        contentPosition = EditorGUI.IndentedRect(position);
        contentPosition.y += yOffset;
        EditorGUIUtility.labelWidth = letterWidth * 4;
        contentPosition.x += contentPosition.width * 0.36f - EditorGUIUtility.labelWidth;
        contentPosition.width = contentPosition.width * 0.3f + EditorGUIUtility.labelWidth;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("Down"), new GUIContent("Down"));
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 16f + 54f;
    }
}
