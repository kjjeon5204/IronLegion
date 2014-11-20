using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct EndlessWaveEnemyData {
	public GameObject enemy;
	public int numEnemy;
    public int levelMin;
    public int levelMax;
    public bool landcraftActive;
};

[System.Serializable]
public struct EndlessWaveBattleData {
	public EndlessWaveEnemyData[] enemies;
};

public struct EndlessWaveRewardData {
	int credit;
	int exp;
}

public class EndlessWave : MonoBehaviour {
	public EndlessWaveBattleData[] endlessWaveBattles;
	public EndlessWaveRewardData rewards;
	public Collider spawnBox;
    public MapChargeFlag mapchargeflag;
	private int waveCounter = 0;

    IList<Character> activeEnemyList = new List<Character>();

    EndlessBattle endlessbattle;
    MainChar playerScript;

	void spawnWave(int waveNumber) {
        activeEnemyList = new List<Character>();

		for (int i = 0; i < endlessWaveBattles[waveNumber].enemies.Length; i++) {
			for (int j = 0; j < endlessWaveBattles[waveNumber].enemies[i].numEnemy; j++) {
				//create randomized location drop
				var dropPosition = transform.position;
				dropPosition.x += Random.Range (spawnBox.bounds.min.x, spawnBox.bounds.max.x);
				dropPosition.z += Random.Range (spawnBox.bounds.min.z, spawnBox.bounds.max.z);

				//spawn enemy
                GameObject enemy = (GameObject)Instantiate(endlessWaveBattles[waveNumber].enemies[i].enemy, dropPosition, Quaternion.identity);
                activeEnemyList.Add(enemy.GetComponent<Character>());

                activeEnemyList[activeEnemyList.Count - 1].mapFlag = mapchargeflag;
                activeEnemyList[activeEnemyList.Count - 1].set_enemy_unit_index(activeEnemyList.Count - 1);
                activeEnemyList[activeEnemyList.Count - 1].set_level(
                    Random.Range(endlessWaveBattles[waveNumber].enemies[i].levelMin, endlessWaveBattles[waveNumber].enemies[i].levelMax));
                activeEnemyList[activeEnemyList.Count - 1].landCraftActive = endlessWaveBattles[waveNumber].enemies[i].landcraftActive;
                activeEnemyList[activeEnemyList.Count - 1].set_target(playerScript.gameObject);

                activeEnemyList[activeEnemyList.Count - 1].manual_start();
			}
		}
	}


    bool checkWaveCompletion()
    {
        for (int i = 0; i < activeEnemyList.Count; i++)
        {
            if (activeEnemyList[i] != null)
            {
                return false;
            }
        }
        return true;
    }

	// Use this for initialization
	public void customStart (MainChar playerScriptInput, EndlessBattle currentBattleInput) {

        playerScript = playerScriptInput;
        endlessbattle = currentBattleInput;

		spawnWave (waveCounter);
 
	}
	
	// Update is called once per frame
	void Update () {
		if (endlessbattle != null && ((activeEnemyList.Count > 0 && checkWaveCompletion()) || Input.GetKeyDown(KeyCode.Backspace))) {
			waveCounter++;
            if (waveCounter >= endlessWaveBattles.Length)
            {
                //All battles finished return back to endless battle and other end battle stuff
                endlessbattle.battleReset = true;
            }
            else
            {
                spawnWave(waveCounter);
            }
		}
	
	}
}
