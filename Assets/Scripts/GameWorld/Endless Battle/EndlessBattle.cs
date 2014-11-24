using UnityEngine;
using System.Collections;

[System.Serializable]
public struct EndlessWaveMasterData {
	public EndlessWave enviroment;
};


public class EndlessBattle : MonoBehaviour {
	public EndlessWaveMasterData[] endlessWaves;
	public int curSceneID = 0;
	GameObject curScene;
    GameObject player;
    MainChar playerScript;
    public bool battleReset = true;
    public CombatUICC combatUIControls;
    public PlayerMasterData playerMasterData;


	// Use this for initialization
	void Start () {
        playerMasterData.initialize_script();
		curScene = (GameObject)Instantiate (endlessWaves [curSceneID].enviroment.gameObject, transform.position, Quaternion.identity);
        GameObject temp = (GameObject) Resources.Load("heromech");
        player = (GameObject)Instantiate(temp, Vector3.zero, Quaternion.identity);
        playerScript = player.GetComponent<MainChar>();
        playerScript.playerMasterData = playerMasterData;
        playerScript.set_battle_type(BattleType.REGULAR);
        playerScript.manual_start();
        combatUIControls.initialize_ui(playerScript);
	}
	
	// Update is called once per frame
	void Update () {
        if (battleReset == true || curScene == null)
        {
            battleReset = false;
            //Update to the next scene
            if (curScene != null)
            {
                Destroy(curScene);
            }
            //increment next scene
            curSceneID++;
            if (curSceneID >= endlessWaves.Length)
            {
                curSceneID = 0;
            }

            //start player camera
            playerScript.enable_player_camera();
            //create next scene
            curScene = (GameObject)Instantiate(endlessWaves[curSceneID].enviroment.gameObject, transform.position, Quaternion.identity);
            curScene.GetComponent<EndlessWave>().initialize_combat(playerScript);
        }
        else if (curScene  != null)
        {
            curScene.GetComponent<EndlessWave>().update_combat();
        }
	}
}