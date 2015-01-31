using UnityEngine;
using System.Collections;

[System.Serializable]
public struct AIStatData
{
    int hp;
    int armor;
    int damage;
}

/*This class handles basic stat table that all AI will use */
public class BaseAIStatTable : MonoBehaviour {
    public AIStatData[] statTable; 
}
