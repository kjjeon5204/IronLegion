using UnityEngine;
using System.Collections;


[System.Serializable]
public struct AerialType1Attacks
{
    public GameObject projectile;
    public float attackRange;
    public int attackFrequency;
    public float damagePercentage;
    public bool forceTarget;
    public GameObject[] muzzle;
    public bool parentEffectToMuzzle;
    public GameObject[] destroyEffect;
    //public GameObject[] activeEffect;

    public AnimationClip[] attackAnimations;
}


public class AerialType1NoTurret : Character {

    enum AerialState
    {
        IDLE,
        ATTACKING,
        MOVING,
        DEATH
    }
    public AerialType1Attacks[] attackTypes;
    

    AerialState aerialState;
    public AnimationClip idleClip;
    public bool turnAble;
    public float maxAttackTimeInterval;

    float currentAggressiveness;
    float nextPollTime;

    bool phasePlayed;
    int phaseCtr;
    int currentlyPolledAttack = 0;
    int attackFrequencyTracker;

    int spawnPhase = 0;

    Vector3 easingDirection;

	// Use this for initialization
    public override void manual_start()
    {
        base.manual_start();
    }
	

	// Update is called once per frame
    public override void manual_update()
    {
        float distFromTarget = (target.transform.position - transform.position).magnitude;
        if (aerialState == AerialState.IDLE)
        {
            if (currentAggressiveness < 100)
            {
                currentAggressiveness += 100.0f / maxAttackTimeInterval * Time.deltaTime;
            }
            if (Time.time > nextPollTime)
            {
                //Poll attack
                int pollVal = Random.Range(0, 100);
                if (pollVal < (int)currentAggressiveness)
                {
                    pollVal = Random.Range(0, attackTypes.Length);
                    currentlyPolledAttack = pollVal;
                    phasePlayed = false;
                    phaseCtr = 0;
                    attackFrequencyTracker = 0;
                    aerialState = AerialState.ATTACKING;
                    if ((target.transform.position - transform.position).magnitude >
                        attackTypes[currentlyPolledAttack].attackRange)
                    {
                        aerialState = AerialState.IDLE;
                    }
                }
                nextPollTime = Time.time + 1.0f;
            }
        }
        if (aerialState == AerialState.IDLE)
        {
            if (distFromTarget > 50.0f)
            {
                aerialState = AerialState.MOVING;
            }
        }


        if (curStats.baseHp <= 0.0f && aerialState != AerialState.DEATH)
        {
            aerialState = AerialState.DEATH;
        }

        if (turnAble == true)
        {
            custom_look_at_3D(target.collider.bounds.center);
        }
        if (aerialState == AerialState.IDLE)
        {
            animation.CrossFade(idleClip.name);
            transform.Translate(5.0f * Time.deltaTime * Vector3.forward);
        }
        else if (aerialState == AerialState.DEATH)
        {
            death_state();
        }
        else if (aerialState == AerialState.MOVING)
        {
            transform.Translate(10.0f * Vector3.forward * Time.deltaTime);
            if (distFromTarget < 50.0f)
            {
                aerialState = AerialState.IDLE;
            }
        }
        else if (aerialState == AerialState.ATTACKING)
        {
            if (phaseCtr == 0)
            {
                if (phasePlayed == false)
                {
                    animation.Play(attackTypes[currentlyPolledAttack].attackAnimations[0].name);
                    phasePlayed = true;
                }
                else
                {
                    if (!animation.IsPlaying(attackTypes[currentlyPolledAttack].
                        attackAnimations[0].name))
                    {
                        phasePlayed = false;
                        phaseCtr++;
                    }
                }
            }
            else if (phaseCtr == 1)
            {
                if (phasePlayed == false)
                {
                    animation.Play(attackTypes[currentlyPolledAttack].attackAnimations[1].name);
                    //Fire projectil
                    float rawDamage = attackTypes[currentlyPolledAttack].damagePercentage *
                        curStats.baseDamage / 100.0f;
                    Debug.Log("Damage Done: " + rawDamage);
                    foreach (GameObject muzzle in attackTypes[currentlyPolledAttack].muzzle)
                    {
                        if (attackTypes[currentlyPolledAttack].forceTarget == true)
                            muzzle.transform.LookAt(target.collider.bounds.center);
                        GameObject bulletObject = (GameObject)Instantiate(
                            attackTypes[currentlyPolledAttack].projectile,
                            muzzle.transform.position, muzzle.transform.rotation);


                        MyProjectile projectileScript = bulletObject.GetComponent<MyProjectile>();
                        projectileScript.set_projectile(targetScript, gameObject, rawDamage);

                        foreach (GameObject muzzleEffect in
                            attackTypes[currentlyPolledAttack].destroyEffect)
                        {
                            GameObject effectHolder = (GameObject)Instantiate(muzzleEffect,
                                muzzle.transform.position,
                                muzzle.transform.rotation);
                            if (attackTypes[currentlyPolledAttack].parentEffectToMuzzle == true)
                            {
                                effectHolder.transform.parent = muzzle.transform;
                            }
                        }
                    }
                    phasePlayed = true;
                }
                else
                {
                    if (!animation.IsPlaying(attackTypes[currentlyPolledAttack].
                        attackAnimations[1].name) && attackFrequencyTracker >=
                        attackTypes[currentlyPolledAttack].attackFrequency)
                    {
                        phasePlayed = false;
                        phaseCtr++;
                    }

                    if (!animation.IsPlaying(attackTypes[currentlyPolledAttack].
                        attackAnimations[1].name) && attackFrequencyTracker <
                        attackTypes[currentlyPolledAttack].attackFrequency)
                    {
                        phasePlayed = false;
                        attackFrequencyTracker++;
                    }
                }
            }
            else if (phaseCtr == 2)
            {
                if (phasePlayed == false)
                {
                    Debug.Log("Back to idle");
                    animation.Play(attackTypes[currentlyPolledAttack].attackAnimations[2].name);
                    phasePlayed = true;
                }
                else
                {
                    if (!animation.IsPlaying(attackTypes[currentlyPolledAttack].
                        attackAnimations[2].name))
                    {
                        Debug.Log("Back to idle");
                        currentAggressiveness = 0;
                        aerialState = AerialState.IDLE;
                    }
                }
            }
        }
    }

    public override void precombat_phase()
    {
        if (spawnPhase == 0)
        {
            custom_look_at_3D(mapFlag.transform.position);
            transform.Translate(20.0f * Vector3.forward * Time.deltaTime);

            if ((mapFlag.transform.position - transform.position).magnitude < 40.0f)
            {
                spawnPhase++;
                easingDirection = transform.position + transform.right;
            }
        }
        //Condition to pause precombat phase.
        else if (spawnPhase == 1)
        {
            if (custom_look_at_3D(easingDirection))
            {
                spawnPhase++;
            }
        }
        else if (spawnPhase == 2)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 20.0f);
            transform.Rotate(Vector3.up * 45.0f * Time.deltaTime);
        }
    }
}
