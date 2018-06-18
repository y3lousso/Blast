using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AudioData : ScriptableObject
{
    [Header("Inputs")]
    [SerializeField] public AudioClip audioClip;
    [SerializeField] public int beatPerMinute = 90;
    [SerializeField] public float startingOffset = 0f;
    [SerializeField] public Difficulty difficulty = Difficulty.Normal;

    [Header("From Randomizer")]
    [SerializeField] public int nbFrames;
    [SerializeField] public int randomSeed;
    [SerializeField] public float isNotEmptyChance = 0.5f;
    [SerializeField] public float isDoubleChance = 0.5f;
    [SerializeField] public float isTopChance = 0.5f;
    [SerializeField] public float isExtremChance = 0.5f;
    [SerializeField] public float isSameColorChance = 0.5f;

    [Header("From data Signature")]
    [SerializeField] public string signatureFilePath = "";

    [Header("From data Signature with pattern")]
    [SerializeField] public PatternManager patternManager;

    [Header("Data")]
    [SerializeField] public bool showListCubes = false;
    [SerializeField] public List<TargetCubeData> listCubes = new List<TargetCubeData>();
    [SerializeField] public bool showListWalls = false;
    [SerializeField] public List<WallData> listWalls = new List<WallData>();
}
