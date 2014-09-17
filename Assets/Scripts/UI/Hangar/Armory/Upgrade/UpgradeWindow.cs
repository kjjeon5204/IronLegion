using UnityEngine;
using System.Collections;



public class UpgradeWindow : MonoBehaviour {
    public UpgradeSlot[] upgradeSlots;

    UpgradeData myData;

    

    public void upgrade_stat(UpgradeType upgradeType)
    {
        if (upgradeType == UpgradeType.HP)
        {
            myData.upgrade_hp();
            myData.save_upgrade_data();
        }
        if (upgradeType == UpgradeType.DAMAGE)
        {
            myData.upgrade_damage();
            myData.save_upgrade_data();
        }
        if (upgradeType == UpgradeType.ENERGY)
        {
            myData.upgrade_energy();
            myData.save_upgrade_data();
        }
    }

    public int get_upgrade_count(UpgradeType upgradeType)
    {
        if (myData == null)
        {
            myData = new UpgradeData();
        }
        return myData.get_upgrade_count(upgradeType);
    }

	// Use this for initialization
	void Start () {
        if (myData == null)
            myData = new UpgradeData();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
