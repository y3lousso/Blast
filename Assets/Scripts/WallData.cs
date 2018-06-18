using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallData {

    [SerializeField] public int Id = 0;
    [SerializeField] public WallPosition position = WallPosition.Top;
    [SerializeField] public int length = 1;

}

public enum WallPosition
{
    ExtremLeft = -3,
    Left = -1,
    Top = 0,
    Right = 1,
    ExtremRight = 3
}
