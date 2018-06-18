using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using System.Linq;
using System;

[CustomEditor(typeof(AudioData))]
[CanEditMultipleObjects]
public class AudioDataEditor : Editor {
    
    SerializedProperty audioClip;
    SerializedProperty beatPerMinute;
    SerializedProperty startingOffset;
    SerializedProperty difficulty;

    // Random
    SerializedProperty nbFrames;
    SerializedProperty randomSeed;   
    SerializedProperty isNotEmptyChance;
    SerializedProperty isDoubleChance;
    SerializedProperty isTopChance;
    SerializedProperty isExtremChance;
    SerializedProperty isSameColorChance;

    // With signature
    SerializedProperty signatureFilePath;

    //With signature and patterns
    SerializedProperty patternManager;

    SerializedProperty showListCubes;
    SerializedProperty listCubes;
    private ReorderableList listCubesReordorable;

    SerializedProperty showListWalls;
    SerializedProperty listWalls;
    private ReorderableList listWallsReordorable;

    #region Inspector

    private void OnEnable()
    {
        audioClip = serializedObject.FindProperty("audioClip");
        beatPerMinute = serializedObject.FindProperty("beatPerMinute");
        startingOffset = serializedObject.FindProperty("startingOffset");
        difficulty = serializedObject.FindProperty("difficulty");

        nbFrames = serializedObject.FindProperty("nbFrames");
        randomSeed = serializedObject.FindProperty("randomSeed");       
        isNotEmptyChance = serializedObject.FindProperty("isNotEmptyChance");
        isDoubleChance = serializedObject.FindProperty("isDoubleChance");
        isTopChance = serializedObject.FindProperty("isTopChance");
        isExtremChance = serializedObject.FindProperty("isExtremChance");
        isSameColorChance = serializedObject.FindProperty("isSameColorChance");

        signatureFilePath = serializedObject.FindProperty("signatureFilePath");

        patternManager = serializedObject.FindProperty("patternManager");

        showListCubes = serializedObject.FindProperty("showListCubes");        
        listCubes = serializedObject.FindProperty("listCubes");
        InitListCubes();

        showListWalls = serializedObject.FindProperty("showListWalls");
        listWalls = serializedObject.FindProperty("listWalls");
        InitListWalls();
    }

    private void InitListCubes()
    {
        listCubesReordorable = new ReorderableList(serializedObject,
                serializedObject.FindProperty("listCubes"),
                true, true, true, true);

        listCubesReordorable.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = listCubesReordorable.serializedProperty.GetArrayElementAtIndex(index);
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

        listCubesReordorable.onAddCallback = (ReorderableList l) => {
            var index = l.serializedProperty.arraySize;
            l.serializedProperty.arraySize++;
            l.index = index;
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("Id").intValue = listCubesReordorable.count;
        };
    }

    private void InitListWalls()
    {
        listWallsReordorable = new ReorderableList(serializedObject,
                serializedObject.FindProperty("listWalls"),
                true, true, true, true);

        listWallsReordorable.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = listWallsReordorable.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, 30, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Id"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + 35, rect.y, 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("position"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + 100, rect.y, 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("length"), GUIContent.none);
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(audioClip);
        EditorGUILayout.PropertyField(beatPerMinute);
        EditorGUILayout.PropertyField(startingOffset);
        EditorGUILayout.PropertyField(difficulty);


        EditorGUILayout.PropertyField(nbFrames);
        EditorGUILayout.PropertyField(randomSeed);
        EditorGUILayout.Slider(isNotEmptyChance, 0f, 1f);
        EditorGUILayout.Slider(isDoubleChance, 0f, 1f);
        EditorGUILayout.Slider(isTopChance, 0f, 1f);
        EditorGUILayout.Slider(isExtremChance, 0f, 1f);
        EditorGUILayout.Slider(isSameColorChance, 0f, 1f);

        if (GUILayout.Button("Randomize"))
        {
            ((AudioData)target).listCubes = Randomizer();
        }

        EditorGUILayout.PropertyField(signatureFilePath);

        if (GUILayout.Button("Generate from signature (filepath)"))
        {
            ((AudioData)target).listCubes = GenerateFromFile(signatureFilePath.stringValue);
        }

        EditorGUILayout.PropertyField(patternManager);

        if (GUILayout.Button("Generate from signature and patterns"))
        {
            ((AudioData)target).listCubes = GenerateFromFileAndPatterns(signatureFilePath.stringValue);
        }

        EditorGUILayout.PropertyField(showListCubes);

        if (showListCubes.boolValue)
        {
            listCubesReordorable.DoLayoutList();
        }

        EditorGUILayout.PropertyField(showListWalls);

        if (showListWalls.boolValue)
        {
            listWallsReordorable.DoLayoutList();
        }

        serializedObject.ApplyModifiedProperties();
    }

    #endregion

    #region ListBuildingLogic

    private List<TargetCubeData> Randomizer()
    {
        UnityEngine.Random.InitState(randomSeed.intValue);

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
                    current.vecticalPosition = VerticalPosition.Top;
                }
                else
                {
                    current.vecticalPosition = VerticalPosition.Bot;
                }

                current.orientation = (Orientation)RandomEnum(new int[] { 0, 1, 2, 3 } );

                current.cubeColor = cubeColor;
                targetCubesData.Add(current);

                if (isDouble)
                {
                    TargetCubeData current2 = new TargetCubeData();
                    current2.Id = i;

                    UnityEngine.Random.InitState(i);

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
                        current2.vecticalPosition = VerticalPosition.Top;
                    }
                    else
                    {
                        current2.vecticalPosition = VerticalPosition.Bot;
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

    /// <summary>
    /// File need to be Assets folder
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private List<TargetCubeData> GenerateFromFile(string filePath)
    {
        // From 2D array of bool to Tuple<Index,Size>
        List<List<bool>> lines = ExtractBoolArrayFromFile(Application.dataPath +"/"+ filePath);
        List<bool> bools = Convert2dBoolArrayTo1d(lines);
        List<Tuple<int, int>> tuples = GenerateTupleFromBool(bools);

        // Generate random targetCubeData for each tuple
        List<TargetCubeData> targetCubeDatas = new List<TargetCubeData>();

        foreach(Tuple<int,int> t in tuples)
        {
            targetCubeDatas.AddRange(GenerateDefaultPattern(t));
        }

        return targetCubeDatas;
    }

    private List<TargetCubeData> GenerateFromFileAndPatterns(string filePath)
    {
        // From 2D array of bool to Tuple<Index,Size>
        List<List<bool>> lines = ExtractBoolArrayFromFile(Application.dataPath + "/" + filePath);
        List<bool> bools = Convert2dBoolArrayTo1d(lines);
        List<Tuple<int, int>> tuples = GenerateTupleFromBool(bools);

        // Generate random targetCubeData for each tuple
        List<TargetCubeData> targetCubeDatas = new List<TargetCubeData>();
       
        foreach (Tuple<int, int> t in tuples)
        {
            //Debug.Log(t);
            List<Pattern> patterns = ((AudioData)target).patternManager.listPatterns.FindAll(p => p.length == t.Item2);
            Pattern pattern = null;

            if (patterns.Count > 0)
            {
                pattern = patterns[UnityEngine.Random.Range(0, patterns.Count)];
            }

            if ( pattern != null)
            {
                foreach(TargetCubeData t2 in pattern.targetCubeDatas)
                {
                    TargetCubeData targetCube = new TargetCubeData(t2);
                    targetCube.Id = t2.Id + t.Item1;                    
                    targetCubeDatas.Add(targetCube);
                }  
            }
            else
            {
                targetCubeDatas.AddRange(GenerateDefaultPattern(t));
            }            
        }

        return targetCubeDatas;
    }   

    // Example 
    // Index : 1  5  10
    // Size  : 3  1  2
    private List<Tuple<int, int>> GenerateTupleFromBool(List<bool> bools)
    {
        List<Tuple<int, int>> listTuples = new List<Tuple<int, int>>();

        int currentIndex = -1;
        int currentSize = 0;

        for (int i = 0; i < bools.Count; i++)
        {
            if (bools[i])
            {
                if(currentIndex == -1)
                {
                    currentIndex = i;
                }
                currentSize++;
                if(i == bools.Count - 1)
                {
                    listTuples.Add(new Tuple<int, int>(currentIndex, currentSize));
                }
            }
            else
            {
                if (currentIndex != -1)
                {
                    listTuples.Add(new Tuple<int, int>(currentIndex, currentSize));
                    currentIndex = -1;
                    currentSize = 0;
                }
            }
        }

        return listTuples;
    }

    private List<TargetCubeData> GenerateDefaultPattern(Tuple<int,int> tuple)
    {
        List<TargetCubeData> targetCubeDatas = new List<TargetCubeData>();
        for (int i = 0; i < tuple.Item2; i++)
        {
            TargetCubeData cube = new TargetCubeData { Id = tuple.Item1 + i, cubeColor = CubeColor.Blue, horizontalPosition = HorizontalPosition.Right, vecticalPosition = VerticalPosition.Bot, orientation = Orientation.Top };
            if ((tuple.Item1 + i) % 2 == 0)
            {
                cube.orientation = Orientation.Bot;
            }
            targetCubeDatas.Add(cube);                
        }
        return targetCubeDatas;
    }

    #endregion

    #region Utils

    private int RandomEnum(int[] enums)
    {
        return enums[UnityEngine.Random.Range(0, enums.Length)];
    }

    private List<List<bool>> ExtractBoolArrayFromFile(string filePath)
    {
        List<List<bool>> signature = new List<List<bool>>();
        if (!File.Exists(filePath))
        {
            throw new IOException("The file " + filePath + " doesn't exist !");
        }

        string[] file = File.ReadAllLines(filePath);
        foreach (string s in file)
        {
            signature.Add(s.Split(',').Select(bool.Parse).ToList());
        }

        return signature;
    }

    private bool RandomBool(float chance)
    {
        float random = UnityEngine.Random.Range(0f, 1f);
        if (random <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private List<bool> Convert2dBoolArrayTo1d(List<List<bool>> lines)
    {
        List<bool> outputs = new List<bool>();

        foreach (List<bool> line in lines)
        {
            if (line.Any(b => b.Equals(true)))
            {
                outputs.Add(true);
            }
            else
            {
                outputs.Add(false);
            }
        }
        return outputs;
    }

    #endregion
}
