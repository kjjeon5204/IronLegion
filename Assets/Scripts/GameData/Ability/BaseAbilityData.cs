using UnityEngine;
using System.Collections;


public struct DamageModel
{
    public bool selfTarget;
    public float damage;
    public float penetration;
    public BuffData enemyBuffEffect;
    public BuffData playerBuffEffect;
    public GameObject damageEffect;
}

/*This class is base ability of all different types of abilities.*/

public class BaseAbilityData : MonoBehaviour {
    public string skillName;

    //Damage model specifies the number of times damage is applied. If damage is applied
    //2 or more times, set number of damage model to number of times damage model is applied.
    public DamageModel[] damageModels;
     
    /*This function is used to apply damage effect. This one uses default damage effect.
      However, more different damage effect can be made by overriding this basic attack function.*/
    public virtual void apply_damage_effect(Character attackingChar, Character defendingChar)
    {

    }

    float curCoolDown;
    /*This function handles non active state actions of the ability such as decreasing cooldown*/
    public virtual void update_ability()
    {
        
    }
}
