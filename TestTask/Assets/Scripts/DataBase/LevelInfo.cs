using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelInfo")]
public class LevelInfo : ScriptableObject
{
    public List<Level> levels = new List<Level>();
}

[System.Serializable]

public class Level
{
    [Header("Выберите конкретный набор из SetInfo или используйте случайный, задав нестандартное значение")]
    public int type;
    public int columns;
    public int lines;
}