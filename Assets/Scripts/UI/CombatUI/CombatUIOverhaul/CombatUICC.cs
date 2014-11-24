using UnityEngine;
using System.Collections;

public class CombatUICC : MonoBehaviour {
    public MainChar playerCharacter;
    public AbilityButton[] abilityButtons;
    //0 - 3 close ranged
    //0 - 4 far ranged

    public UI2DController combatUIControls;
    public PlayerCombatStatus playerCombatStatus;
    public TargetStatus targetStatus;

    bool uiInitialized = false;

    public struct CombatUIPhases
    {

    }

    public void enable_close_range_weapon()
    {
        for (int ctr = 0; ctr < 4; ctr++)
        {
            abilityButtons[ctr].gameObject.SetActive(true);
            abilityButtons[ctr + 4].gameObject.SetActive(false);
        }

    }

    public void enable_long_range_weapon()
    {
        for (int ctr = 0; ctr < 4; ctr++)
        {
            abilityButtons[ctr].gameObject.SetActive(false);
            abilityButtons[ctr + 4].gameObject.SetActive(true);
        }
    }
    

    public void initialize_ui(MainChar inPlayerCharacter)
    {
        playerCharacter = inPlayerCharacter;
        string[] playerAbilities = playerCharacter.abilityNames;
        for (int ctr = 0; ctr < abilityButtons.Length; ctr++)
        {
            abilityButtons[ctr].initialize_button(playerAbilities[ctr], playerCharacter);
        }
        enable_close_range_weapon();
        uiInitialized = true;
    }



    public void run_ui_controls()
    {
        playerCombatStatus.update_player_status(playerCharacter);
        targetStatus.update_target_data(playerCharacter.targetScript);
    }
	
	// Update is called once per frame
	void Update () {
        if (uiInitialized == true)
        {
            playerCombatStatus.update_player_status(playerCharacter);
            targetStatus.update_target_data(playerCharacter.targetScript);
        }
	}
}
