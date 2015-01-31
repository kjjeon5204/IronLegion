using UnityEngine;
using System.Collections;


/*Struct that holds data for scaling stats. If stat value is not applicable
 to the unit, set to 0*/
[System.Serializable]
public struct CharacterScalingStat
{
    public int hp;
    public int armor;
    public int damage;
    public int experience;
}

public class CharacterStatTable : MonoBehaviour {
    public CharacterScalingStat[] characterStatTable;

    /*This function is used to get the stat depending on level. Level ranges from 0 to n where n is max level - 1.*/
    public CharacterScalingStat get_stat_val(int level)
    {
        return characterStatTable[level];
    }
}
