using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ComputerInput))]
public class ComputerInputEditorEditor : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.FindPropertyRelative("Opened").boolValue)
            return 68;
        else
            return 17;
    }

    public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
    {
        pos.height = 17;
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.label);

        string labelName = (property.FindPropertyRelative("Opened").boolValue ? "▼ " : "► ") + label.text;
        pos.x += 17;
        pos.width -= 17;
        bool buttonPressed = GUI.Button(pos, labelName, buttonStyle);

        if (buttonPressed)
        {
            property.FindPropertyRelative("Opened").boolValue = !property.FindPropertyRelative("Opened").boolValue;
        }

        if (property.FindPropertyRelative("Opened").boolValue)
        {
            SerializedProperty inputType = property.FindPropertyRelative("_inputType");
            SerializedProperty inputMode = property.FindPropertyRelative("_inputMode");
            SerializedProperty mouseButtonid = property.FindPropertyRelative("_mouseButtonId");
            SerializedProperty keyboardKey = property.FindPropertyRelative("_key");
            SerializedProperty wheelDirection = property.FindPropertyRelative("_wheelDirection");

            pos.y += 17;
            inputType.enumValueIndex = (int)((ComputerInputType)EditorGUI.EnumPopup(pos, "Input Type", (ComputerInputType)inputType.enumValueIndex));

            pos.y += 17;
            inputMode.enumValueIndex = (int)((InputManager.InputMode)EditorGUI.EnumPopup(pos, "Input Mode", (InputManager.InputMode)inputMode.enumValueIndex));

            pos.y += 17;
            switch ((ComputerInputType)inputType.enumValueIndex)
            {
                case ComputerInputType.Mouse:
                    mouseButtonid.intValue = EditorGUI.IntField(pos, "Mouse Button", mouseButtonid.intValue);
                    break;
                case ComputerInputType.Keyboard:
                    keyboardKey.intValue = (int)((KeyCode)EditorGUI.EnumPopup(pos, "Key", (KeyCode)keyboardKey.intValue));
                    break;
                case ComputerInputType.Wheel:
                    wheelDirection.enumValueIndex = (int)((InputManager.InputState)EditorGUI.EnumPopup(pos, "Wheel Direction", (InputManager.InputState)wheelDirection.enumValueIndex));
                    break;
            }
        }
        else
        {

        }
    }
}
#endif
