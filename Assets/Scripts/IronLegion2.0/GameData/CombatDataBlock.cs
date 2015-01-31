using UnityEngine;
using System.Collections;



public class CombatDataBlock : MonoBehaviour {
    public enum MapClearCondition
    {
        WIPEOUT_ALL_ENEMY
    }

    [System.Serializable]
    public struct WaveDataBlock
    {
        public EnemyData[] randomEnemyPool;
        public EnemyData[] requiredEnemyPool;
    }



    public string mapName;
    public string mapID;
    public Sprite mapPicRef;

    //Map clear drop
    public int creditDrop;
    public int cogentumDrop;
    public int experienceDrop;
    public MapClearDropData mapDropInfo;
    public WaveDataBlock[] waveDataBlock;

    public BaseDropItem[] get_key_map_drop_list()
    {
        //Use this later
        //return mapDropInfo.get_key_drop_list();
        return null;
    }
}
