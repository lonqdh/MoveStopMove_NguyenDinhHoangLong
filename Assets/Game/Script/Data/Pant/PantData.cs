using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PantType
{
    Default = 0,
    Batman = 1,
    Chambi = 2,
    Dabao = 3,
    Onion = 4,
    Pokemon = 5,
    Rainbow = 6,
    Skull = 7,
    Vantim = 8
}


[Serializable]
public class PantData
{
    public PantType PantType;
    public Material PantMaterial;
    public Sprite PantImage;
    public int PantPrice;
    public float Speed;
}



