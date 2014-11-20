using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapTileManager : MonoBehaviour {
    int tileCount;
    MapTileButton[] mapTileButton;
    public int chapterCount;
    public PlayerMasterData masterData;

    public void initialize_map_tile()
    {
        mapTileButton = new MapTileButton[transform.childCount];
        IList<int> currentlyUnlocked = masterData.get_unlocked_levels(chapterCount);
        int unlockedCtr = 0;
        for (int ctr = 0; ctr < transform.childCount; ctr++)
        {
            mapTileButton[ctr] = transform.GetChild(ctr).GetComponent<MapTileButton>();
            if (unlockedCtr < currentlyUnlocked.Count &&
                currentlyUnlocked[unlockedCtr] == ctr)
            {
                mapTileButton[ctr].gameObject.SetActive(true);
                mapTileButton[ctr].initialize_tile(true);
                unlockedCtr++;
            }
            else
            {
                mapTileButton[ctr].initialize_tile(false);
             }
        }
    }
}
