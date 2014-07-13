using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

public struct Abilities {
	public string name;
	public int id;
	public bool isClose;
	public string target;
	public int damage_percent;
	public int frequency;
	public int deviation;
	public int hp_modifier;
	public int hp_leech;
	public int armor_modifier;
	public int armor_mod_duration;
	public int cooldown;
	public int level;
}

public class AbilityData {
	public int num_of_close;
	public int num_of_far;
	Abilities[] close;
	Abilities[] far;
	
	
	public AbilityData()
	{
	}
	
	public void save_data() {
		string fileName = "/AbilityData.txt";
		string path = Application.persistentDataPath + fileName;
		
		using (StreamWriter outfile = File.CreateText(path))
		{
			outfile.WriteLine(num_of_close);
			outfile.WriteLine(num_of_far);
			for (int i = 0; i < num_of_close; i++)
			{
				outfile.WriteLine(close[i].name);
				outfile.WriteLine(close[i].id);
				if (close[i].isClose)
				outfile.WriteLine("true");
				else
				outfile.WriteLine("false");
				outfile.WriteLine(close[i].target);
				outfile.WriteLine(close[i].damage_percent);
				outfile.WriteLine(close[i].frequency);
				outfile.WriteLine(close[i].deviation);
				outfile.WriteLine(close[i].hp_modifier);
				outfile.WriteLine(close[i].hp_leech);
				outfile.WriteLine(close[i].armor_modifier);
				outfile.WriteLine(close[i].armor_mod_duration);
				outfile.WriteLine(close[i].cooldown);
				outfile.WriteLine(close[i].level);
			}
			outfile.WriteLine("-----------------------------------------------");
			for (int i = 0; i < num_of_far; i++)
			{
				outfile.WriteLine(far[i].name);
				outfile.WriteLine(far[i].id);
				if (far[i].isClose)
				outfile.WriteLine("true");
				else
				outfile.WriteLine("false");
				outfile.WriteLine(far[i].target);
				outfile.WriteLine(far[i].damage_percent);
				outfile.WriteLine(far[i].frequency);
				outfile.WriteLine(far[i].deviation);
				outfile.WriteLine(far[i].hp_modifier);
				outfile.WriteLine(far[i].hp_leech);
				outfile.WriteLine(far[i].armor_modifier);
				outfile.WriteLine(far[i].armor_mod_duration);
				outfile.WriteLine(far[i].cooldown);
				outfile.WriteLine(far[i].level);
			}
		}
	}
	
	public void load_data() {
		string fileName = "/AbilityData.txt";
		string path = Application.persistentDataPath + fileName;
		string rawFileData;
		
		using (StreamReader inFile = File.OpenText(path))
		{
			rawFileData = inFile.ReadLine();
			num_of_close = Convert.ToInt32(rawFileData);
			
			close = new Abilities[num_of_close];
			rawFileData = inFile.ReadLine();
			num_of_far = Convert.ToInt32(rawFileData);
			
			far = new Abilities[num_of_far];
			
			for (int i = 0; i < num_of_close; i++)
			{
				close[i].name = inFile.ReadLine();
				rawFileData = inFile.ReadLine();
				close[i].id = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				if (rawFileData == "true")
					close[i].isClose = true;
				else
					close[i].isClose = false;
				close[i].target = inFile.ReadLine();
				rawFileData = inFile.ReadLine();
				close[i].damage_percent = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				close[i].frequency = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				close[i].deviation = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				close[i].hp_modifier = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				close[i].hp_leech = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				close[i].armor_modifier = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				close[i].armor_mod_duration = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				close[i].cooldown = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				close[i].level = Convert.ToInt32(rawFileData);
				
			}
			rawFileData = inFile.ReadLine();
			for (int i = 0; i < num_of_far; i++)
			{
				far[i].name = inFile.ReadLine();
				
				rawFileData = inFile.ReadLine();
				far[i].id = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				if (rawFileData == "true")
					far[i].isClose = true;
				else
					far[i].isClose = false;
					
				far[i].target = inFile.ReadLine();
				
				rawFileData = inFile.ReadLine();
				far[i].damage_percent = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				far[i].frequency = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				far[i].deviation = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				far[i].hp_modifier = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				far[i].hp_leech = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				far[i].armor_modifier = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				far[i].armor_mod_duration = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				far[i].cooldown = Convert.ToInt32(rawFileData);
				
				rawFileData = inFile.ReadLine();
				far[i].level = Convert.ToInt32(rawFileData);
			}
		}
	}
	
	public int return_id(string name, bool isClose) {
		if (isClose)
		{
			for (int i = 0; i < num_of_close; i++)
			{
				if (close[i].name == name)
				return close[i].id;
			}
		}
		else
		{
			for (int i = 0; i < num_of_far; i++)
			{
				if(far[i].name == name)
				return far[i].id;
			}
		}
		return -1;
	}
	
	public Abilities GetAbilityInfo(int id)
	{
		if (id < 100)
		{
			return close[id];
		}
		else
		{
			return far[id-100];
		}
	}
}
