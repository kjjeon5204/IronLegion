using UnityEngine;
using System.Collections;

public class PlayerCombatStatus : MonoBehaviour {
    public GameObject healthBar;
    public GameObject[] energyBars;

    public float healthBarModRate;
    float currentHealthBarScale;
    float targetHealthBarScale;

    public void update_player_status(MainChar mainCharacter)
    {
        float energyRatio = mainCharacter.return_cur_stats().energy / mainCharacter.return_base_stats().energy;
        float healthRatio = mainCharacter.return_cur_stats().hp / mainCharacter.return_base_stats().hp;
        ///Update Energy bar
        int numOfEnergyActivate = (int)(energyBars.Length * energyRatio);
        for (int ctr = 0; ctr < energyBars.Length; ctr++)
        {
            if (ctr < numOfEnergyActivate)
                energyBars[ctr].SetActive(true);
            else
                energyBars[ctr].SetActive(false);
        }

        //Update health bar
        Vector3 healthBarScale = healthBar.transform.localScale;
        targetHealthBarScale = healthRatio;
        if (targetHealthBarScale < healthBarScale.x)
        {
            if (targetHealthBarScale < healthBarScale.x
                - healthBarModRate * Time.deltaTime)
            {
                healthBarScale.x -= healthBarModRate * Time.deltaTime;
            }
            else
            {
                healthBarScale.x = targetHealthBarScale;
            }
        }
        else if (targetHealthBarScale > healthBarScale.x)
        {
            if (targetHealthBarScale > healthBarScale.x
                + healthBarModRate * Time.deltaTime)
            {
                healthBarScale.x += healthBarModRate * Time.deltaTime;
            }
            else
            {
                healthBarScale.x = targetHealthBarScale;
            }
        }
        healthBar.transform.localScale = healthBarScale;
    }
}
