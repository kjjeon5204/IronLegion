using UnityEngine;
using System.Collections;

public class TargetStatus : MonoBehaviour {
    public GameObject healthBar;
    public TextMesh targetArmorText;
    public TextMesh targetDamageText;
    public TextMesh targetNameText;
    public float healthBarModRate;

    public void update_target_data(Character target)
    {
        //Update health bar
        targetArmorText.text = target.return_cur_stats().armor.ToString();
        targetDamageText.text = target.return_cur_stats().damage.ToString();
        float targetHealthBarScale = target.return_cur_stats().hp / 
            target.return_base_stats().hp;
        Vector3 healthBarScale = healthBar.transform.localScale;
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
