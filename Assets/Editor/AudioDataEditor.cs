using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(AudioData))]
[CanEditMultipleObjects]
public class AudioDataEditor : Editor {
    
    SerializedProperty audioClip;
    SerializedProperty beatPerMinute;
    SerializedProperty nbFrames;
    SerializedProperty randomSeed;
    SerializedProperty startingOffset;

    [Range(0,1)]
    SerializedProperty isNotEmptyChance;
    SerializedProperty isDoubleChance;
    SerializedProperty isTopChance;
    SerializedProperty isExtremChance;
    SerializedProperty isSameColorChance;

    SerializedProperty list;

    private ReorderableList listPatterns;

    private void OnEnable()
    {
        audioClip = serializedObject.FindProperty("audioClip");
        beatPerMinute = serializedObject.FindProperty("beatPerMinute");
        nbFrames = serializedObject.FindProperty("nbFrames");
        randomSeed = serializedObject.FindProperty("randomSeed");
        startingOffset = serializedObject.FindProperty("startingOffset");

        isNotEmptyChance = serializedObject.FindProperty("isNotEmptyChance");
        isDoubleChance = serializedObject.FindProperty("isDoubleChance");
        isTopChance = serializedObject.FindProperty("isTopChance");
        isExtremChance = serializedObject.FindProperty("isExtremChance");
        isSameColorChance = serializedObject.FindProperty("isSameColorChance");

        list = serializedObject.FindProperty("targetCubesData");


        listPatterns = new ReorderableList(serializedObject,
                serializedObject.FindProperty("targetCubesData"),
                true, true, true, true);

        listPatterns.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = listPatterns.serializedProperty.GetArrayElementAtIndex(index);
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

        listPatterns.onAddCallback = (ReorderableList l) => {
            var index = l.serializedProperty.arraySize;
            l.serializedProperty.arraySize++;
            l.index = index;
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("Id").intValue = listPatterns.count;
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(audioClip);
        EditorGUILayout.PropertyField(beatPerMinute);
        EditorGUILayout.PropertyField(nbFrames);
        EditorGUILayout.PropertyField(randomSeed);
        EditorGUILayout.PropertyField(startingOffset);

        EditorGUILayout.Slider(isNotEmptyChance, 0f, 1f);
        EditorGUILayout.Slider(isDoubleChance, 0f, 1f);
        EditorGUILayout.Slider(isTopChance, 0f, 1f);
        EditorGUILayout.Slider(isExtremChance, 0f, 1f);
        EditorGUILayout.Slider(isSameColorChance, 0f, 1f);

        if (GUILayout.Button("Randomize"))
        {
            ((AudioData)target).targetCubesData = Randomizer();
        }

        listPatterns.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private List<TargetCubeData> Randomizer()
    {
        Random.InitState(randomSeed.intValue);

        List<TargetCubeData> targetCubesData = new List<TargetCubeData>();

        CubeColor cubeColor = (CubeColor)RandomEnum(new int[] { (int)CubeColor.Blue, (int)CubeColor.Red });

        for (int i = 0; i < nbFrames.intValue; i++)
        {
            //Setting
            bool isNotEmpty = RandomBool(isNotEmptyChance.floatValue);
            bool isDouble = RandomBool(isDoubleChance.floatValue);
            bool isTop = RandomBool(isTopChance.floatValue);
            bool isExtrem = RandomBool(isExtremChance.floatValue);
            bool isSameColor = RandomBool(isSameColorChance.floatValue);

            if (isNotEmpty)
            {
                TargetCubeData current = new TargetCubeData();
                current.Id = i;

                if (isExtrem)
                {
                    current.horizontalPosition = (HorizontalPosition)RandomEnum(new int[] { (int)HorizontalPosition.ExtLeft, (int)HorizontalPosition.ExtRight });
                }
                else
                {
                    current.horizontalPosition = (HorizontalPosition)RandomEnum(new int[] { (int)HorizontalPosition.Left, (int)HorizontalPosition.Right });
                }
                if (isTop)
                {
                    current.vecticalPosition = VecticalPosition.Top;
                }
                else
                {
                    current.vecticalPosition = VecticalPosition.Bot;
                }

                current.orientation = (Orientation)RandomEnum(new int[] { 0, 1, 2, 3 } );

                current.cubeColor = cubeColor;
                targetCubesData.Add(current);

                if (isDouble)
                {
                    TargetCubeData current2 = new TargetCubeData();
                    current2.Id = i;

                    Random.InitState(i);

                    if (isExtrem)
                    {
                        current2.horizontalPosition = (HorizontalPosition)RandomEnum(new int[] { (int)HorizontalPosition.ExtLeft, (int)HorizontalPosition.ExtRight });
                    }
                    else
                    {
                        current2.horizontalPosition = (HorizontalPosition)RandomEnum(new int[] { (int)HorizontalPosition.Left, (int)HorizontalPosition.Right });
                    }
                    if (!isTop)
                    {
                        current2.vecticalPosition = VecticalPosition.Top;
                    }
                    else
                    {
                        current2.vecticalPosition = VecticalPosition.Bot;
                    }

                    current2.orientation = (Orientation)RandomEnum(new int[] { 0, 1, 2, 3 });

                    if (cubeColor == CubeColor.Blue)
                    {
                        current2.cubeColor = CubeColor.Red;
                    }
                    else
                    {
                        current2.cubeColor = CubeColor.Blue;
                    }

                    targetCubesData.Add(current2);
                }

                if (!isSameColor)
                {
                    if(cubeColor == CubeColor.Blue)
                    {
                        cubeColor = CubeColor.Red;
                    }
                    else
                    {
                        cubeColor = CubeColor.Blue;
                    }              
                }          
            }
        }

        return targetCubesData;
    }

    private bool RandomBool(float chance)
    {
        float random = Random.Range(0f, 1f);
        if (random <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private int RandomEnum(int[] enums)
    {
        return enums[Random.Range(0, enums.Length)];
    }
}
