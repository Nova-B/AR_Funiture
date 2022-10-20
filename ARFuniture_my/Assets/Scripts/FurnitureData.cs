using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FurnitureData
{
    public enum Kind { sofa, chair, bed, sport};

    public Kind kind;
    public Sprite furnitureSprite;
    public GameObject furniturePrefab;
    public string furnitureName;

    [HideInInspector]
    public int id;
}
