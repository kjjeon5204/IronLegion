using UnityEngine;
using System.Collections;


[System.Serializable]
public class EngageData
{
    public int levelNum;
    public Material skyBox;
    public CustomRenderSettings fogSettings;
    public WaveData[] waveData;
    public float experience;
    public int lootableItemTier;
    public int creditReceived;
    public GameObject playerStartPos;
}

[System.Serializable]
public struct CustomRenderSettings
{
    public bool fogEnabled;
    public Color fogColor;
    public float fogStartDistance;
    public float fogEndDistance;
}

[System.Serializable]
public class WaveData
{
    public bool loadBeforeStory;
    public GameObject storyObjectStart;
    public GameObject storyObjectEnd;
    public BattleType battleType;

    //Used by regular battle
    public EnemyData[] requiredEnemy;
    public EnemyData[] randomEnemy;
    public int randomEnemyCount;

    //Used by boss battle
    public GameObject[] availableEnemy;
}

[System.Serializable]
public struct EnemyData
{
    public GameObject enemyUnit;
    public int level;
}




public struct WaveBattleRunData
{
    public bool eventRunPhase;
    public bool loadBeforeStory;
    public bool storyInitialized;
    public bool storyEnded;
    public bool waveEnded;
    public BattleStory thisStoryStart;
    public BattleStory thisStoryEnd;
    public GameObject[] enemyList;
    public Character[] enemyListScript;
    //enemyListScript + playerScript
    public Character[] characterScriptCollection;
    public GameObject player;
    public MainChar playerScript;

}




[System.Serializable]
public enum BattleType
{
    REGULAR,
    AERIAL,
    BOSS
}


public struct PlayerLevelReadData
{
    public int level;
    public int expRequired;
    public int curExperience;
    public bool levelUp;
}

public class EventControls : MonoBehaviour {
    public bool tutorialStage = false;
    bool tutorialPhase;
    bool endBattle = false;
    bool waveReadyPhase;

    public EngageData curEngageData;
    WaveBattleRunData[] waveRunData;
    int curWave = 0;

    bool startPhaseEnded = true;
    MapData mapDataStorage;
    
	KeyCode userInput;
    string[] enemyQuery;
	GameObject[] enemies; /*List of possible enemies*/
	Character[] enemyScripts; /*Scripts of spawned enemies*/
	GameObject[] enemyList; /*List of spawned enemies*/
    GameObject player; /*Player model*/
    public MainChar playerScript;
    GameObject loadPlayer;
    GameObject loadEnvironment;
    int curEnemy;
    int curPlayerTarget;
    bool receiveGUIInput;
    GameObject boundaryObject;
    Bounds mapBoundary;

    public GameObject combatScriptObject;
    CombatScript combatScript;

    public GameObject advCombatGUI;

    public GameObject radar;
    Radar radarScript;

    MapChargeFlag[] mapChargeFlags;
    Character[] allCharacterScripts;
    int targetPathUpdater = 0;


    bool gamePaused = false;
    bool mapCleared = false;

	public int[] close_index() {
		int[] retVal = new int[4];
		for (int ctr = 0; ctr < 4; ctr ++) {
			retVal[ctr] = ctr;
		}
		return retVal;
	}

	public int[] far_index() {
		int[] retVal = new int[4];
		for (int ctr = 0; ctr < 4; ctr ++) {
			retVal[ctr] = ctr;
		}
		return retVal;
	}

    public void end_battle()
    {
        endBattle = true;
    }

    public void pause_game()
    {
        Time.timeScale = 0.0f;
        gamePaused = true;
        MyProjectile[] bullets = FindObjectsOfType<MyProjectile>();
        foreach (MyProjectile bullet in bullets)
        {
            bullet.pause_projectile(true);
        }
    }

    public void unpause_game()
    {
        Time.timeScale = 1.0f;
        gamePaused = false;
        MyProjectile[] bullets = FindObjectsOfType<MyProjectile>();
        foreach (MyProjectile bullet in bullets)
        {
            bullet.pause_projectile(false);
        }
    }


    public bool is_win()
    {
        if (check_wave_ended(waveRunData[curWave]) && 
		    curWave >= waveRunData.Length && waveRunData[curWave].thisStoryEnd == null)
        {
            
            end_battle_win();
            enabled = false;
            
            return true;
        }
        return false;
    }


    void end_battle_win()
    {
        //end battle
        combatScriptObject.SetActive(true);
        MapData curMap = new MapData(System.Convert.ToInt32(gameObject.name[1].ToString()));
        curMap.clear_level(curEngageData.levelNum);

        combatScript.enable_end_battle_window(curEngageData.creditReceived,
            playerScript.player_add_experience((int)curEngageData.experience),
            true, curEngageData.lootableItemTier);


        mapCleared = true;

    }
   
    Vector3 generate_spawn_coordinate(BattleType battleType) {
		Vector3 spawnPoint = Vector3.zero;
		int xSide = Random.Range (0,2);
        //Debug.Log("Appear poll " + xSide);
		spawnPoint.x = Random.Range (mapBoundary.center.x - 1.0f * mapBoundary.extents.x,
			                             mapBoundary.center.x + 1.0f * mapBoundary.extents.x);

		spawnPoint.z = Random.Range (mapBoundary.center.z - 1.0f * mapBoundary.extents.z,
		                             mapBoundary.center.z + 1.0f * mapBoundary.extents.z);
        if (battleType == BattleType.AERIAL)
        {
            spawnPoint.y = Random.Range(-5.0f, 5.0f);
        }
        
        
        return spawnPoint;
	}
	

    


	struct GameObjectTracker {
		string nextState;
		//mainCharacter 
	}
	Character mainCharacter;


    void wave_ready_phase(WaveBattleRunData instatiateWave)
    {
        Debug.Log("Prep wave");
        for (int ctr = 0; ctr < instatiateWave.enemyList.Length; ctr++)
        {
            
            instatiateWave.enemyList[ctr].SetActive(true);
            instatiateWave.enemyListScript[ctr].set_player(player);
            instatiateWave.enemyListScript[ctr].set_target(player);
            instatiateWave.enemyListScript[ctr].manual_start();
        }
        playerScript.enemyList = instatiateWave.enemyListScript;
        combatScriptObject.SetActive(false);
        waveReadyPhase = true;
    }

    bool all_landed(WaveBattleRunData checkWave)
    {
        for (int ctr = 0; ctr < checkWave.enemyListScript.Length; ctr++)
        {
            if (!checkWave.enemyListScript[ctr].is_ready())
            {
                return false;
            }
        }
        start_wave(checkWave);
        return true;
    }




    void start_wave(WaveBattleRunData instatiateWave)
    {
        Debug.Log("Wave started!");
        waveReadyPhase = false;
        combatScriptObject.SetActive(true);
        /*
        for (int ctr = 0; ctr < instatiateWave.enemyList.Length; ctr++)
        {
            instatiateWave.enemyList[ctr].SetActive(true);
            instatiateWave.enemyListScript[ctr].set_player(player);
            instatiateWave.enemyListScript[ctr].manual_start();
        }
         */ 
		playerScript.enemyList = instatiateWave.enemyListScript;

		playerScript.set_target(instatiateWave.enemyList[0]);
        radarScript.initialize_radar(waveRunData[curWave].enemyList, player);

        playerScript.enable_auto_adjust();

        if (curWave == 0 && tutorialStage)
        {
            enable_tutorial(curWave);
        }
        else if (curWave == 1 && tutorialStage)
        {
            enable_tutorial(curWave);
        }
        else if (curWave == 2 && tutorialStage)
        {
            enable_tutorial(curWave);
        }
    }

    bool check_wave_ended(WaveBattleRunData instantiateWave)
    {
        for (int ctr = 0; ctr < instantiateWave.enemyList.Length; ctr++)
        {
            if (instantiateWave.enemyListScript[ctr].return_cur_stats().baseHp > 0)
            {
                return false;
            }
        }
        for (int ctr = 0; ctr < instantiateWave.enemyList.Length; ctr++)
        {
            if (instantiateWave.enemyList[ctr] != null)
            {
                Destroy(instantiateWave.enemyList[ctr]);
            }
        }
        return true;
    }


    // Use this for initialization
    void Start()
    {
		RenderSettings.skybox = curEngageData.skyBox;
        RenderSettings.fog = curEngageData.fogSettings.fogEnabled;
        RenderSettings.fogColor = curEngageData.fogSettings.fogColor;
        RenderSettings.fogStartDistance = curEngageData.fogSettings.fogStartDistance;
        RenderSettings.fogEndDistance = curEngageData.fogSettings.fogEndDistance;

        combatScriptObject = GameObject.Find("Camera");
        radar = GameObject.Find("RadarDisplay");

        boundaryObject = transform.FindChild("BattleSceneBoundary").gameObject;
        mapBoundary = boundaryObject.collider.bounds;
        GetComponent<MapFeatures>().intialize_script();
        mapChargeFlags = GetComponent<MapFeatures>().flagScripts;

        waveRunData = new WaveBattleRunData[curEngageData.waveData.Length];

        Transform playerStartPos = curEngageData.playerStartPos.transform;
        loadPlayer = (GameObject)Resources.Load("heromech");
        player = (GameObject)Instantiate(loadPlayer, playerStartPos.position, playerStartPos.rotation);
        playerScript = player.GetComponent<MainChar>();
        playerScript.battleBoundary = boundaryObject.collider;
        playerScript.worldObject = gameObject;
		playerScript.set_battle_type(curEngageData.waveData[0].battleType);
        playerScript.manual_start();


        radarScript = radar.GetComponent<Radar>();



        //Set up all waves
        for (int waveCtr = 0; waveCtr < waveRunData.Length; waveCtr++)
        {
            //Storyline parser
            waveRunData[waveCtr].waveEnded = false;
            if (curEngageData.waveData[waveCtr].storyObjectStart != null)
            {
                waveRunData[waveCtr].thisStoryStart = curEngageData.waveData[waveCtr].storyObjectStart.
                    GetComponent<BattleStory>();

                waveRunData[waveCtr].loadBeforeStory = curEngageData.waveData[waveCtr].loadBeforeStory;
                waveRunData[waveCtr].storyEnded = false;
                waveRunData[waveCtr].eventRunPhase = true;
                waveRunData[waveCtr].storyInitialized = false;
            }
            else
            {
                waveRunData[waveCtr].eventRunPhase = false;
                waveRunData[waveCtr].storyEnded = false;
            }


            if (curEngageData.waveData[waveCtr].storyObjectEnd != null)
            {
                waveRunData[waveCtr].thisStoryEnd = curEngageData.waveData[waveCtr].storyObjectEnd.
                    GetComponent<BattleStory>();
            }

            waveRunData[waveCtr].player = player;
            waveRunData[waveCtr].playerScript = playerScript;
            if (curEngageData.waveData[waveCtr].battleType == BattleType.REGULAR ||
                curEngageData.waveData[waveCtr].battleType == BattleType.AERIAL)
            {
                //Create must enemy
                waveRunData[waveCtr].enemyList = new GameObject[curEngageData.waveData[waveCtr].requiredEnemy.Length +
                    curEngageData.waveData[waveCtr].randomEnemyCount];
                waveRunData[waveCtr].enemyListScript = new Character[waveRunData[waveCtr].enemyList.Length];

                waveRunData[waveCtr].characterScriptCollection = new Character[waveRunData[waveCtr].enemyList.Length + 1];
                waveRunData[waveCtr].characterScriptCollection[0] = playerScript;


                int enemyStoreCtr = 0;
                for (int enemyCtr = 0; enemyCtr < curEngageData.waveData[waveCtr].requiredEnemy.Length; enemyCtr++)
                {
                    waveRunData[waveCtr].enemyList[enemyStoreCtr] = (GameObject)Instantiate(
                        curEngageData.waveData[waveCtr].requiredEnemy[enemyCtr].enemyUnit,
                        generate_spawn_coordinate(curEngageData.waveData[waveCtr].battleType),
                        Quaternion.identity);

                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr] =
                        waveRunData[waveCtr].enemyList[enemyStoreCtr].GetComponent<Character>();

                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr].mapFlag = mapChargeFlags[0];
                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr].set_enemy_unit_index(enemyStoreCtr);
                    //Set level
                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr].set_level
                        (curEngageData.waveData[waveCtr].requiredEnemy[enemyCtr].level);
                    waveRunData[waveCtr].characterScriptCollection[enemyStoreCtr + 1] = waveRunData[waveCtr].enemyListScript[enemyStoreCtr];
                    waveRunData[waveCtr].enemyList[enemyStoreCtr].SetActive(false);
                    enemyStoreCtr++;
                }


                //Load random enemy from pool
                for (int enemyCtr = 0; enemyCtr < curEngageData.waveData[waveCtr].randomEnemyCount; enemyCtr++)
                {
                    int randPool = Random.Range(0, curEngageData.waveData[waveCtr].randomEnemy.Length);
                    waveRunData[waveCtr].enemyList[enemyStoreCtr] = (GameObject)Instantiate(
                        curEngageData.waveData[waveCtr].randomEnemy[randPool].enemyUnit,
                        generate_spawn_coordinate(curEngageData.waveData[waveCtr].battleType),
                        Quaternion.identity);

                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr] = waveRunData[waveCtr].enemyList[enemyStoreCtr].GetComponent<Character>();
                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr].mapFlag = mapChargeFlags[0];
                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr].set_enemy_unit_index(enemyStoreCtr);
                    //set level
                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr].set_level(curEngageData.waveData[waveCtr].randomEnemy[randPool].level);

                    waveRunData[waveCtr].characterScriptCollection[enemyStoreCtr + 1] = waveRunData[waveCtr].enemyListScript[enemyStoreCtr];
                    waveRunData[waveCtr].enemyList[enemyStoreCtr].SetActive(false);
                    enemyStoreCtr++;
                }

                for (int enemyCtr = 0; enemyCtr < waveRunData[waveCtr].enemyList.Length; enemyCtr++)
                {
                    waveRunData[waveCtr].enemyListScript[enemyCtr].battleBoundary = boundaryObject.collider;
                    waveRunData[waveCtr].enemyListScript[enemyCtr].targets = waveRunData[waveCtr].characterScriptCollection;
                }
            }
            else if (curEngageData.waveData[waveCtr].battleType == BattleType.BOSS)
            {
                waveRunData[waveCtr].enemyList = new GameObject[curEngageData.waveData[waveCtr].requiredEnemy.Length];
                waveRunData[waveCtr].enemyListScript = new Character[curEngageData.waveData[waveCtr].requiredEnemy.Length];
                for (int enemyCtr = 0; enemyCtr < waveRunData[waveCtr].enemyList.Length; enemyCtr++)
                {
                    waveRunData[waveCtr].enemyList[enemyCtr] = curEngageData.waveData[waveCtr].requiredEnemy[enemyCtr].enemyUnit;
                    waveRunData[waveCtr].enemyListScript[enemyCtr] = waveRunData[waveCtr].enemyList[enemyCtr].GetComponent<Character>();
                    waveRunData[waveCtr].enemyListScript[enemyCtr].set_enemy_unit_index(enemyCtr);
                }

            }
        }

        curWave = 0;
        //First wave initialize
        if (waveRunData[0].thisStoryStart != null)
        {
            Debug.Log("Has story");
            if (waveRunData[0].loadBeforeStory == true)
                wave_ready_phase(waveRunData[curWave]);
            waveRunData[0].eventRunPhase = true;
            waveRunData[0].waveEnded = false;
            waveRunData[0].storyInitialized = false;
        }
        else
        {
            //start_wave(waveRunData[0]);
            Debug.Log("No story");
            wave_ready_phase(waveRunData[0]);
            waveRunData[0].eventRunPhase = false;
            waveRunData[0].waveEnded = false;
            waveRunData[0].storyInitialized = false;
        }

        combatScript = combatScriptObject.GetComponent<CombatScript>();
        combatScript.eventControlObject = gameObject;
        combatScript.initialize_script();
        boundaryObject.SetActive(false);

	}

    void target_path_updater()
    {
        waveRunData[curWave].enemyListScript[targetPathUpdater].modifyPath = false;
        targetPathUpdater++;
        if (targetPathUpdater >= waveRunData[curWave].enemyListScript.Length)
        {
            targetPathUpdater = 0;
        }
        waveRunData[curWave].enemyListScript[targetPathUpdater].modifyPath = true;
    }

    void enable_tutorial(int tutorialStep)
    {
        tutorialPhase = true;
        if (tutorialStep == 0)
        {
            combatScript.activate_skill_button_tutorial(true);
            pause_game();
        }
        else if (tutorialStep == 1)
        {
            combatScript.activate_state_tutorial(true);
            pause_game();
        }
        else if (tutorialStep == 2)
        {
            combatScript.activate_left_right_dodge_tutorial(true);
            pause_game();
        }
    }

    void disable_tutorial()
    {
        if (Input.GetKeyDown(KeyCode.K) || Input.touchCount > 0)
        {
            if (curWave == 0)
            {
                combatScript.activate_skill_button_tutorial(false);
                unpause_game();
            }
            else if (curWave == 1)
            {
                combatScript.activate_state_tutorial(false);
                unpause_game();
            }
            else if (curWave == 2)
            {
                combatScript.activate_left_right_dodge_tutorial(false);
                unpause_game();
            }
            tutorialPhase = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialPhase == true)
            disable_tutorial();
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gamePaused == false)
                pause_game();
            else unpause_game();
        }
        if (playerScript.return_cur_stats().baseHp <= 0.0f || endBattle == true)
        {
            enabled = false;
            Application.LoadLevel(0);
        }
        //Checking wave clear/win conditions
        if (mapCleared == false && curWave < waveRunData.Length)
        {
            if (waveRunData[curWave].eventRunPhase == false)
            {
                //targetpath update

                playerScript.enable_player_camera();

                if (combatScriptObject.activeInHierarchy == false
                    && waveReadyPhase == false)
                {
                    combatScriptObject.SetActive(true);
                }

                //Check for last wave/win condition
                if (curWave >= waveRunData.Length)
                {
                    Debug.Log("Current wave counter: " + curWave);
                    end_battle_win();
                    enabled = false;
                    return;
                }

                if (check_wave_ended(waveRunData[curWave]))
                {
                    if (!is_win())
                    {
                        //If there is an ending storyline
                        if (waveRunData[curWave].thisStoryEnd != null)
                        {
                            waveRunData[curWave].eventRunPhase = true;
                            waveRunData[curWave].waveEnded = true;
                            waveRunData[curWave].storyInitialized = false;
                        }
                        //If there is no ending storyline
                        else
                        {
                            curWave++;
                            if (curWave >= waveRunData.Length)
                            {
                                //end battle
                                end_battle_win();
                                return;
                            }
                            Debug.Log("Current wave number: " + curWave);
                            //If there is a beginning storyline at next wave.
                            if (waveRunData[curWave].thisStoryStart != null)
                            {
                                if (waveRunData[curWave].loadBeforeStory == true)
                                {
                                    waveRunData[curWave].eventRunPhase = true;
                                    waveRunData[curWave].storyInitialized = false;
                                    waveRunData[curWave].waveEnded = false;
                                }
                                else
                                {
                                    wave_ready_phase(waveRunData[curWave]);
                                    waveRunData[curWave].eventRunPhase = true;
                                    waveRunData[curWave].storyInitialized = false;
                                    waveRunData[curWave].waveEnded = false;
                                }
                            }
                            //If there is no beginning storyline at next wave.
                            else
                            {

                                wave_ready_phase(waveRunData[curWave]);
                                waveRunData[curWave].eventRunPhase = false;
                                waveRunData[curWave].storyInitialized = false;
                                waveRunData[curWave].waveEnded = false;
                            }

                        }
                        return;
                    }

                }


                if (waveReadyPhase == true)
                {
                    playerScript.animation.CrossFade("idle");
                    Debug.Log("wave not ready");
                    all_landed(waveRunData[curWave]);
                }


                
                /*Event Handle*/
                if (gamePaused == false && waveReadyPhase == false)
                {
                    playerScript.manual_update();

                    for (int ctr = 0; ctr < waveRunData[curWave].enemyList.Length; ctr++)
                    {
                        if (waveRunData[curWave].enemyList[ctr] != null)
                        {
                            waveRunData[curWave].enemyListScript[ctr].manual_update();
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Run story");
                if (combatScriptObject.activeInHierarchy == true)
                {
                    combatScriptObject.SetActive(false);
                }
                playerScript.disable_player_camera();
                if (waveRunData[curWave].waveEnded == false &&
                    waveRunData[curWave].storyEnded == false)
                {
                    if (waveRunData[curWave].storyInitialized == false)
                    {
                        waveRunData[curWave].thisStoryStart.gameObject.SetActive(true);
                        waveRunData[curWave].thisStoryStart.manual_start();
                        waveRunData[curWave].storyInitialized = true;
                    }
                    waveRunData[curWave].storyEnded =
                        waveRunData[curWave].thisStoryStart.manual_update();
                }
                else if (waveRunData[curWave].waveEnded == true &&
                    waveRunData[curWave].storyEnded == false)
                {
                    if (waveRunData[curWave].storyInitialized == false)
                    {
                        waveRunData[curWave].thisStoryEnd.manual_start();
                        waveRunData[curWave].storyInitialized = true;
                    }
                    waveRunData[curWave].storyEnded =
                        waveRunData[curWave].thisStoryEnd.manual_update();
                }
                else
                {
                    if (waveRunData[curWave].waveEnded == true)
                    {
                        curWave++;
                        waveRunData[curWave].eventRunPhase = false;
                        waveRunData[curWave].storyEnded = true;
                    }
                    else
                    {
                        waveRunData[curWave].eventRunPhase = false;
                        waveRunData[curWave].storyEnded = false;
                        if (waveRunData[curWave].loadBeforeStory == false)
                        {
                            wave_ready_phase(waveRunData[curWave]);
                            //start_wave(waveRunData[curWave]);
                        }
                    }
                    if (waveRunData[curWave].waveEnded == false)
                        waveRunData[curWave].thisStoryStart.gameObject.SetActive(false);
                    else waveRunData[curWave].thisStoryEnd.gameObject.SetActive(false);
                }
            }
        }
	}
}
