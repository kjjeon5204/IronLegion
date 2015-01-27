using UnityEngine;
using System.Collections;

public class PlayerAbilityData : BaseAbilityData {
    public Sprite skillSprite;
    public string skillDescription; //Skill description that will be displayed at on ability page.
    public float coolDown; //Sets the cool down of the current skill
    public bool startCD; //Start cool down

    float curCooldown;

    public override void update_ability()
    {
        if (curCooldown > 0.0f)
        {
            curCooldown -= Time.deltaTime;
            if (curCooldown < 0.0f)
            {
                curCooldown = 0.0f;
            }
        }
    }

    public float get_cur_cooldown_ratio()
    {
        return curCooldown / coolDown;
    }

}
