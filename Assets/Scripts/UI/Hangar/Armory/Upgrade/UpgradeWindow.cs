using UnityEngine;
using System.Collections;



public class UpgradeWindow : MonoBehaviour {
    public UpgradeSlot[] upgradeSlots;
    PlayerMasterData playerMasterData;
    //UpgradeData myData;

    

    public void upgrade_stat(UpgradeType upgradeType)
    {
        if (upgradeType == UpgradeType.HP)
        {
            playerMasterData.access_upgrade_data().upgrade_hp();
            playerMasterData.save_upgrade_data();
        }
        if (upgradeType == UpgradeType.DAMAGE)
        {
            playerMasterData.access_upgrade_data().upgrade_damage();
            playerMasterData.save_upgrade_data();
        }
        if (upgradeType == UpgradeType.ENERGY)
        {
            playerMasterData.access_upgrade_data().upgrade_energy();
            playerMasterData.save_upgrade_data();
        }
    }

    public int get_upgrade_count(UpgradeType upgradeType)
    {
        return playerMasterData.access_upgrade_data().get_upgrade_count(upgradeType);
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
