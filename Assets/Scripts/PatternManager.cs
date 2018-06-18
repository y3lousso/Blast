using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PatternManager : ScriptableObject
{
    [SerializeField] public bool showListPatterns = false;
    [SerializeField] public List<Pattern> listPatterns = new List<Pattern>();

}
