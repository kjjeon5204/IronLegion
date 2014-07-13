﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public struct AIStatElement
{
    public string level;
    public int hp;
    public float baseArmor;
    public float baseAttack;
}

public class AIStatScript : MonoBehaviour {
    public AIStatElement[] aiStatTable;

    public AIStatElement getLevelData(int level)
    {
        if (level > aiStatTable.Length)
        {
            level = 0;
        }
        return aiStatTable[level - 1];
    }
}
