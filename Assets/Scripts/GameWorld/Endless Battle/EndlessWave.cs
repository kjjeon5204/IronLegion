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

public class EndlessWave : BaseCombatStructure {
	public EndlessWaveBattleData[] endlessWaveBattles;
	public EndlessWaveRewardData rewards;
	public Collider spawnBox;
    public MapChargeFlag mapchargeflag;
	private int waveCounter = 0;


	void spawnWave(int waveNumber) {
        enemyUnitList = new List<Character>();
        spawnBox.enabled = true;
		for (int i = 0; i < endlessWaveBattles[waveNumber].enemies.Length; i++) {
			for (int j = 0; j < endlessWaveBattles[waveNumber].enemies[i].numEnemy; j++) {
				//create randomized location drop
				var dropPosition = transform.position;
				dropPosition.x += Random.Range (spawnBox.bounds.min.x, spawnBox.bounds.max.x);
				dropPosition.z += Random.Range (spawnBox.bounds.min.z, spawnBox.bounds.max.z);

				//spawn enemy
                GameObject enemy = (GameObject)Instantiate(endlessWaveBattles[waveNumber].enemies[i].enemy, dropPosition, Quaternion.identity);
                enemyUnitList.Add(enemy.GetComponent<Character>());

                enemyUnitList[enemyUnitList.Count - 1].mapFlag = mapchargeflag;
                enemyUnitList[enemyUnitList.Count - 1].set_enemy_unit_index(enemyUnitList.Count - 1);
                enemyUnitList[enemyUnitList.Count - 1].set_level(
                    Random.Range(endlessWaveBattles[waveNumber].enemies[i].levelMin, endlessWaveBattles[waveNumber].enemies[i].levelMax));
                enemyUnitList[enemyUnitList.Count - 1].landCraftActive = endlessWaveBattles[waveNumber].enemies[i].landcraftActive;
                enemyUnitList[enemyUnitList.Count - 1].set_target(playerScript.gameObject);
                enemyUnitList[enemyUnitList.Count - 1].set_player(playerScript.gameObject);
                enemyUnitList[enemyUnitList.Count - 1].manual_start();
			}
		}
        spawnBox.enabled = false;
	}



    bool combatInitialized = false;


    public override void initialize_combat(MainChar playerScriptInput)
    {
        playerScript = playerScriptInput;
        playerScript.initialize_character(this);
        spawnWave(waveCounter);
    }

    IList<Character> deathList = new List<Character>();
	
	// Update is called once per frame
	public override void update_battle() {
        if (enemyUnitList.Count == 0)
        {
            waveCounter++;
            if (waveCounter >= endlessWaveBattles.Length)
            {
                //wave phase end condition
                Destroy(gameObject);
            }
            else
            {
                spawnWave(waveCounter);
            }
        }
        else
        {
            if (enemyUnitList.Count > 0)
            {
                playerScript.manual_update();
                for (int ctr = 0; ctr < enemyUnitList.Count; ctr++)
                {
                    if (enemyUnitList[ctr].return_cur_stats().hp > 0)
                    {
                        enemyUnitList[ctr].manual_update();
                    }
                    else
                    {
                        deathList.Add(enemyUnitList[ctr]);
                        enemyUnitList.RemoveAt(ctr);
                        ctr--;
                    }
                }
                for (int ctr = 0; ctr < deathList.Count; ctr++)
                {
                    if (deathList[ctr] != null)
                    {
                        deathList[ctr].death_state();
                    }
                    else
                    {
                        deathList.RemoveAt(ctr);
                        ctr--;
                    }
                }
            }
        }
	}
}
