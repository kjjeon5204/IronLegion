using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DebuffTracker
{
    public string skillID;
    public float armorModifier;
    public float attackModifier;
    public float duration;
    public int buffType;
    public int buffIconSlot;
}


public class Debuff : MonoBehaviour {
    public IList<DebuffTracker> trackDebuff = new List<DebuffTracker>();
    public int numOfActiveDebuff = 0;
    Character thisUnit;

    public CombatScript combatScriptAcc;
    bool scriptInitialized = false;

    int contains_debuff_type(string skillID)
    {
        int acc = -1;
        for (int ctr = 0; ctr < numOfActiveDebuff; ctr++)
        {
            if (trackDebuff[ctr].skillID == skillID) 
            {
                acc = ctr;
            }
        }
        return acc;
    }

    public void apply_debuff(string skillID, float armorMod,float damageMod, float duration, 
        int buffIconType)
    {
        int acc = contains_debuff_type(skillID);
        if (acc == -1)
        {
            numOfActiveDebuff++;
            DebuffTracker curDebuff = new DebuffTracker();
            curDebuff.skillID = skillID;
            curDebuff.armorModifier = armorMod;
            curDebuff.attackModifier = damageMod;
            curDebuff.duration = duration;
            curDebuff.buffType = buffIconType;
            curDebuff.buffIconSlot = -1;
            trackDebuff.Add(curDebuff);
            thisUnit.modify_stat(curDebuff.armorModifier, curDebuff.attackModifier);
        }
        else
        {
            trackDebuff[acc].duration = duration;
        }
    }

    

    // Use this for initialization
    public void initialize_script()
    {
        thisUnit = this.GetComponent<Character>();
        combatScriptAcc = GameObject.Find("Camera").GetComponent<CombatScript>();
        scriptInitialized = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (scriptInitialized == true)
        {
            for (int ctr = 0; ctr < numOfActiveDebuff; ctr++)
            {
                trackDebuff[ctr].duration -= Time.deltaTime;
                if (trackDebuff[ctr].duration <= 0.0f)
                {
                    combatScriptAcc.turn_off_buff_icon(trackDebuff[ctr].buffType, trackDebuff[ctr].buffIconSlot);
                    thisUnit.modify_stat(-trackDebuff[ctr].armorModifier, -trackDebuff[ctr].attackModifier);
                    trackDebuff.Remove(trackDebuff[ctr]);
                    numOfActiveDebuff--;
                }
            }
        }
	}
}
