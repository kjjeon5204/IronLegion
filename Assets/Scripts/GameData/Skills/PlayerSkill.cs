
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct CharacterAttackType {
	public string attackID;
    public int attackIndex;
	public int attackType;
	public float damagePercentage;
	public int damageFrequency;
	public float damageRange;
	public float hpMod;
	public float hpLeech;
	public float armorMod;
	public float armorModDur;
	public float CD;
	public int level;

	public CharacterAttackType (string attackID, 
                                 int attackIndex,
	                             int attackType, /*0 = attack, 1 = AOE, 2 = buff*/
	                             float damagePercentage,
	                             int damageFrequency,
	                             float damageRange,
	                             float hpMod,
	                             float hpLeech,
	                             float armorMod,
	                             float armorModDur,
	                             float CD,
	                             int level) {
		this.attackID = attackID;
        this.attackIndex = attackIndex;
		this.attackType = attackType;
		this.damagePercentage = damagePercentage;
		this.damageFrequency = damageFrequency;
		this.damageRange = damageRange;
		this.hpMod = hpMod;
		this.hpLeech = hpLeech;
		this.armorMod = armorMod;
		this.armorModDur = armorModDur;
		this.CD = CD;
		this.level = level;
	}
}

public class CharSkills
{
    IDictionary<string, CharacterAttackType> attackList;

    public CharSkills () {
    	
    	attackList = new Dictionary<string, CharacterAttackType>();
        CharacterAttackType temp = new CharacterAttackType("Gatling",
                                                           0,
                                                           0,
                                                           0.3f,
                                                           5,
                                                           0.4f,
                                                           0.0f,
                                                           0.0f,
                                                           0.0f,
                                                           0.0f,
                                                           4.0f,
                                                           1);
        attackList[temp.attackID] = temp;

        temp = new CharacterAttackType("Blutsauger",
                               1,
                               0,
                               0.7f,
                               1,
                               0.1f,
                               0.0f,
                               2.0f,
                               0.0f,
                               0.0f,
                               5.0f,
                               1);
        attackList[temp.attackID] = temp;

        temp = new CharacterAttackType("Shatter",
                                2,
                                0,
                                0.35f,
                                1,
                                0.15f,
                                0.0f,
                                0.0f,
                                -5.0f,
                                20.0f,
                                1.0f,
                                1);
        attackList[temp.attackID] = temp;

		temp = new CharacterAttackType("Slash",
		                        3,
		                        0,
		                        3.0f,
		                        1,
		                        0.1f,
		                        0.0f,
		                        0.0f,
		                        -50.0f,
		                        10.0f,
		                        30.0f,
		                        9);
		attackList[temp.attackID] = temp;

        temp = new CharacterAttackType("Rifleshot",
                                0,
                                0,
                                1.0f,
                                1,
                                0.05f,
                                0.0f,
                                0.0f,
                                0.0f,
                                0.0f,
                                3.0f,
                                1);
        attackList[temp.attackID] = temp;

        temp = new CharacterAttackType("Missile",
                                1,
                                1,
                                0.25f,
                                1,
                                0.25f,
                                0.0f,
                                0.0f,
                                0.0f,
                                0.0f,
                                7.0f,
                                2);
        attackList[temp.attackID] = temp;

        temp = new CharacterAttackType("Aegis",
                                2,
                                2,
                                0.0f,
                                0,
                                0.0f,
                                200.0f,
                                0.0f,
                                0.0f,
                                0.0f,
                                60.0f,
                                5);
        attackList[temp.attackID] = temp;

        temp = new CharacterAttackType("Beam",
                                3,
                                0,
                                1.5f,
                                4,
                                0.3f,
                                0.0f,
                                0.0f,
                                0.0f,
                                0.0f,
                                30.0f,
                                10);
        attackList[temp.attackID] = temp;

		temp = new CharacterAttackType("idle",
		                               0,
		                               0,
		                               0f,
		                               0,
		                               0f,
		                               0.0f,
		                               0.0f,
		                               0.0f,
		                               0.0f,
		                               0.0f,
		                               0);
		attackList[temp.attackID] = temp;

    }

    public CharacterAttackType return_data(string skillID)
    {
    	
        return attackList[skillID];
    }
}




