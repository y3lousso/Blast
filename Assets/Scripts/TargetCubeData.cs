using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetCubeData
{
    [SerializeField] public int Id = 0;
    [SerializeField] public HorizontalPosition horizontalPosition = HorizontalPosition.Left;
    [SerializeField] public VecticalPosition vecticalPosition = VecticalPosition.Mid;
    [SerializeField] public Orientation orientation = Orientation.Left;
    [SerializeField] public CubeColor cubeColor = CubeColor.Blue;
}

public enum HorizontalPosition
{
    ExtLeft = -2, Left = -1, Right = 1, ExtRight = 2
};

public enum VecticalPosition
{
    Bot = -1, Mid = 0, Top = 1
}

public enum Orientation
{
    Bot = 0, 
    Right = 1,
    Top = 2,
    Left = 3
}

public enum CubeColor
{
    Blue = 1,
    Red = 2
}
