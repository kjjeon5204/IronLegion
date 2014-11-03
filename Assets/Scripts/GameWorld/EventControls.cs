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
    public int cogentumReceived;
    public float cogentumRandDropRate;
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
    public AudioSource waveThemeMusic;
    public bool loadBeforeStory;
    public GameObject storyObjectStart;
    public GameObject storyObjectEnd;
    public BattleType battleType;

    public Collider[] spawnPoints;

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
    public bool landingCraftActive;
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
    public int remainingEnemy;
}




[System.Serializable]
public enum BattleType
{
    REGULAR,
    AERIAL,
    AERIAL_MULTI_SPAWN_POINT,
    BOSS
}


public struct PlayerLevelReadData
{
    public int level;
    public int expRequired;
    public int curExperience;
    public bool levelUp;
}

public class EventControls : MonoBehaviour
{
    PlayerDataReader eventRecord;
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

    Character mainBody;

    bool faderActive = false;
    public ScreenFader myScreenFadeScript;
    int afterFadeAction;
    //0 = next wave
    //1 = end battle
    //2 = start cutscene (battle -> cutscene);
    //3 = start wave(cutscene -> battle)

    //Ally variables
    Character allyUnit;
    AllyData allyData;
    bool runPlayerSide;
    int enemyScriptRunCtr;

    PlayerMasterData playerMasterData;


    public void end_battle_fade_process()
    {
        end_battle_win();
    }

    public void wave_end_cutscene_fade_process()
    {

        playerScript.target_indicator_switch(false);
        combatScript.turn_off_combat_ui();
        playerScript.gameObject.SetActive(false);
        waveRunData[curWave].eventRunPhase = true;
        waveRunData[curWave].waveEnded = true;
        waveRunData[curWave].storyInitialized = true;
        waveRunData[curWave].thisStoryEnd.gameObject.SetActive(true);
        waveRunData[curWave].thisStoryEnd.manual_start();
        eventRecord.event_played(waveRunData[curWave].thisStoryEnd.cutSceneID);
        if (waveRunData[curWave].thisStoryEnd.customCutsceneAudio != null)
        {
            curEngageData.waveData[curWave].waveThemeMusic.Stop();
        }
        else if (curEngageData.waveData[curWave].waveThemeMusic.isPlaying == false)
        {
            curEngageData.waveData[curWave].waveThemeMusic.Play();
        }
    }


    public void wave_start_cutscene_fade_process()
    {
        playerScript.target_indicator_switch(false);
        playerScript.gameObject.SetActive(false);
        combatScript.turn_off_combat_ui();
        waveRunData[curWave].eventRunPhase = true;
        waveRunData[curWave].waveEnded = false;
        waveRunData[curWave].storyInitialized = false;
        waveRunData[curWave].thisStoryStart.gameObject.SetActive(true);
        waveRunData[curWave].thisStoryStart.manual_start();
        eventRecord.event_played(waveRunData[curWave].thisStoryStart.cutSceneID);

        if (waveRunData[curWave].thisStoryStart.customCutsceneAudio != null)
        {
            curEngageData.waveData[curWave].waveThemeMusic.Stop();
        }
        else if (curEngageData.waveData[curWave].waveThemeMusic.isPlaying == false)
        {
            curEngageData.waveData[curWave].waveThemeMusic.Play();
        }
    }

    public void wave_start_cutscene_end()
    {

        playerScript.target_indicator_switch(true);
        waveRunData[curWave].thisStoryStart.gameObject.SetActive(false);
        waveRunData[curWave].eventRunPhase = false;
        waveRunData[curWave].storyEnded = false;
        wave_ready_phase(waveRunData[curWave]);
    }

    public void start_battle_immediate()
    {

        playerScript.target_indicator_switch(true);
        wave_ready_phase(waveRunData[0]);
        waveRunData[0].eventRunPhase = false;
        waveRunData[0].waveEnded = false;
        waveRunData[0].storyInitialized = false;
    }


    public int[] close_index()
    {
        int[] retVal = new int[4];
        for (int ctr = 0; ctr < 4; ctr++)
        {
            retVal[ctr] = ctr;
        }
        return retVal;
    }

    public int[] far_index()
    {
        int[] retVal = new int[4];
        for (int ctr = 0; ctr < 4; ctr++)
        {
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
        playerMasterData.player_win_increment();

        playerScript.disable_targeting_indicator();

        AudioSource[] mapAudios = gameObject.GetComponents<AudioSource>();
        for (int ctr = 0; ctr < mapAudios.Length; ctr++)
        {
            if (mapAudios[ctr].isPlaying == true)
                mapAudios[ctr].Stop();
        }
        combatScriptObject.SetActive(true);
        int clearCount = playerMasterData.clear_level(System.Convert.ToInt32(gameObject.name[1].ToString()), curEngageData.levelNum);
        int cogentumReceived = 0;
        if (clearCount == 1)
        {
            cogentumReceived = curEngageData.cogentumReceived;
        }
        else
        {
            if (curEngageData.cogentumRandDropRate != 0)
            {
                float percentageMultiplier = 100.0f;
                int pollNum = Random.Range(0, (int)(100.0f
                    * percentageMultiplier));

                if (pollNum <= (int)(curEngageData.cogentumRandDropRate *
                    percentageMultiplier))
                {
                    cogentumReceived = curEngageData.cogentumReceived;
                }
            }
        }

        if (allyData.unitName != "NONE")
            allyData.exp += (int)curEngageData.experience;



        combatScript.enable_end_battle_window(curEngageData.creditReceived, cogentumReceived, 
            playerScript.player_add_experience((int)curEngageData.experience),
            true, curEngageData.lootableItemTier, allyData, allyUnit);


        mapCleared = true;

    }

    Vector3 generate_spawn_coordinate(BattleType battleType)
    {
        Vector3 spawnPoint = Vector3.zero;
        //int xSide = Random.Range (0,2);
        //Debug.Log("Appear poll " + xSide);
        if (battleType == BattleType.AERIAL || battleType == BattleType.REGULAR)
        {
            spawnPoint.x = Random.Range(mapBoundary.center.x - 1.0f * mapBoundary.extents.x,
                                         mapBoundary.center.x + 1.0f * mapBoundary.extents.x);

            spawnPoint.z = Random.Range(mapBoundary.center.z - 1.0f * mapBoundary.extents.z,
                                     mapBoundary.center.z + 1.0f * mapBoundary.extents.z);
        }

        return spawnPoint;
    }

    Vector3 generate_spawn_coordinate(BattleType battleType, Collider[] availablePoints)
    {
        Vector3 spawnPoint = Vector3.zero;
        //int xSide = Random.Range (0,2);
        //Debug.Log("Appear poll " + xSide);
        if (battleType == BattleType.AERIAL || battleType == BattleType.REGULAR)
        {
            spawnPoint.x = Random.Range(mapBoundary.center.x - 1.0f * mapBoundary.extents.x,
                                         mapBoundary.center.x + 1.0f * mapBoundary.extents.x);

            spawnPoint.z = Random.Range(mapBoundary.center.z - 1.0f * mapBoundary.extents.z,
                                     mapBoundary.center.z + 1.0f * mapBoundary.extents.z);
        }
        else if (battleType == BattleType.AERIAL_MULTI_SPAWN_POINT)
        {
            Debug.Log("Generated modified coordinate");
            int spawnAccessPoint = Random.Range(0, availablePoints.Length);
            Bounds spawnBounds = availablePoints[spawnAccessPoint].bounds;
            spawnPoint.x = Random.Range(spawnBounds.center.x - 1.0f * spawnBounds.extents.x,
                                         spawnBounds.center.x + 1.0f * spawnBounds.extents.x);

            spawnPoint.z = Random.Range(mapBoundary.center.z - 1.0f * mapBoundary.extents.z,
                                     spawnBounds.center.z + 1.0f * spawnBounds.extents.z);
        }
        return spawnPoint;
    }





    struct GameObjectTracker
    {
        string nextState;
        //mainCharacter 
    }
    Character mainCharacter;


    void wave_ready_phase(WaveBattleRunData instatiateWave)
    {
        player.SetActive(true);
        Debug.Log("Prep wave");
        for (int ctr = 0; ctr < instatiateWave.enemyList.Length; ctr++)
        {

            instatiateWave.enemyList[ctr].SetActive(true);
            instatiateWave.enemyListScript[ctr].set_player(player);
            instatiateWave.enemyListScript[ctr].set_target(player);
            instatiateWave.enemyListScript[ctr].manual_start();
        }
        playerScript.enemyList = instatiateWave.enemyListScript;
        combatScript.turn_off_combat_ui(); ;
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
        if (curEngageData.waveData[curWave].waveThemeMusic != null &&
            curEngageData.waveData[curWave].waveThemeMusic.isPlaying == false)
        {
            curEngageData.waveData[curWave].waveThemeMusic.Play();
        }
        waveReadyPhase = false;
        combatScript.turn_on_combat_ui();
        playerScript.enemyList = instatiateWave.enemyListScript;

        //playerScript.set_target(instatiateWave.enemyList[0]);
        radarScript.initialize_radar(waveRunData[curWave].enemyList, player);

        playerScript.enable_auto_adjust();

        if (curEngageData.waveData[curWave].battleType == BattleType.BOSS &&
            curWave == waveRunData.Length - 1)
        {
            playerScript.curState = "PATHING";
            playerScript.phasePlayed = false;
        }
    }

    bool check_wave_ended(WaveBattleRunData instantiateWave)
    {
        int remainingEnemy = 0;
        for (int ctr = 0; ctr < instantiateWave.enemyList.Length; ctr++)
        {
            if (instantiateWave.enemyListScript[ctr].return_cur_stats().hp > 0)
            {
                //return false;
                remainingEnemy++;
            }
        }
        if (remainingEnemy > 0)
        {
            waveRunData[curWave].remainingEnemy = remainingEnemy;
            return false;
        }
        for (int ctr = 0; ctr < instantiateWave.enemyList.Length; ctr++)
        {
            if (instantiateWave.enemyList[ctr] != null)
            {
                Destroy(instantiateWave.enemyList[ctr]);
            }
        }
        //Turn off current sound
        //if (curEngageData.waveData[curWave].waveThemeMusic != null)
        //    curEngageData.waveData[curWave].waveThemeMusic.Stop();
        playerScript.cancel_cur_state();
        return true;
    }


    void run_aerial_prewave(WaveBattleRunData nextWaveData)
    {
        for (int ctr = 0; ctr < nextWaveData.enemyListScript.Length;
            ctr++)
        {
            if (nextWaveData.enemyListScript[ctr].gameObject.activeInHierarchy == false)
                nextWaveData.enemyListScript[ctr].gameObject.SetActive(true);
            nextWaveData.enemyListScript[ctr].precombat_phase();
        }
    }

    // Use this for initialization
    public void initialize_script(PlayerMasterData input)
    {
        playerMasterData = input;
        eventRecord = playerMasterData.access_player_event_record();
        Application.targetFrameRate = 60;
        RenderSettings.skybox = curEngageData.skyBox;
        RenderSettings.fog = curEngageData.fogSettings.fogEnabled;
        RenderSettings.fogColor = curEngageData.fogSettings.fogColor;
        RenderSettings.fogStartDistance = curEngageData.fogSettings.fogStartDistance;
        RenderSettings.fogEndDistance = curEngageData.fogSettings.fogEndDistance;

        combatScriptObject = GameObject.Find("Camera");
        GameObject screenObject = GameObject.Find("texture_blacksprite");
        myScreenFadeScript = screenObject.GetComponent<ScreenFader>();



        //initialize battle variables
        boundaryObject = transform.FindChild("BattleSceneBoundary").gameObject;
        mapBoundary = boundaryObject.collider.bounds;
        GetComponent<MapFeatures>().intialize_script();
        mapChargeFlags = GetComponent<MapFeatures>().flagScripts;

        waveRunData = new WaveBattleRunData[curEngageData.waveData.Length];

        Transform playerStartPos = curEngageData.playerStartPos.transform;
        loadPlayer = (GameObject)Resources.Load("heromech");
        //loadPlayer = heroMechPrefab;
        player = (GameObject)Instantiate(loadPlayer, playerStartPos.position, playerStartPos.rotation);
        playerScript = player.GetComponent<MainChar>();
        playerScript.battleBoundary = boundaryObject.collider;
        playerScript.worldObject = gameObject;
        playerScript.playerMasterData = playerMasterData;
        if (playerMasterData == null)
        {
            Debug.Log("Null Master Data!");
        }
        playerScript.set_battle_type(curEngageData.waveData[0].battleType);
        playerScript.manual_start();
        
        if (eventRecord.check_event_played("Battle_end_6"))
        {
            AllyDataList allyLoader = new AllyDataList();
            allyData = allyLoader.get_cur_equipped_ally();
            string allyDataPath = "Ally/Tier" + allyData.tier
                + "/" + allyData.unitName;
            Debug.Log(allyData.unitName);
            GameObject allyObjectLoad = (GameObject)Resources.Load(allyDataPath);
            if (allyObjectLoad != null)
            {
                GameObject tempAllyObject = (GameObject)Instantiate(allyObjectLoad, Vector3.zero, Quaternion.identity);
                allyUnit = tempAllyObject.GetComponent<BaseAlly>();
                allyUnit.set_level(allyData.level);
                allyUnit.manual_start();
                playerScript.set_ally_unit(allyUnit.GetComponent<BaseAlly>());
            }
            else
            {
                Debug.LogError("Unit not found!");
            }
        }
        

        



        //Set up all waves
        for (int waveCtr = 0; waveCtr < waveRunData.Length; waveCtr++)
        {
            //Storyline parser
            waveRunData[waveCtr].waveEnded = false;
            if (curEngageData.waveData[waveCtr].storyObjectStart != null)
            {
                BattleStory tempHolder = curEngageData.waveData[waveCtr].storyObjectStart.
                    GetComponent<BattleStory>();
                if (tempHolder.cutSceneID.Length != 0 && !eventRecord.check_event_played(tempHolder.cutSceneID))
                {
                    waveRunData[waveCtr].thisStoryStart = tempHolder;
                    waveRunData[waveCtr].thisStoryStart.gameObject.SetActive(false);
                    waveRunData[waveCtr].loadBeforeStory = curEngageData.waveData[waveCtr].loadBeforeStory;
                }
                else if (tempHolder.cutSceneID.Length != 0 && eventRecord.check_event_played(tempHolder.cutSceneID))
                {
                    tempHolder.gameObject.SetActive(false);
                }
            }


            if (curEngageData.waveData[waveCtr].storyObjectEnd != null)
            {
                BattleStory tempHolder = curEngageData.waveData[waveCtr].storyObjectEnd.
                    GetComponent<BattleStory>();
                if (tempHolder.cutSceneID.Length != 0 && !eventRecord.check_event_played(tempHolder.cutSceneID))
                {
                    waveRunData[waveCtr].thisStoryEnd = curEngageData.waveData[waveCtr].storyObjectEnd.
                        GetComponent<BattleStory>();
                    //eventRecord.event_played(tempHolder.cutSceneID);
                    waveRunData[waveCtr].thisStoryEnd.gameObject.SetActive(false);
                }
                else if (tempHolder.cutSceneID.Length != 0 && eventRecord.check_event_played(tempHolder.cutSceneID))
                {
                    tempHolder.gameObject.SetActive(false);
                }

            }
            //SOund
            if (curEngageData.waveData[waveCtr].waveThemeMusic == null)
                curEngageData.waveData[waveCtr].waveThemeMusic = GetComponent<AudioSource>();

            waveRunData[waveCtr].player = player;
            waveRunData[waveCtr].playerScript = playerScript;
            if (curEngageData.waveData[waveCtr].battleType == BattleType.REGULAR ||
                curEngageData.waveData[waveCtr].battleType == BattleType.AERIAL
                || curEngageData.waveData[waveCtr].battleType == BattleType.AERIAL_MULTI_SPAWN_POINT)
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
                        generate_spawn_coordinate(curEngageData.waveData[waveCtr].battleType,
                        curEngageData.waveData[waveCtr].spawnPoints),
                        Quaternion.identity);

                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr] =
                        waveRunData[waveCtr].enemyList[enemyStoreCtr].GetComponent<Character>();

                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr].mapFlag = mapChargeFlags[0];
                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr].set_enemy_unit_index(enemyStoreCtr);
                    //Set level
                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr].set_level
                        (curEngageData.waveData[waveCtr].requiredEnemy[enemyCtr].level);
                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr].landCraftActive =
                        curEngageData.waveData[waveCtr].requiredEnemy[enemyCtr].landingCraftActive;

                    waveRunData[waveCtr].characterScriptCollection[enemyStoreCtr + 1] =
                        waveRunData[waveCtr].enemyListScript[enemyStoreCtr];

                    waveRunData[waveCtr].enemyList[enemyStoreCtr].SetActive(false);
                    enemyStoreCtr++;
                }


                //Load random enemy from pool
                for (int enemyCtr = 0; enemyCtr < curEngageData.waveData[waveCtr].randomEnemyCount; enemyCtr++)
                {
                    int randPool = Random.Range(0, curEngageData.waveData[waveCtr].randomEnemy.Length);
                    waveRunData[waveCtr].enemyList[enemyStoreCtr] = (GameObject)Instantiate(
                        curEngageData.waveData[waveCtr].randomEnemy[randPool].enemyUnit,
                        generate_spawn_coordinate(curEngageData.waveData[waveCtr].battleType,
                        curEngageData.waveData[waveCtr].spawnPoints),
                        Quaternion.identity);

                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr] = waveRunData[waveCtr].enemyList[enemyStoreCtr].GetComponent<Character>();
                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr].mapFlag = mapChargeFlags[0];
                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr].set_enemy_unit_index(enemyStoreCtr);
                    //set level
                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr].set_level(curEngageData.waveData[waveCtr].randomEnemy[randPool].level);
                    waveRunData[waveCtr].enemyListScript[enemyStoreCtr].landCraftActive = curEngageData.waveData[waveCtr].
                        randomEnemy[randPool].landingCraftActive;

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
                    waveRunData[waveCtr].enemyListScript[enemyCtr].set_level
                        (curEngageData.waveData[waveCtr].requiredEnemy[enemyCtr].level);
                    waveRunData[waveCtr].enemyListScript[enemyCtr].set_enemy_unit_index(enemyCtr);
                }
                if (waveCtr == waveRunData.Length - 1)
                {
                    mainBody = waveRunData[waveCtr].enemyList[0].GetComponent<Character>();
                    mainBody.set_target(player);
                }
            }
        }


        combatScript = combatScriptObject.GetComponent<CombatScript>();
        combatScript.eventControlObject = gameObject;
        combatScript.initialize_script();
        radarScript = combatScript.radarDisplay.GetComponent<Radar>();

        curWave = 0;
        //First wave initialize
        if (waveRunData[0].thisStoryStart != null)
        {
            faderActive = true;
            myScreenFadeScript.screen_fade_active(wave_start_cutscene_fade_process);

        }
        else
        {
            faderActive = true;
            myScreenFadeScript.screen_fade_active(start_battle_immediate);
        }


        boundaryObject.SetActive(false);

    }

    void target_path_updater()
    {
        if (targetPathUpdater >= waveRunData[curWave].enemyListScript.Length)
        {
            targetPathUpdater = 0;
        }
        if (waveRunData[curWave].enemyListScript.Length != 0 &&
            waveRunData[curWave].enemyListScript[targetPathUpdater] != null)
            waveRunData[curWave].enemyListScript[targetPathUpdater].modifyPath = false;
        targetPathUpdater++;
        if (targetPathUpdater >= waveRunData[curWave].enemyListScript.Length)
        {
            targetPathUpdater = 0;
        }
        if (waveRunData[curWave].enemyListScript.Length != 0 &&
            waveRunData[curWave].enemyListScript[targetPathUpdater] != null)
            waveRunData[curWave].enemyListScript[targetPathUpdater].modifyPath = true;
    }




    // Update is called once per frame
    void Update()
    {
        //Check for last wave/win condition
        if (curWave >= waveRunData.Length && mapCleared == false)
        {
            mapCleared = true;
            Debug.Log("Current wave counter: " + curWave);
            faderActive = true;
            myScreenFadeScript.screen_fade_active(end_battle_fade_process);
            enabled = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gamePaused == false)
                pause_game();
            else unpause_game();
        }
        if (playerScript.return_cur_stats().hp <= 0.0f || endBattle == true)
        {
            enabled = false;
            playerScript.animation.Play("deathstart");
            playerScript.animation.PlayQueued("deathloop");
            playerScript.initiateDeathSequence = true;
            combatScript.lose_battle_screen();
        }
        //Checking wave clear/win conditions
        if (faderActive == true)
        {
            if (!myScreenFadeScript.screen_fade_is_active())
            {
                faderActive = false;
            }
        }
        else if (mapCleared == false && curWave < waveRunData.Length)
        {
            if (waveRunData[curWave].eventRunPhase == false)
            {
                if (curEngageData.waveData[curWave].waveThemeMusic != null &&
                    curEngageData.waveData[curWave].waveThemeMusic.isPlaying == false)
                    curEngageData.waveData[curWave].waveThemeMusic.Play();
                if (player.activeInHierarchy == false)
                {
                    player.SetActive(true);
                }
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
                    faderActive = true;
                    myScreenFadeScript.screen_fade_active(end_battle_fade_process);
                    //end_battle_win();
                    enabled = false;
                    return;
                }

                if (check_wave_ended(waveRunData[curWave]))
                {
                    //If there is an ending storyline
                    if (waveRunData[curWave].thisStoryEnd != null)
                    {

                        faderActive = true;
                        myScreenFadeScript.screen_fade_active(wave_end_cutscene_fade_process);

                        return;
                    }
                    //If there is no ending storyline
                    else
                    {
                        curWave++;
                        if (curWave >= waveRunData.Length)
                        {
                            return;
                        }
                        //If there is a beginning storyline at next wave.
                        if (waveRunData[curWave].thisStoryStart != null)
                        {
                            faderActive = true;
                            myScreenFadeScript.screen_fade_active(wave_start_cutscene_fade_process);

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


                if (waveReadyPhase == true)
                {
                    playerScript.animation.CrossFade("idle");
                    playerScript.wave_transition_phase(waveRunData[curWave].enemyList[0]);
                    //Debug.Log("wave not ready");
                    all_landed(waveRunData[curWave]);
                }



                /*Event Handle*/
                if (gamePaused == false && waveReadyPhase == false)
                {
                    //if (runPlayerSide == true) {
                    playerScript.manual_update();
                    if (allyUnit != null)
                        allyUnit.manual_update();

                    runPlayerSide = false;
                    //}

                    target_path_updater();
                    if (curWave < waveRunData.Length - 1 &&
                        curEngageData.waveData[curWave].battleType == BattleType.BOSS)
                    {
                        if (curWave == 0)
                            mainBody.first_wave();
                    }
                    if (enemyScriptRunCtr >= waveRunData[curWave].enemyListScript.Length)
                    {
                        enemyScriptRunCtr = 0;
                    }
                    if (runPlayerSide == false)
                    {
                        for (int ctr = 0; ctr < waveRunData[curWave].enemyList.Length; ctr++)
                        {

                            //Debug.Log("Running enemy script: " + enemyScriptRunCtr);
                            if (waveRunData[curWave].enemyList[ctr] != null)
                            {
                                //Debug.Log("Running enemy script: " + enemyScriptRunCtr);
                                waveRunData[curWave].enemyListScript[ctr].manual_update();
                            }
                        }
                    }

                    if (enemyScriptRunCtr >= waveRunData[curWave].enemyListScript.Length)
                    {
                        enemyScriptRunCtr = 0;
                        if (runPlayerSide == true)
                            runPlayerSide = false;
                        if (runPlayerSide == false)
                            runPlayerSide = true;
                    }
                    if (curWave < waveRunData.Length - 1 &&
                        curEngageData.waveData[curWave + 1].battleType == BattleType.AERIAL_MULTI_SPAWN_POINT &&
                        waveRunData[curWave].remainingEnemy == 1)
                    {
                        run_aerial_prewave(waveRunData[curWave + 1]);
                    }

                }
            }
            else
            {
                if (player.activeInHierarchy == true)
                {
                    player.SetActive(false);
                }
                if (waveRunData[curWave].waveEnded == false &&
                    waveRunData[curWave].storyEnded == false)
                {
                    waveRunData[curWave].storyEnded =
                        waveRunData[curWave].thisStoryStart.manual_update();
                }
                else if (waveRunData[curWave].waveEnded == true &&
                    waveRunData[curWave].storyEnded == false)
                {
                    waveRunData[curWave].storyEnded =
                        waveRunData[curWave].thisStoryEnd.manual_update();
                }

                else
                {
                    if (waveRunData[curWave].waveEnded == true)
                    {
                        waveRunData[curWave].thisStoryEnd.gameObject.SetActive(false);
                        curWave++;
                        //Check for last wave/win condition
                        if (curWave >= waveRunData.Length)
                        {
                            Debug.Log("Current wave counter: " + curWave);
                            faderActive = true;
                            myScreenFadeScript.screen_fade_active(end_battle_fade_process);
                            //end_battle_win();
                            enabled = false;
                            return;
                        }
                        waveRunData[curWave].eventRunPhase = false;
                        waveRunData[curWave].storyEnded = true;
                        wave_ready_phase(waveRunData[curWave]);
                    }
                    else
                    {
                        faderActive = true;
                        myScreenFadeScript.screen_fade_active(wave_start_cutscene_end);

                    }
                }
            }
        }
    }
}
