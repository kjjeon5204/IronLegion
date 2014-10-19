using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public struct TutorialStatus
{
    public int combatTutorial;
    public int hangerTutorial;
    public int overWorldTutorial;
}




public class PlayerDataReader {
    public struct EventProgressionData
    {
        public string eventID;
        public int recordAccNum;
        public bool eventPlayed;
    }

    TutorialStatus curStatus;
    string curVersionNumber;
    IList<string> textFileRecord = new List<string>();
    IList<EventProgressionData> eventList = new List<EventProgressionData>();
    IDictionary<string, EventProgressionData> eventTracker = 
        new Dictionary<string, EventProgressionData>();

    string persistentDataPath;

    public string get_next_line(StreamReader textToRead)
    {
        string rawString;
        rawString = textToRead.ReadLine();
        textFileRecord.Add(rawString);
        while (rawString.Length == 0 ||
            rawString[0] == '#' || rawString == null)
        {
            rawString = textToRead.ReadLine();
            textFileRecord.Add(rawString);
        }
        return rawString;
    }

    public PlayerDataReader(string input)
    {
        load_player_data_file();
    }

    public PlayerDataReader()
    {
        load_player_data_file();
    }

    public void load_player_data_file() {
        string dataPath = Application.persistentDataPath + "/PlayerData.txt";
        if (File.Exists(dataPath))
        {

            using (StreamReader inFile = File.OpenText(dataPath))
            {


                string rawString = get_next_line(inFile);
                curVersionNumber = rawString;
                while (rawString != "END")
                {
                    EventProgressionData curProgress = new EventProgressionData();
                    curProgress.eventID = rawString;
                    rawString = get_next_line(inFile);
                    curProgress.recordAccNum = textFileRecord.Count - 1;
                    if (rawString == "1")
                    {
                        curProgress.eventPlayed = false;
                    }
                    else if (rawString == "0")
                    {
                        curProgress.eventPlayed = true;
                    }
                    else
                    {
                        curProgress.eventPlayed = false;
                    }
                    eventTracker[curProgress.eventID] = curProgress;
                }

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

    public void event_played(string eventID)
    {
        if (eventTracker.ContainsKey(eventID))
        {
            EventProgressionData myEvent = eventTracker[eventID];
            textFileRecord[myEvent.recordAccNum] = "0";
        }
        save_data();
    }


    //Returns false if even has not been played!
    public bool check_event_played(string eventID)
    {
        if (eventTracker.ContainsKey(eventID))
            return eventTracker[eventID].eventPlayed;
        else return false;
    }

    public void save_data()
    {
        string datapath = Application.persistentDataPath + "/PlayerData.txt";
        using (StreamWriter outfile = File.CreateText(datapath))
        {
            for (int ctr = 0; ctr < textFileRecord.Count; ctr++)
            {
                outfile.WriteLine(textFileRecord[ctr]);
            }
        }
    }
}
