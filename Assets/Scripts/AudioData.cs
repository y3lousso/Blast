using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioData : ScriptableObject
{
    [SerializeField] public AudioClip audioClip;
    [SerializeField] public int beatPerMinute = 90;
    [SerializeField] public float startingOffset = 0f;
    [SerializeField] public List<TargetCubeData> targetCubesData = new List<TargetCubeData>();
}
