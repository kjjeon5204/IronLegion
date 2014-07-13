using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

public class FirstLogIn : MonoBehaviour {
    public TextAsset heroStatsFile;
    public TextAsset inventoryFile;
    public TextAsset mapDataFile;
	public TextAsset heroDataFile;
	public TextAsset abilityDataFile;
    public int myFlag;//non zero value will always cause all data to be reset.
    

    bool check_data()
    {
        string path = Application.persistentDataPath;
        if (!File.Exists(path + "/HeroStats.txt"))
        {
            return false;
        }
        if (!File.Exists(path + "/inventory.txt"))
        {
            return false;
        }
        if (!File.Exists(path + "/MapData.txt"))
        {
            return false;
        }
		if (!File.Exists (path + "/HeroData.txt")) 
		{
			return false;
		}
		if (!File.Exists (path + "/AbilityData.txt"))
		{
			return false;
		}
        return true;
    }


    void create_data_file()
    {
        string path = Application.persistentDataPath;
        using (StreamWriter outfile = File.CreateText(path + "/HeroStats.txt"))
        {
            outfile.Write(heroStatsFile.text);
        }
        using (StreamWriter outfile = File.CreateText(path + "/Inventory.txt"))
        {
            outfile.Write(inventoryFile.text);
        }
        using (StreamWriter outfile = File.CreateText(path + "/MapData.txt"))
        {
            outfile.Write(mapDataFile.text);
		}
		using (StreamWriter outfile = File.CreateText(path + "/HeroData.txt")) 
		{
			outfile.Write(heroDataFile.text);
		}
		using (StreamWriter outfile = File.CreateText(path + "/AbilityData.txt"))
		{
			outfile.Write(abilityDataFile.text);
		}
    }

	// Use this for initialization
	void Start () {
        if (!check_data())
        {
            create_data_file();
        }
        if (myFlag != 0)
        {
            create_data_file();
        }
	}
	
	public void reset_data() {
		create_data_file();
	}
	
}
