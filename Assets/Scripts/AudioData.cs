using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioData : ScriptableObject
{
    [SerializeField] public AudioClip audioClip;
    [SerializeField] public int beatPerMinute = 90;
    [SerializeField] public int nbFrames;
    [SerializeField] public int randomSeed;
    [SerializeField] public float startingOffset = 0f;

    [Header("Settings")]
    [Range(0,1)]
    [SerializeField] public float isNotEmptyChance = 0.5f;
    [SerializeField] public float isDoubleChance = 0.5f;
    [SerializeField] public float isTopChance = 0.5f;
    [SerializeField] public float isExtremChance = 0.5f;
    [SerializeField] public float isSameColorChance = 0.5f;

    [SerializeField] public List<TargetCubeData> targetCubesData = new List<TargetCubeData>();
}
