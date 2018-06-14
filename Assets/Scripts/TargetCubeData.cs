using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetCubeData
{
    [SerializeField] public int Id = 0;
    [SerializeField] public HorizontalPosition horizontalPosition = HorizontalPosition.Left;
    [SerializeField] public VerticalPosition vecticalPosition = VerticalPosition.Bot;
    [SerializeField] public Orientation orientation = Orientation.Left;
    [SerializeField] public CubeColor cubeColor = CubeColor.Blue;
}

public enum HorizontalPosition
{
    ExtLeft = -3, Left = -1, Right = 1, ExtRight = 3
};

public enum VerticalPosition
{
    Bot = 0, Top = 1
}

public enum Orientation
{
    Top = 0,
    Left = 1,
    Bot = 2,
    Right = 3
}

public enum CubeColor
{
    Blue = 1,
    Red = 2
}
