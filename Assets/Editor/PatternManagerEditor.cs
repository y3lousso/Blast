using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(PatternManager))]
[CanEditMultipleObjects]
public class PatternManagerEditor : Editor {

    SerializedProperty showListPatterns;
    SerializedProperty listPatterns;
    private ReorderableList listPatternsReordorable;

    private void OnEnable()
    {
        showListPatterns = serializedObject.FindProperty("showListPatterns");
        listPatterns = serializedObject.FindProperty("listPatterns");
        //InitListPatterns();
    }

    /*private void InitListPatterns()
    {
        listPatternsReordorable = new ReorderableList(serializedObject,
                serializedObject.FindProperty("listPatterns"),
                true, true, true, true);

        listPatternsReordorable.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = listPatternsReordorable.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, 30, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("length"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x+ 40, rect.y, 90, 80),
                element.FindPropertyRelative("targetCubeDatas"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + 40, 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("targetCubeDatas").FindPropertyRelative("Id"), GUIContent.none);
        };
    }*/

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();
        /*
        EditorGUILayout.LabelField("From Randomizer", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(showListPatterns);

        if (showListPatterns.boolValue)
        {
            EditorGUILayout.PropertyField(listPatterns);
        }*/

        serializedObject.ApplyModifiedProperties();
    }
}
