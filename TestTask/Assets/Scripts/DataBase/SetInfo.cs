using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetInfo")]
public class SetInfo : ScriptableObject
{
    public List<Collection> sets = new List<Collection>();
}

[System.Serializable]
public class Collection
{ 
    public List<Element> elements = new List<Element>();
}

[System.Serializable]
public class Element
{
    public string name;
    public Sprite image;
}