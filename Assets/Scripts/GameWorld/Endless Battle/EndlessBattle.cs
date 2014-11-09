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


	// Use this for initialization
	void Start () {
		curScene = (GameObject)Instantiate (endlessWaves [curSceneID].enviroment.gameObject, transform.position, Quaternion.identity);
        GameObject temp = (GameObject) Resources.Load("heromech");
        player = (GameObject)Instantiate(temp, Vector3.zero, Quaternion.identity);
        playerScript = player.GetComponent<MainChar>();
	}
	
	// Update is called once per frame
	void Update () {
		if (battleReset == true) {
            battleReset = false;
			//Update to the next scene
            if (curScene != null) {
                Destroy(curScene);
            }
			//increment next scene
			curSceneID++;
			if (curSceneID >= endlessWaves.Length) {
					curSceneID = 0;
			}

            //start player camera
            playerScript.enable_player_camera();
			//create next scene
			curScene = (GameObject)Instantiate (endlessWaves [curSceneID].enviroment.gameObject, transform.position, Quaternion.identity);
            curScene.GetComponent<EndlessWave>().customStart(playerScript, this);
		}
	}
}