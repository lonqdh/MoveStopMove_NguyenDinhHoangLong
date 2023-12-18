using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelDataSO")]
public class LevelDataSO : ScriptableObject
{
    public List<Level> listLevels;
}
