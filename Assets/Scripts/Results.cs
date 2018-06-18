using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Results
{
    [SerializeField] public string Date { get; set; } = "";
    [SerializeField] public Difficulty Difficulty { get; set; } = Difficulty.Normal;
    [SerializeField] public int CorrectHit { get; set; } = 0;
    [SerializeField] public int MaxHit { get; set; } = 0;
    [SerializeField] public int Score { get; set; } = 0;
}

public enum Difficulty
{
    Easy,
    Normal,
    Hard,
    Expert
}
