using UnityEngine;
using System.Collections;
using System.IO;

public struct UpgradeSubData
{
    public int statValue;
    public int upgradeCount;
}

public struct UpgradeStats
{
    public UpgradeSubData HP;
    public UpgradeSubData damage;
    public UpgradeSubData energy;
}

public class UpgradeData 
{
    UpgradeStats myData;


    public UpgradeData()
    {
        string dataPath = Application.persistentDataPath +
            "/UpgradeData.txt";

        if (!File.Exists(dataPath))
        {
            using (StreamWriter outFile = File.CreateText(dataPath))
            {
                for (int ctr = 0; ctr < 6; ctr++) 
                    outFile.WriteLine("0");
            }
        }

        using (StreamReader inFile = File.OpenText(dataPath))
        {
            myData.HP.statValue = System.Convert.ToInt32(inFile.ReadLine());
            myData.HP.upgradeCount = System.Convert.ToInt32(inFile.ReadLine());
            myData.damage.statValue = System.Convert.ToInt32(inFile.ReadLine());
            myData.damage.upgradeCount = System.Convert.ToInt32(inFile.ReadLine());
            myData.energy.statValue = System.Convert.ToInt32(inFile.ReadLine());
            myData.energy.upgradeCount = System.Convert.ToInt32(inFile.ReadLine());
        }
    }

    public UpgradeStats get_upgrade_data()
    {
        return myData;
    }

    public void upgrade_hp()
    {
        myData.HP.statValue += 10;
        myData.HP.upgradeCount++;
    }

    public void upgrade_energy()
    {
        myData.energy.statValue++;
        myData.energy.upgradeCount++;
    }

    public void upgrade_damage()
    {
        myData.damage.statValue += 5;
        myData.damage.upgradeCount++;
    }

    public int get_upgrade_count(UpgradeType findType)
    {
        if (findType == UpgradeType.HP)
        {
            return myData.HP.upgradeCount;
        }
        else if (findType == UpgradeType.DAMAGE)
        {
            return myData.damage.upgradeCount;
        }
        else if (findType == UpgradeType.ENERGY)
        {
            return myData.energy.upgradeCount;
        }
        else
        {
            return 0;
        }
    }

    public void save_upgrade_data()
    {
        string dataPath = Application.persistentDataPath +
            "/UpgradeData.txt";

        using (StreamWriter outFile = File.CreateText(dataPath))
        {
            outFile.WriteLine(myData.HP.statValue);
            outFile.WriteLine(myData.HP.upgradeCount);

            outFile.WriteLine(myData.damage.statValue);
            outFile.WriteLine(myData.damage.upgradeCount);

            outFile.WriteLine(myData.energy.statValue);
            outFile.WriteLine(myData.energy.upgradeCount);
        }
    }
}
