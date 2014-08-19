using UnityEngine;
using System.Collections;

public enum AttackTypes {
	HITBOX,
	PROJECTILE,
	PROJECTILE_AOE_CONE,
	MELEE,
	RANGED,
    MELEE_ANGLE
}


[System.Serializable]
public class CustomAbility {
	public bool self;
	public bool ranged;
	public float cooldown;
	public bool startCooldown;
    public float energyUsage;
    public int debuffIcon;
	public AttackTypes attackType;
    public AbilityPhase[] abilityPhase;
}

[System.Serializable]
public class AbilityPhase
{
    public string phaseID;
    public float targetDam;
    public float damageRange;
    public float hpLeech;
    public float targetArmor;
    public float targetArmorDuration;
    public float selfHPModifier;
    public float selfArmor;
    public float selfArmorDuration;
    public float meleeAttackRange;
    public float meleeAngleRange;
	public float aoeProjectileConeDegree;
    public float phaseDuration;
    public AnimationClip phaseAnimation;
    public bool overrideAnimationTime;
    public bool forceTarget;
    public GameObject[] muzzle;
    public GameObject projectiles;
    public bool selfTargetProjectile;
    public bool childEffectToMuzzle;
    public GameObject muzzleEffects;
    public GameObject hitEffectAutoDeactivate;
    public float radius;
    public GameObject[] effect;
    public bool isCancellable;
    public AudioSource phaseSound;

    //**ONLY USED BY MAIN CHARACTER
    public bool isMoving;
    public string movementName;
    public float approachSpeed;
    public float distFromTarget;
    public Vector3 movementDirection;
}



public class Ability : MonoBehaviour {
    public string abilityName;
	//phase tracking values
    int phaseCtr;
    bool phasePlayed;
    float phaseDuration;
    float animationLength;
	int currentTarget;


    //Character main stat
    Stats curStat;
    Character myCharacter;

	public CustomAbility myData;
    AbilityPhase currentPhaseData;

    Movement customMovement;


    float calc_damage(float damagePercentage)
    {
        float damage;
        damage = (damagePercentage / 100.0f) * curStat.baseDamage;
        if (currentPhaseData.damageRange != 0.0f)
            damage = Random.Range(damage - damage * (currentPhaseData.damageRange / 100.0f),
                damage + damage * (currentPhaseData.damageRange / 100.0f));
        return damage;
    }

    public void start_ability()
    {
        phaseCtr = 0;
        phasePlayed = false;
    }

    


    public bool is_cancellable()
    {
        return currentPhaseData.isCancellable;
    }

    public bool initialize_ability()
    {
        if (myCharacter.curEnergy < myData.energyUsage)
        {
            Debug.Log("No energy left");
            return false;
        }
        curStat = myCharacter.return_cur_stats();
        phaseCtr = 0;
        phasePlayed = false;
        myCharacter.curEnergy -= myData.energyUsage;
        return true;
    }

    public void cancel_ability()
    {
        turn_effect_off();
    }


	public CustomAbility return_data()
	{
		return myData;
	}

    //Returns true is animation is successfully parse otherwise prints error and returns false
    bool animation_parser()
    {
     
        AnimationState animationModifier;
        //Animation parser
        if (currentPhaseData.overrideAnimationTime == false && currentPhaseData.phaseAnimation != null)
        {
            animation.CrossFade(currentPhaseData.phaseAnimation.name, 0.1f);
        }
        else if (currentPhaseData.phaseAnimation == null && currentPhaseData.isMoving == false)
        {
            if (currentPhaseData.isMoving == false)
                phaseDuration = Time.time + currentPhaseData.phaseDuration;
        }
        else if (currentPhaseData.overrideAnimationTime == true)
        {
            animationModifier = animation[currentPhaseData.phaseAnimation.name];
            animationModifier.speed = 1.0f;
            animationModifier.speed = animationModifier.length / currentPhaseData.phaseDuration;
            animation.CrossFade(currentPhaseData.phaseAnimation.name, 0.1f);
        }
        else if (currentPhaseData.isMoving == true)
        {
        }
        else
        {
            Debug.LogError("In " + this.name + "'s phase " + phaseCtr +
                " unparsable animation option detected!" + " Ending current animation phase");
            return false;
        }
        if (currentPhaseData.phaseAnimation != null)
            animationLength = currentPhaseData.phaseAnimation.length;
        return true;
    }


	public bool check_within_cone(GameObject myTarget) {
		float checkAngle = Vector3.Angle (transform.forward,
		               		transform.InverseTransformPoint(myTarget.transform.position));
		if (checkAngle < currentPhaseData.aoeProjectileConeDegree) {
			return true;
		}
		else {
			return false;
		}
	}


    bool projectile_parser()
    {
    	if(currentPhaseData.muzzleEffects != null) {
			foreach (GameObject muzzle in currentPhaseData.muzzle) {
				GameObject effectStore = (GameObject)Instantiate(currentPhaseData.muzzleEffects, muzzle.transform.position, muzzle.transform.rotation);
				if (currentPhaseData.childEffectToMuzzle == true) {
					effectStore.transform.parent = muzzle.transform;
				}
			}
		}
        if (myData.ranged == false)
        {
            if (currentPhaseData.targetDam > 0.0f && (myData.attackType == AttackTypes.MELEE_ANGLE
                || myData.attackType == AttackTypes.HITBOX) &&
                ((myCharacter.transform.position - myCharacter.target.transform.position).magnitude 
                < currentPhaseData.meleeAttackRange || currentPhaseData.meleeAttackRange == 0) &&
                ((Vector3.Angle (Vector3.forward, transform.InverseTransformPoint(myCharacter.target.transform.position)) < currentPhaseData.meleeAngleRange) ||
                currentPhaseData.meleeAngleRange == 0.0f))
            {
                Debug.Log("Ability used");
                if (currentPhaseData.targetArmorDuration > 0.0f)
                {
                    myCharacter.target.GetComponent<Character>().
                        characterDebuffScript.apply_debuff(abilityName, currentPhaseData.targetArmor,
                        0.0f, currentPhaseData.targetArmorDuration , myData.debuffIcon);
                    if (currentPhaseData.hitEffectAutoDeactivate != null)
                    {
                        currentPhaseData.hitEffectAutoDeactivate.SetActive(true);
                    }
                }
                float damageDone = myCharacter.target.GetComponent<Character>().hit(calc_damage(currentPhaseData.targetDam));
                if (currentPhaseData.hpLeech > 0)
                {
                    myCharacter.hit(-1.0f * damageDone * currentPhaseData.hpLeech / 100.0f);
                }
            }
            if (currentPhaseData.projectiles != null && myData.attackType == AttackTypes.PROJECTILE)
            {

                foreach (GameObject curMuzzle in currentPhaseData.muzzle)
                {
                    if (currentPhaseData.forceTarget == true)
                        curMuzzle.transform.LookAt(myCharacter.target.collider.bounds.center);
                    GameObject projectileObject = (GameObject)Instantiate(currentPhaseData.projectiles,
                        curMuzzle.transform.position, curMuzzle.transform.rotation);
                    MyProjectile projectileScript = projectileObject.GetComponent<MyProjectile>();
                    ProjectileDataInput tempData;
                    tempData.inTargetScript = myCharacter.target.GetComponent<Character>();

                    if (currentPhaseData.selfTargetProjectile)
                        projectileScript.set_projectile(myCharacter, gameObject, calc_damage(currentPhaseData.targetDam));
                    else projectileScript.set_projectile(myCharacter.targetScript, gameObject, calc_damage(currentPhaseData.targetDam));
                }
            }
            if (currentPhaseData.projectiles != null && myData.attackType == AttackTypes.HITBOX)
            {

                foreach (GameObject curMuzzle in currentPhaseData.muzzle)
                {
                    if (currentPhaseData.forceTarget == true)
                        curMuzzle.transform.LookAt(myCharacter.target.collider.bounds.center);
                    GameObject projectileObject = (GameObject)Instantiate(currentPhaseData.projectiles,
                        curMuzzle.transform.position, curMuzzle.transform.rotation);
                    MyProjectile projectileScript = projectileObject.GetComponent<MyProjectile>();
                    ProjectileDataInput tempData;
                    tempData.inTargetScript = myCharacter.target.GetComponent<Character>();

                    if (currentPhaseData.selfTargetProjectile)
                        projectileScript.set_projectile(myCharacter, gameObject, calc_damage(currentPhaseData.targetDam));
                    else projectileScript.set_projectile(myCharacter.targetScript, gameObject, calc_damage(currentPhaseData.targetDam));
                }
            }
        }
        else if (myData.ranged == true)
        {
            if (currentPhaseData.projectiles != null && myData.attackType == AttackTypes.PROJECTILE)
            {
            	
                foreach (GameObject curMuzzle in currentPhaseData.muzzle)
                {
                    if (currentPhaseData.forceTarget == true)
                        curMuzzle.transform.LookAt(myCharacter.target.collider.bounds.center);
                    GameObject projectileObject = (GameObject)Instantiate(currentPhaseData.projectiles,
                        curMuzzle.transform.position, curMuzzle.transform.rotation);
                    MyProjectile projectileScript = projectileObject.GetComponent<MyProjectile>();
					ProjectileDataInput tempData;
					tempData.inTargetScript = myCharacter.target.GetComponent<Character>();
					
                    if (currentPhaseData.selfTargetProjectile) 
						projectileScript.set_projectile(myCharacter, gameObject, calc_damage(currentPhaseData.targetDam));
					else projectileScript.set_projectile(myCharacter.targetScript, gameObject, calc_damage(currentPhaseData.targetDam));
                }
            }
			
            else
            {
                if (currentPhaseData.targetDam > 0.0f)
                    myCharacter.target.GetComponent<Character>().hit(calc_damage(currentPhaseData.targetDam));
            }
        }
        return true;
    }

    void character_self_buff_parser() 
    {
        if (currentPhaseData.selfArmorDuration > 0.0f)
        {
            myCharacter.characterDebuffScript.apply_debuff(abilityName, currentPhaseData.selfArmor,
                0.0f, currentPhaseData.targetArmorDuration, myData.debuffIcon);
        }
        if (currentPhaseData.selfHPModifier > 0.0f)
        {
            myCharacter.hit(-currentPhaseData.selfHPModifier);
        }
    }


    //Toggle effect
    bool effect_parser()
    {
        foreach (GameObject curEffect in currentPhaseData.effect)
        {
            if (curEffect != null)
            {
                curEffect.SetActive(true);
            }
            else
            {
                Debug.LogError("Null effect in " + this.gameObject + 
                    " detected in phase " + phaseCtr);
                return false;
            }
        }
        return true;
    }

    void turn_effect_off()
    {
        foreach (GameObject curEffect in currentPhaseData.effect)
        {
            if (curEffect != null)
            {
                curEffect.SetActive(false);
            }
        }
        if (currentPhaseData.phaseSound != null)
            currentPhaseData.phaseSound.Stop();
    }
    
    float calculate_distance(Transform object1, Transform object2)
    {
        float dist = 0.0f;
        Vector3 rayCastDirection = object2.collider.bounds.center - object1.collider.bounds.center;
        Ray rayData = new Ray();
        rayData.direction = rayCastDirection.normalized;
        rayData.origin = object1.collider.bounds.center;
        RaycastHit hitData;
        if (Physics.Raycast(rayData, out hitData, 100.0f))
        {
            dist = (object1.transform.position - hitData.point).magnitude;
            //Debug.Log("Calculated raycast distance: " + dist +  " checked against: " + hitData.collider.gameObject);
            dist -= object1.gameObject.collider.bounds.extents.z;
        }
        

        return dist;
    }
    
    bool movement_parser()
    {
        if (phasePlayed == false)
        {
            //float distToMove = (this.transform.position - myCharacter.target.transform.position).magnitude
            //    - currentPhaseData.distFromTarget;
            float distToMove = calculate_distance(this.transform, myCharacter.target.transform) - currentPhaseData.distFromTarget;
            if (currentPhaseData.movementDirection == Vector3.forward || currentPhaseData.movementDirection == Vector3.back
                || currentPhaseData.movementDirection == Vector3.zero)
            {
                if (distToMove <= 0.0f)
                {
                    customMovement.initialize_movement(currentPhaseData.movementName,
                        0.0f, currentPhaseData.approachSpeed, currentPhaseData.movementDirection);
                }
                else
                {
                    Debug.Log("Initialize movement: " + currentPhaseData.movementName);
                    customMovement.initialize_movement(currentPhaseData.movementName,
                        distToMove, currentPhaseData.approachSpeed, currentPhaseData.movementDirection);
                }
            }
            else
            {
                customMovement.initialize_movement(currentPhaseData.movementName,
                   currentPhaseData.distFromTarget, currentPhaseData.approachSpeed,
                   currentPhaseData.movementDirection);
            }
        }
        return true;
    }

    public void sound_parser()
    {
        if (currentPhaseData.phaseSound != null)
        {
            currentPhaseData.phaseSound.Play();
        }
    }



    //If ability is not done playing, return true, if ability is done  playing, return false
    public bool run_ability()
    {
        if (phaseCtr >= myData.abilityPhase.Length)
        {
            return false;
        }
        else {
            currentPhaseData = myData.abilityPhase[phaseCtr];
        }
        if (phasePlayed == false)
        {
            if (!animation_parser())
                return false;

            projectile_parser();

            character_self_buff_parser();

            effect_parser();

            if (currentPhaseData.isMoving == true)
            {
                movement_parser();
            }

            sound_parser();

            phasePlayed = true;
            return true;
        }
        else if (phasePlayed == true)
        {
            if (animationLength > 0.0f)
            {
                animationLength -= Time.deltaTime;
            }
            //Condition for ending phase

            //moving based condition
            if (currentPhaseData.isMoving == true && customMovement.run_movement())
            {
                return true;
            }
            else if (currentPhaseData.isMoving == true && !customMovement.run_movement())
            {
                //turn_effect_off();
                phaseCtr++;
                phasePlayed = false;
            }
            else if (currentPhaseData.isMoving == true && currentPhaseData.movementDirection == Vector3.forward)
            {
                float distToMove = calculate_distance(this.transform, myCharacter.target.transform) - currentPhaseData.distFromTarget;
                if (distToMove < currentPhaseData.distFromTarget / 2.0f)
                {
                    //turn_effect_off();
                    phaseCtr++;
                    phasePlayed = false;
                }
            }

            //animation based condition
            else if (/*animationLength <= 0.0f && */((currentPhaseData.overrideAnimationTime == false && currentPhaseData.phaseAnimation != null) ||
                currentPhaseData.overrideAnimationTime == true))
            {
                if (!animation.IsPlaying(currentPhaseData.phaseAnimation.name))
                {
                    //turn_effect_off();
                    phaseCtr++;
                    phasePlayed = false;
                }
            }

            //phase duration condition
            else if (currentPhaseData.phaseAnimation == null && currentPhaseData.isMoving == false)
            {
                if (Time.time > phaseDuration)
                {
                    //turn_effect_off();
                    phaseCtr++;
                    phasePlayed = false;
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    void Start()
    {
        foreach (AbilityPhase thisPhase in myData.abilityPhase)
        {
            foreach (GameObject effectAccess in thisPhase.effect)
            {
                if (effectAccess != null)
                    effectAccess.SetActive(false);
            }
        }

        customMovement = this.GetComponent<Movement>();
        myCharacter = GetComponent<Character>();
    }

    void Update()
    {
    }
}