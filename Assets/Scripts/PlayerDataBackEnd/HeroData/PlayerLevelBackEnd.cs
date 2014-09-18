using UnityEngine;
using System.Collections;
using System.IO;

public class PlayerLevelBackEnd {
    int playerLevel = 0;
    int playerExperience;
    string heroMechID;

    public int get_player_level()
    {
        return playerLevel;
    }

    public int get_player_experience()
    {
        return playerExperience;
    }

    public void create_new_player(string heroMechIDInput)
    {
        string fileName = "/" + heroMechIDInput + "/HeroLevelData.txt";
        string dataPath = Application.persistentDataPath + fileName;

        using (StreamWriter outFile = File.CreateText(dataPath))
        {
            outFile.WriteLine("1");
            outFile.WriteLine("0");
        }
    }

    public void load_file(string heroMechIDInput)
    {

        string fileName = "/" + heroMechIDInput + "/HeroLevelData.txt";
        string dataPath = Application.persistentDataPath + fileName;

        if (!File.Exists(dataPath))
        {
            create_new_player(heroMechIDInput);
        }

        using (StreamReader fileAcc = File.OpenText(dataPath))
        {
            playerLevel = System.Convert.ToInt32(fileAcc.ReadLine());
            playerExperience = System.Convert.ToInt32(fileAcc.ReadLine());
        }
    }

    public void save_file(int level, int experience, string heroMechIDInput)
    {
        string fileName = "/" + heroMechIDInput + "/HeroLevelData.txt";
        string dataPath = Application.persistentDataPath + fileName;

        using (StreamWriter outFile = File.CreateText(dataPath))
        {
            outFile.WriteLine(level);
            outFile.WriteLine(experience);
        }
    }
}
