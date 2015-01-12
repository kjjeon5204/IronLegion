using UnityEngine;
using System.Collections;

public class BuffData : MonoBehaviour {
    public string statusDescription;
    public Sprite buffSprite;
    public float armorModifier;
    public float attackModifier;
    public float modifierDuration;
    public GameObject buffEffect;
    

    //Used to initially apply buff effect to Character
    public virtual void apply_buff_effect(Character character)
    {

    }

    float curBuffDuration;

    //Returns true while buff effect is in effect. If it returns false. buff time has ended
    //and the character should called end_buff_effect and clean up.
    public virtual bool run_buff_effect()
    {
        if (curBuffDuration > 0)
        {
            curBuffDuration -= Time.deltaTime;
        }
        else if (curBuffDuration <= 0)
        {
            return true;
        }
        return false;
    }

    //Removes buff effect from Character
    public virtual void end_buff_effect(Character character)
    {
    }
}
