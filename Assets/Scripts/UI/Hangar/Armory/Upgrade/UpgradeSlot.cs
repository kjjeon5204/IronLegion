using UnityEngine;
using System.Collections;

public enum UpgradeType
{
    HP,
    ENERGY,
    DAMAGE
}

public class UpgradeSlot : MonoBehaviour {
    public UpgradeWindow upgradeWindow;
    public ArmoryControl armoryControl;

    public Renderer[] textData;

    public TextMesh creditRequirement;

    public UpgradeType myType;

    void Clicked()
    {
        int costReq = 0;
        costReq = (int)(200.0f * Mathf.Pow(1.5f, upgradeWindow.get_upgrade_count(myType)));
        if (costReq <= armoryControl.get_owned_credit())
        {
            armoryControl.credit_spent(costReq);
            upgradeWindow.upgrade_stat(myType);
        }
        creditRequirement.text = costReq.ToString();
    }

    void Start()
    {
        creditRequirement.text = ((int)(200.0f * Mathf.Pow(1.5f, 
            upgradeWindow.get_upgrade_count(myType)))).ToString();
    }
}
