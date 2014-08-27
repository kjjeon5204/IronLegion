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
        aiStatTable[0].experience = 100;
        aiStatTable[1].experience = 225;
        aiStatTable[2].experience = 338;
        aiStatTable[3].experience = 675;
        aiStatTable[4].experience = 1266;
        aiStatTable[5].experience = 2278;
        aiStatTable[6].experience = 3987;
        aiStatTable[7].experience = 6834;
        aiStatTable[8].experience = 11533;
        aiStatTable[9].experience = 19222;
        aiStatTable[10].experience = 31716;
        aiStatTable[11].experience = 51899;
        aiStatTable[12].experience = 84335;
        aiStatTable[13].experience = 136234;
        aiStatTable[14].experience = 218947;
        aiStatTable[15].experience = 350315;
        aiStatTable[16].experience = 886735;
        aiStatTable[17].experience = 1403997;
        aiStatTable[18].experience = 2216838;
        aiStatTable[19].experience = 3491520;
        aiStatTable[20].experience = 5486674;
        aiStatTable[21].experience = 8604102;
        aiStatTable[22].experience = 13467290;
        aiStatTable[23].experience = 0;
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
