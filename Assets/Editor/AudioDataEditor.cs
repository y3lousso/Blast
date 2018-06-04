using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(AudioData))]
public class AudioDataEditor : Editor {

    SerializedProperty audioClip;
    SerializedProperty beatPerMinute;
    SerializedProperty startingOffset;
    private ReorderableList list;

    private void OnEnable()
    {
        audioClip = serializedObject.FindProperty("audioClip");
        beatPerMinute = serializedObject.FindProperty("beatPerMinute");
        startingOffset = serializedObject.FindProperty("startingOffset");

        list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("targetCubesData"),
                true, true, true, true);

        list.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, 30, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Id"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + 35, rect.y, 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("horizontalPosition"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + 100, rect.y, 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("vecticalPosition"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + 165, rect.y, 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("orientation"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + 230, rect.y, 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("cubeColor"), GUIContent.none);
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(audioClip);
        EditorGUILayout.PropertyField(beatPerMinute);
        EditorGUILayout.PropertyField(startingOffset);
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
