using UnityEngine;
using System.Collections;
using System.IO;
using System;

public struct TutorialStatus
{
    public int combatTutorial;
    public int hangerTutorial;
    public int overWorldTutorial;
}




public class PlayerDataReader {
    TutorialStatus curStatus;
    string curVersionNumber;

    public string get_next_line(StreamReader textToRead)
    {
        string rawString;
        rawString = textToRead.ReadLine();
        while (rawString[0] == '#')
        {
            rawString = textToRead.ReadLine();
        }
        return rawString;
    }

    public void save_data()
    {
        string dataPath = Application.persistentDataPath + "/PlayerData.txt";
    }

    public PlayerDataReader() {
        string dataPath = Application.persistentDataPath + "/PlayerData.txt";
        if (File.Exists(dataPath))
        {
            using (StreamReader inFile = File.OpenText(dataPath))
            {
                string rawString = get_next_line(inFile);
                curVersionNumber = rawString;
                //combat tutorial
                rawString = get_next_line(inFile);
                if (rawString == "1")
                    curStatus.combatTutorial = 1;
                else
                    curStatus.combatTutorial = 0;

                //hanger tutorial
                rawString = get_next_line(inFile);
                if (rawString == "1")
                    curStatus.hangerTutorial = 1;
                else
                    curStatus.hangerTutorial = 0;

                //overworld tutorial
                rawString = get_next_line(inFile);
                if (rawString == "1")
                    curStatus.overWorldTutorial = 1;
                else
                    curStatus.overWorldTutorial = 0;
            }
        }
        else
        {
            curVersionNumber = "Unknown";
            curStatus.combatTutorial = 1;
            curStatus.hangerTutorial = 1;
            curStatus.overWorldTutorial = 1;
        }
    }

    public bool combat_tutorial_played()
    {
        if (curStatus.combatTutorial == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool hanger_tutorial_played()
    {
        if (curStatus.hangerTutorial == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool overworld_tutorial_played()
    {
        if (curStatus.overWorldTutorial == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool check_event_played(string eventID)
    {
        return false;
    }
}
