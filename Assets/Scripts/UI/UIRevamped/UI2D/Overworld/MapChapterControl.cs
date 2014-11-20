using UnityEngine;
using System.Collections;
using System.IO;

public class MapChapterControl : MonoBehaviour {
    public MapTileManager[] chapterData;
    public GameObject leftButton;
    public GameObject rightButton;

    int curMapAccess;

    void enable_chapter(int chapter)
    {
        for (int ctr = 0; ctr < chapterData.Length; ctr++)
        {
            chapterData[ctr].gameObject.SetActive(false);
        }
        chapterData[chapter].gameObject.SetActive(true);
        if (chapter <= 0)
        {
            leftButton.SetActive(false);
        }
        else
        {
            leftButton.SetActive(true);
        }

        if (chapter >= chapterData.Length - 1)
        {
            rightButton.SetActive(false);
        }
        else
        {
            rightButton.SetActive(true);
        }
    }

    public void increment_chapter_ctr()
    {
        curMapAccess++;
        enable_chapter(curMapAccess - 1);
    }

    public void decrement_chapter_ctr()
    {
        curMapAccess--;
        enable_chapter(curMapAccess - 1);
    }

    // Use this for initialization
    void Start()
    {
        string dataPath = Application.persistentDataPath + "MapTransferData.txt";
        if (File.Exists(dataPath))
        {
            using (StreamReader infile = File.OpenText(dataPath))
            {
                curMapAccess = System.Convert.ToInt32(infile.ReadLine()[1]);
            }
        }
        else
        {
            curMapAccess = 1;
        }
        enable_chapter(curMapAccess - 1);
	}
}
