using UnityEngine;
using System.Collections;

public class HangerController : MonoBehaviour {
    public GameObject equipWindow;
    public GameObject armoryWindow;
    public GameObject abilitiesWindow;
    public GameObject alliesWindow;
    public GameObject cashShopWindow;

    public enum HangerWindowState {
        EQUIP,
        ARMORY,
        ABILITY,
        ALLY,
        CASH_SHOP
    }

    public void set_window_state(HangerWindowState setWindowState)
    {
        equipWindow.SetActive(false);
        armoryWindow.SetActive(false);
        abilitiesWindow.SetActive(false);
        alliesWindow.SetActive(false);
        cashShopWindow.SetActive(false);

        if ((int)setWindowState == 0)
        {
            equipWindow.SetActive(true);
        }
        else if ((int)setWindowState == 1)
        {
            armoryWindow.SetActive(true);
        }
        else if ((int)setWindowState == 2)
        {
            abilitiesWindow.SetActive(true);
        }
        else if ((int)setWindowState == 3)
        {
            alliesWindow.SetActive(true);
        }
        else if ((int)setWindowState == 4)
        {
            cashShopWindow.SetActive(true);
        }
    }
}
