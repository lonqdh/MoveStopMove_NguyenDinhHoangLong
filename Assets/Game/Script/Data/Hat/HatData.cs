using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HatType
{
    Default = 0,
    Arrow = 1,
    Cowboy = 2,
    Crown = 3,
    Ear = 4,
    Hat = 5,
    Hat_Cap = 6,
    Hat_Yellow = 7,
    Horn = 8
}

[Serializable]
public class HatData
{
    public HatType hatType;
    public GameObject hatPrefab;
    public Sprite hatImage;
    public float range;
    public int hatPrice;
}
