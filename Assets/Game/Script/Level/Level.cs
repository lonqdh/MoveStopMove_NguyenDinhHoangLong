using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level : MonoBehaviour
{
    public int levelId;
    public GameObject levelEnvironment;
    public Transform playerSpawnPoint;
}
