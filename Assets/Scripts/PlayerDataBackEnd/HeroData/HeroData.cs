using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

public struct HeroAbilities {
	public string ability_name;
	public int ability_number;
}


public class HeroData {
	HeroAbilities[] hero;
    IDictionary<string, HeroAbilities> heroAbilityEquipAcc = new Dictionary<string, HeroAbilities>();
	
	public HeroData()
	{
	}
	
	public void save_data(/*string heroID*/) {
		//string fileName = "/" + heroID + "/HeroData.txt";
        string fileName = "/HeroData.txt";
		string path = Application.persistentDataPath + fileName;
		
		using (StreamWriter outfile = File.CreateText(path))
		{
			for (int i = 0; i < 8; i++)
			{
				outfile.WriteLine(hero[i].ability_name);
				outfile.WriteLine(hero[i].ability_number);
			}
		}
	}
	
	public void create_data(/*string heroID*/) {
		//string fileName = "/" + heroID + "/HeroData.txt";
        string fileName = "/HeroData.txt";
		string path = Application.persistentDataPath + fileName;
		
		using (StreamWriter outfile = File.CreateText(path)) {
			outfile.WriteLine("GATLING_GUN");
			outfile.WriteLine(0);
			outfile.WriteLine("SHATTER"); 
			outfile.WriteLine(1);
			outfile.WriteLine("BLUTSAUGER");
			outfile.WriteLine(2);
			outfile.WriteLine("ENERGY_BLADE");
			outfile.WriteLine(3);
			//Up is close range :: down is long range
			outfile.WriteLine("SHOTGUN");
			outfile.WriteLine(100);
			outfile.WriteLine("BARRAGE");
			outfile.WriteLine(101);
			outfile.WriteLine("AEGIS");
			outfile.WriteLine(102);
			outfile.WriteLine("BEAM_CANNON");
			outfile.WriteLine(103);
		}
	}
	
	public string[] load_data(/*string heroID*/) {
		//string fileName = "/" + heroID + "/HeroData.txt";
        string fileName = "/HeroData.txt";
		string path = Application.persistentDataPath + fileName;
		string rawFileData;
		
		using (StreamReader inFile = File.OpenText(path))
		{
			hero = new HeroAbilities[8];
			for (int i = 0; i < 8; i++)
			{
				hero[i].ability_name = inFile.ReadLine();
				
				rawFileData = inFile.ReadLine();
				if (rawFileData == "-")
				hero[i].ability_number = -1;
				else
				hero[i].ability_number = Convert.ToInt32(rawFileData);
                heroAbilityEquipAcc[hero[i].ability_name] =
                    hero[i];
			}
		}
		string[] names = new string[8];
		for (int i = 0; i < 8; i++)
		names[i] = hero[i].ability_name;
		return names;
	}
	
	public bool CheckAbilityList()
	{
		for (int i = 0; i < 8; i++)
		{
			if (hero[i].ability_name == "-")
			return false;
		}
		return true;
	}
	
	public bool SetAbility(int index, int id)
	{
		int switch_index = index;
		string temp_name = "";
		int temp_num;
	
		if (id < 100 && index >= 4)
		return false;
		else if (id >= 100 && index < 4)
		return false;
		else if (id == -1)
		return false;
		
		for (int i = 0; i < 8; i++)
		{
			if (id == hero[i].ability_number && i != index)
			{
				switch_index = i;
				
			}
		}
        temp_name = hero[switch_index].ability_name;
        hero[switch_index].ability_name = hero[index].ability_name;
        hero[index].ability_name = temp_name;

        temp_num = hero[switch_index].ability_number;
        hero[switch_index].ability_number = hero[index].ability_number;
        hero[index].ability_number = temp_num;
		return true;
	}
	
	public string ReturnAbilityName(int index)
	{
		return hero[index].ability_name;
	}
	
	public int ReturnAbilityID(int index)
	{
		return hero[index].ability_number;
	}
}
