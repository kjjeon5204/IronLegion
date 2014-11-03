using UnityEngine;
using System.Collections;

[System.Serializable]
public struct AIStatElement
{
    public string level;
    public int hp;
    public float baseArmor;
    public float baseAttack;
    public int experience;
}

public class AIStatScript : MonoBehaviour {
    public AIStatElement[] aiStatTable;



    public int get_experience_data(int level)
    {
        return aiStatTable[level - 1].experience;
    }

    public AIStatElement getLevelData(int level)
    {
        if (level > aiStatTable.Length)
        {
            level = 1;
        }
        //Debug.Log("Load enemy level: " + (level - 1));
        return aiStatTable[level - 1];
    }
}
