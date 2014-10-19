using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

[System.Serializable]
public struct AllyAbilityData
{
    public string abilityID;
    public int abilityUnlockLevel;
}

[System.Serializable]
public struct AllyData
{
    public string unitName;
    public string fileName;
    public int level;
    public string tier;
    public int exp;
    public string allyType;
    public int numOfAbility;
    public AllyAbilityData[] abilityData;
    public string allyEquipID;
    public float damageModifier;
    public int healthModifier;
}


public class AllyDataList : CustomTextFileClass {
    IList<AllyData> allyDataCollection;
    bool equippedAllyLoaded = false;
    AllyData curEquippedAlly;

    public AllyDataList()
    { 
    }

    public void load_cur_equipped_ally()
    {
        AllyData tempData = new AllyData();
        string dataPath = Application.persistentDataPath + "/EquippedAllyList.txt";
        using (StreamReader inFile = File.OpenText(dataPath))
        {
            tempData.unitName = get_next_line(inFile);
            if (tempData.unitName != "NONE")
            {
                tempData.tier = get_next_line(inFile);
                tempData.level = System.Convert.ToInt32(get_next_line(inFile));
                tempData.exp = System.Convert.ToInt32(get_next_line(inFile));
                tempData.allyType = get_next_line(inFile);
                tempData.numOfAbility = System.Convert.ToInt32(get_next_line(inFile));
                tempData.abilityData = new AllyAbilityData[tempData.numOfAbility];
                for (int abilityCtr = 0; abilityCtr < tempData.numOfAbility; abilityCtr++)
                {
                    tempData.abilityData[abilityCtr].abilityID = get_next_line(inFile);
                    tempData.abilityData[abilityCtr].abilityUnlockLevel = System.Convert.ToInt32(get_next_line(inFile));
                }

                tempData.allyEquipID = get_next_line(inFile);
                tempData.damageModifier = (float)System.Convert.ToDouble(get_next_line(inFile));
                tempData.healthModifier = System.Convert.ToInt32(get_next_line(inFile));
            }
        }
        curEquippedAlly = tempData;
        equippedAllyLoaded = true;
    }

    public void save_equipped_ally_data(AllyData inputData)
    {
        string dataPath = Application.persistentDataPath + "/EquippedAllyList.txt";
        using (StreamWriter outfile = File.CreateText(dataPath))
        {
            outfile.WriteLine(inputData.unitName);
            outfile.WriteLine(inputData.tier);
            outfile.WriteLine(inputData.level);
            outfile.WriteLine(inputData.exp);
            outfile.WriteLine(inputData.allyType);
            outfile.WriteLine(inputData.numOfAbility);
            for (int ctr = 0; ctr < inputData.numOfAbility; ctr++)
            {
                outfile.WriteLine(inputData.abilityData[ctr].abilityID);
                outfile.WriteLine(inputData.abilityData[ctr].abilityUnlockLevel);
            }
            outfile.WriteLine(inputData.allyEquipID);
            outfile.WriteLine(inputData.damageModifier);
            outfile.WriteLine(inputData.healthModifier);
        }
    }

    public AllyData get_cur_equipped_ally()
    {
        if (equippedAllyLoaded == false)
            load_cur_equipped_ally();

        return curEquippedAlly;
    }

}
