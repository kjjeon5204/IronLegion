using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

public struct Stats
{
    public string[] equipment;
	public int hp;
	public int armor;
	public int damage;
	public int energy;
	public float penetration;
	public int luck;
}

public struct PlayerStat
{
	public int item_hp;
	public int item_armor;
	public int item_damage;
	public int item_energy;
	public float item_penetration;
	public int item_luck;
}


public class HeroStats{
    Stats curStats;

    public HeroStats()
    {
    }
    


    public void save_data(string heroMechID)
    {
        string fileName = "/" + heroMechID + "/HeroStats.txt";
        string path = Application.persistentDataPath + fileName;

        using (StreamWriter outfile = File.CreateText(path))
        {
            foreach (string itemID in curStats.equipment) {
                outfile.WriteLine(itemID);
            }
			outfile.WriteLine(curStats.hp);
			outfile.WriteLine(curStats.damage);
			outfile.WriteLine(curStats.energy);
			outfile.WriteLine(curStats.penetration);
			outfile.WriteLine(curStats.luck);
        }
    }

    public void save_data(Stats inputStat, string heroMechID)
    {
        curStats = inputStat;
        string fileName = "/" + heroMechID + "/HeroStats.txt";
        string path = Application.persistentDataPath + fileName;

        using (StreamWriter outfile = File.CreateText(path))
        {
            foreach (string itemID in curStats.equipment)
            {
                outfile.WriteLine(itemID);
            }
            outfile.WriteLine(curStats.hp);
            outfile.WriteLine(curStats.damage);
            outfile.WriteLine(curStats.energy);
            outfile.WriteLine(curStats.penetration);
            outfile.WriteLine(curStats.luck);
        }
    }



    public void create_data(string heroMechID)
    {
        string fileName = "/" + heroMechID + "/HeroStats.txt";
        string path = Application.persistentDataPath + fileName;

        using (StreamWriter outfile = File.CreateText(path)) {
            outfile.WriteLine("000000");
            outfile.WriteLine("000000");
            outfile.WriteLine("000000");
            outfile.WriteLine("000000");
            outfile.WriteLine("000000");
			outfile.WriteLine("0");
			outfile.WriteLine("0");
			outfile.WriteLine("0");
			outfile.WriteLine("0");
			outfile.WriteLine("0");
        }
    }



    public Stats load_data(string heroMechID)
    {
        string fileName = "/" + heroMechID + "/HeroStats.txt";
        string path = Application.persistentDataPath + fileName;
        if (!File.Exists(path))
        {
            create_data(heroMechID);
        }

        string rawFileData;

        
        

        using (StreamReader inFile = File.OpenText(path))
        {
            curStats.equipment = new string[5];
            
            for (int ctr = 0; ctr < 5; ctr++)
            {
                curStats.equipment[ctr] = inFile.ReadLine();
            }
			rawFileData = inFile.ReadLine();
			curStats.hp = Convert.ToInt32(rawFileData);
			rawFileData = inFile.ReadLine();
			curStats.damage = (int)Convert.ToDouble(rawFileData);
			rawFileData = inFile.ReadLine();
			curStats.energy = (int)Convert.ToDouble(rawFileData);
			rawFileData = inFile.ReadLine();
			curStats.penetration = (float)Convert.ToDouble(rawFileData);
			rawFileData = inFile.ReadLine();
			curStats.luck = (int)Convert.ToDouble(rawFileData);
        }
        return curStats;
    }

    public void equip_item(Item equipItem)
    {
        if (equipItem.itemID[2] == 'H')
        {
            curStats.equipment[0] = equipItem.itemID;
            curStats.hp += equipItem.hp;
            curStats.damage += (int)equipItem.damage;
            curStats.armor += (int)equipItem.armor;
            curStats.energy += (int)equipItem.energy;
            curStats.luck += (int)equipItem.luck;
        }
        else if (equipItem.itemID[2] == 'W')
        {
            curStats.equipment[1] = equipItem.itemID;
            curStats.hp += equipItem.hp;
            curStats.damage += (int)equipItem.damage;
            curStats.armor += (int)equipItem.armor;
            curStats.energy += (int)equipItem.energy;
            curStats.luck += (int)equipItem.luck;

        }
        else if (equipItem.itemID[2] == 'B')
        {
            curStats.equipment[2] = equipItem.itemID;
            curStats.hp += equipItem.hp;
            curStats.damage += (int)equipItem.damage;
            curStats.armor += (int)equipItem.armor;
            curStats.energy += (int)equipItem.energy;
            curStats.luck += (int)equipItem.luck;
        }
        else if (equipItem.itemID[2] == 'C')
        {
            Debug.Log(equipItem.itemID + " " + equipItem.itemName);
            curStats.equipment[3] = equipItem.itemID;
            curStats.hp += equipItem.hp;
            curStats.damage += (int)equipItem.damage;
            curStats.armor += (int)equipItem.armor;
            curStats.energy += (int)equipItem.energy;
            curStats.luck += (int)equipItem.luck;
        }
    }

    public void remove_item(Item removeItem)
    {
        if (removeItem.itemID[2] == 'H')
        {
            curStats.equipment[0] = "000000";
            curStats.hp -= removeItem.hp;
            curStats.damage -= (int)removeItem.damage;
            curStats.armor -= (int)removeItem.armor;
            curStats.energy -= (int)removeItem.energy;
            curStats.luck -= (int)removeItem.luck;
        }
        else if (removeItem.itemID[2] == 'W')
        {
            curStats.equipment[1] = "000000";
            curStats.hp -= removeItem.hp;
            curStats.damage -= (int)removeItem.damage;
            curStats.armor -= (int)removeItem.armor;
            curStats.energy -= (int)removeItem.energy;
            curStats.luck -= (int)removeItem.luck;
        }
        else if (removeItem.itemID[2] == 'B')
        {
            curStats.equipment[2] = "000000";
            curStats.hp -= removeItem.hp;
            curStats.damage -= (int)removeItem.damage;
            curStats.armor -= (int)removeItem.armor;
            curStats.energy -= (int)removeItem.energy;
            curStats.luck -= (int)removeItem.luck;
        }
        else if (removeItem.itemID[2] == 'C')
        {
            curStats.equipment[3] = "000000";
            curStats.hp -= removeItem.hp;
            curStats.damage -= (int)removeItem.damage;
            curStats.armor -= (int)removeItem.armor;
            curStats.energy -= (int)removeItem.energy;
            curStats.luck -= (int)removeItem.luck;
        }
    }

    public string[] get_equipped_item()
    {
        return curStats.equipment;
    }

    public string get_equipped_item(int acc)
    {
        return curStats.equipment[acc];
    }

    public Stats get_current_stats()
    {
        return curStats;
    }
	
	public void set_equipped_items(string[] items)
	{
		for (int i = 0; i < 5; i++)
		{
			curStats.equipment[i] = items[i];
		}
	}
	
	public PlayerStat get_item_stats()
	{
		PlayerStat item_stat;
		item_stat.item_hp = curStats.hp;
		item_stat.item_armor = curStats.armor;
		item_stat.item_damage = curStats.damage;
		item_stat.item_energy = curStats.energy;
		item_stat.item_penetration = curStats.penetration;
		item_stat.item_luck = curStats.luck;
		
		return item_stat;
	}
}
