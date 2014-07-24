using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

public struct Stats
{
    public int level;
    public float curExp;
    public float totalExp;
    public int baseHp;
    public float baseDamage;
    public float armor;
    public string[] equipment;
	public int item_hp;
	public float item_armor;
	public float item_damage;
	public float item_energy;
	public float item_penetration;
	public float item_luck;
}

public struct PlayerStat
{
	public int item_hp;
	public float item_armor;
	public float item_damage;
	public float item_energy;
	public float item_penetration;
	public float item_luck;
}


public class HeroStats{
    Stats curStats;

    public HeroStats()
    {
    }
    
    public void level_up()
    {
        curStats.baseHp += 50;
        curStats.baseDamage += 25.0f;
        curStats.totalExp += 5 * curStats.level;
        curStats.level++;
    }

    public void increaseXp(float input)
    {
        curStats.curExp += input;
        if (curStats.curExp >= curStats.totalExp) {
            this.level_up();
            curStats.curExp = curStats.totalExp - curStats.curExp;
        }
    }

    public void save_data()
    {
        string fileName = "/HeroStats.txt";
        string path = Application.persistentDataPath + fileName;

        using (StreamWriter outfile = File.CreateText(path))
        {
            outfile.WriteLine(curStats.level);
            outfile.WriteLine(curStats.curExp);
            outfile.WriteLine(curStats.totalExp);
            outfile.WriteLine(curStats.baseHp);
            outfile.WriteLine(curStats.baseDamage);
            outfile.WriteLine(curStats.armor);
            foreach (string itemID in curStats.equipment) {
                outfile.WriteLine(itemID);
            }
			outfile.WriteLine(curStats.item_hp);
			outfile.WriteLine(curStats.item_damage);
			outfile.WriteLine(curStats.item_energy);
			outfile.WriteLine(curStats.item_penetration);
			outfile.WriteLine(curStats.item_luck);
        }
    }



    public void create_data()
    {
        string fileName = "/HeroStats.txt";
        string path = Application.persistentDataPath + fileName;

        using (StreamWriter outfile = File.CreateText(path)) {
            outfile.WriteLine("1"); /*Level*/
            outfile.WriteLine("0"); /*cur Exp*/
            outfile.WriteLine("50"); /*total Exp*/
            outfile.WriteLine("500"); /*HP*/
            outfile.WriteLine("100");
            outfile.WriteLine("0");
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



    public Stats load_data()
    {
        string fileName = "/HeroStats.txt";
        string path = Application.persistentDataPath + fileName;
        string rawFileData;

        
        

        using (StreamReader inFile = File.OpenText(path))
        {
            rawFileData = inFile.ReadLine();
            curStats.level = Convert.ToInt32(rawFileData);
            rawFileData = inFile.ReadLine();
            curStats.curExp = (float)Convert.ToDouble(rawFileData);
            rawFileData = inFile.ReadLine();
            curStats.totalExp = (float)Convert.ToDouble(rawFileData);
            rawFileData = inFile.ReadLine();
            curStats.baseHp = Convert.ToInt32(rawFileData);
            rawFileData = inFile.ReadLine();
            curStats.baseDamage = (float)Convert.ToDouble(rawFileData);
            rawFileData = inFile.ReadLine();
            curStats.armor = (float)Convert.ToDouble(rawFileData);
            curStats.equipment = new string[5];
            
            for (int ctr = 0; ctr < 5; ctr++)
            {
                curStats.equipment[ctr] = inFile.ReadLine();
            }
			rawFileData = inFile.ReadLine();
			curStats.item_hp = Convert.ToInt32(rawFileData);
			rawFileData = inFile.ReadLine();
			curStats.item_damage = (float)Convert.ToDouble(rawFileData);
			rawFileData = inFile.ReadLine();
			curStats.item_energy = (float)Convert.ToDouble(rawFileData);
			rawFileData = inFile.ReadLine();
			curStats.item_penetration = (float)Convert.ToDouble(rawFileData);
			rawFileData = inFile.ReadLine();
			curStats.item_luck = (float)Convert.ToDouble(rawFileData);
        }
        return curStats;
    }

    public void equip_item(Item equipItem)
    {
        if (equipItem.itemID[2] == 'H')
        {
            curStats.equipment[0] = equipItem.itemID;
            curStats.baseHp += equipItem.hp;
            curStats.baseDamage += equipItem.damage;
            curStats.armor += equipItem.armor;
        }
        else if (equipItem.itemID[2] == 'W')
        {
            curStats.equipment[1] = equipItem.itemID;
            curStats.baseHp += equipItem.hp;
            curStats.baseDamage += equipItem.damage;
            curStats.armor += equipItem.armor;

        }
        else if (equipItem.itemID[2] == 'B')
        {
            curStats.equipment[2] = equipItem.itemID;
            curStats.baseHp += equipItem.hp;
            curStats.baseDamage += equipItem.damage;
            curStats.armor += equipItem.armor;
        }
    }

    public void remove_item(Item removeItem)
    {
        if (removeItem.itemID[2] == 'H')
        {
            curStats.equipment[0] = "000000";
            curStats.baseHp -= removeItem.hp;
            curStats.baseDamage -= removeItem.damage;
            curStats.armor -= removeItem.armor;
        }
        else if (removeItem.itemID[2] == 'W')
        {
            curStats.equipment[1] = "000000";
            curStats.baseHp -= removeItem.hp;
            curStats.baseDamage -= removeItem.damage;
            curStats.armor -= removeItem.armor;
        }
        else if (removeItem.itemID[2] == 'B')
        {
            curStats.equipment[2] = "000000";
            curStats.baseHp -= removeItem.hp;
            curStats.baseDamage -= removeItem.damage;
            curStats.armor -= removeItem.armor;
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
		item_stat.item_hp = curStats.item_hp;
		item_stat.item_armor = curStats.item_armor;
		item_stat.item_damage = curStats.item_damage;
		item_stat.item_energy = curStats.item_energy;
		item_stat.item_penetration = curStats.item_penetration;
		item_stat.item_luck = curStats.item_luck;
		
		return item_stat;
	}
}
