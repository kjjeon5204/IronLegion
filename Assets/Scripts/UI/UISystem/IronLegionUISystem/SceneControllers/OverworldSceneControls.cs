using UnityEngine;
using System.Collections;

public class OverworldSceneControls : SceneController {
    public enum UIState
    {
        MAIN,
        INITIALIZE_COMBAT
    }

    public UI2DController mapController; //Controls zoom and map movements on various finger input
    public UI2DController mapTilesController; //Controls touches on map tile input and initialization of confirm window
    public UI2DController uiLayoutController; // Controls inputs to UI overlay layout
    public UI2DController initiateCombatController; //Allows interaction with combat initializer

    public MapEngageWindow combatInitializeWindow;
    public MapChapterControl[] chapterList;

    public GameObject incrementChapterButton;
    public GameObject decrementChapterButton;

    int curChapterSelected;

    public void set_ui_state(UIState inputState)
    {
        if (inputState == UIState.INITIALIZE_COMBAT)
        {
            mapController.enabled = false;
            mapTilesController.enabled = false;
            uiLayoutController.enabled = false;
            initiateCombatController.enabled = false;
            combatInitializeWindow.gameObject.SetActive(true);
            string[] messageList = new string[0];
            combatInitializeWindow.set_engage_window(messageList, null);

        }
        if (inputState == UIState.MAIN)
        {
            mapTilesController.enabled = true;
            mapController.enabled = true;
            uiLayoutController.enabled = true;
            initiateCombatController.enabled = false;
            combatInitializeWindow.gameObject.SetActive(false);
        }
    }


    //MapChapter Manager
    MapChapterControl[] mapChapterTracker;
    SpriteModifier[] chapterTrackerDisplay;


    public GameObject chapterTrackerPrefab;

    /*This function is used to set chapter.*/
    public void set_chapter_counter(int chapter)
    {
        curChapterSelected = chapter;
        for (int ctr = 0; ctr < chapterList.Length; ctr++)
        {
            if (ctr == chapter)
            {
                chapterList[ctr].gameObject.SetActive(true);
                chapterTrackerDisplay[ctr].switch_sprite(0);
            }
            else
            {
                chapterList[ctr].gameObject.SetActive(false);
                chapterTrackerDisplay[ctr].switch_sprite(1);
            }
        }
        if (check_valid_increment(curChapterSelected))
        {
            incrementChapterButton.SetActive(true);
        }
        else
        {
            incrementChapterButton.SetActive(false);
        }

        if (check_valid_decrement(curChapterSelected))
        {
            decrementChapterButton.SetActive(true);
        }
        else
        {
            decrementChapterButton.SetActive(false);
        }
       
    }

    /*This function checks to see if incrementing chapter is valid from input chapter number. 
     Returns true if incrementing is valid*/
    public bool check_valid_increment(int chapterNum)
    {
        if (chapterNum == mapChapterTracker.Length - 1)
        {
            return false;
        }
        return true;
    }

    /*This function checks to see if decrementing chapter is valid from input chapter number.
     Returns true if decrementing is valid*/
    public bool check_valid_decrement(int chapterNum)
    {
        if (chapterNum == 0)
        {
            return false;
        }
        return true;
    }

    /*Used to increment chapter. This function does NOT check to see if the incrementing is
     valid.*/
    public void increment_chapter()
    {
        set_chapter_counter(++curChapterSelected);
    }

    /*Used to decrement chapter. This function does Not check to see if the decrementing is 
     valid*/
    public void decrement_chapter()
    {
        set_chapter_counter(--curChapterSelected);
    }

	// Use this for initialization
	void Start () {
        int chapterNum = 0;
        if (UserData.nextTargetScene != null)
        {
            chapterNum = System.Convert.ToInt32(UserData.nextTargetScene[1]);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
