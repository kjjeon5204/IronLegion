using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityDictionary : MonoBehaviour {
    IDictionary<string, PlayerAbilityData> abilityDataDictionary =
        new Dictionary<string, PlayerAbilityData>();

    public PlayerAbilityData find_ability_data(string abilityName)
    {
        if (abilityDataDictionary.ContainsKey(abilityName))
        {
            return abilityDataDictionary[abilityName];
        }
        return null;
    }
}
