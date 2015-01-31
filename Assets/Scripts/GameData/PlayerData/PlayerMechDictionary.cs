using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct PlayerMechDictionaryData
{
    public string mechID;
    public string[] closeRangeAbility;
    public string[] farRangeAbility;
}

/*This class is used to create dictionary that contains various player mechs and data that does not need to be saved
 to player.*/
public class PlayerMechDictionary : MonoBehaviour {
    IDictionary<string, PlayerMechDictionaryData> dictionaryData = new Dictionary<string, PlayerMechDictionaryData>();

    public string[] get_close_range_ability(string inputID)
    {
        return dictionaryData[inputID].closeRangeAbility;
    }


    public string[] get_far_range_ability(string inputID)
    {
        return dictionaryData[inputID].farRangeAbility;
    }
}
