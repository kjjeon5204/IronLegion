using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public struct SettingsList 
{
	public bool music;
	public int quality;
}



public class Settings {
	SettingsList options;
	
	public Settings() {
		load_data();
	}
	
	public void load_data() {
		string fileName = "/Settings.txt";
		string path = Application.persistentDataPath + fileName;
		string rawFileData;
		
		using (StreamReader inFile = File.OpenText(path))
		{
			options = new SettingsList();
			
			rawFileData = inFile.ReadLine();
			if (rawFileData == "false")
			options.music = false;
			else
			options.music = true;
			
			rawFileData = inFile.ReadLine();
			options.quality = Convert.ToInt32(rawFileData);
		}
	}
	
	public void save_data() {
		string fileName = "/HeroData.txt";
		string path = Application.persistentDataPath + fileName;
		
		using (StreamWriter outfile = File.CreateText(path))
		{
			if (options.music)
			outfile.WriteLine("true");
			else
			outfile.WriteLine("false");
		}
	}
	
	public SettingsList GetSettings() {
		return options;
	}
	
	public bool CheckMusic() {
		return options.music;
	}
	
	public int GetQuality() {
		return options.quality;
	}
	
	public void SetQuality(int qual) {
		options.quality = qual;
	}
	
	
}
