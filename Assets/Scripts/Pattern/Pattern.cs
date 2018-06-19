using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pattern {

    [SerializeField] public int length = 0;
    [SerializeField] public List<TargetCubeData> targetCubeDatas = new List<TargetCubeData>();
}
