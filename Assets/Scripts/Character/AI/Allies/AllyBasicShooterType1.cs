using UnityEngine;
using System.Collections;

public class AllyBasicShooterType1 : Character {
    public int allyTier;
    public float movementSpeed;
    public AnimationClip idleAnimation;
    public AnimationClip moveAnimation;
    public AnimationClip attackReadyPhase;
    public AnimationClip attackFirePhase;
    public AnimationClip attackEndPhase;
    public Ability[] unitSpecialAbility;
    public GameObject muzzleDetonator;
    public GameObject muzzlePos;


    enum UnitState
    {
        IDLE,
        MOVING,
        ATTACK,
        SPECIALATTACK
    }

    
    UnitState curState;
    int attackStatePhase;
    bool phasePlayed;
    public float maxAttackInterval;
    float attackTimeTracker;
    

    

     

    public override void manual_start()
    {
        base.manual_start();
        curState = UnitState.IDLE;
    }
    public override void manual_update()
    {
        //State control
        if (curState == UnitState.IDLE)
        {
            /*
            if ((transform.position - movementPosition).magnitude > 2.0f)
            {
                curState = UnitState.MOVING;
            }
             */ 
        }
        if (attackTimeTracker < Time.time)
        {
            curState = UnitState.ATTACK;
        }


        //State maintenance
        if (curState == UnitState.IDLE)
        {
            if (idleAnimation != null)
                animation.Play(idleAnimation.name);
        }
        else if (curState == UnitState.MOVING)
        {
            if (moveAnimation != null)
                animation.Play(moveAnimation.name);
            /*
            custom_look_at(movementPosition);
            if ((transform.position - movementPosition).magnitude > 2.0f)
                transform.Translate(movementSpeed * Vector3.forward * Time.deltaTime);
            else
                curState = UnitState.IDLE;
             */

        }
        else if (curState == UnitState.ATTACK)
        {
            if (attackStatePhase == 0) {
                if (target != null && !custom_look_at(target.transform.position)) {
                    attackStatePhase++;
                    phasePlayed = false;
                }
            }
            if (attackStatePhase == 1)
            {
                if (phasePlayed == false)
                {
                    animation.Play(attackReadyPhase.name);
                    phasePlayed = true;
                }
                else
                {
                    if (!animation.IsPlaying(attackReadyPhase.name)) 
                    {
                        attackStatePhase++;
                        phasePlayed = false;
                    }
                }
            }
            if (attackStatePhase == 2)
            {
                if (phasePlayed == false && target != null)
                {
                    if (muzzleDetonator != null && muzzlePos != null)
                        Instantiate(muzzleDetonator, muzzlePos.transform.position, muzzlePos.transform.rotation);
                    target.hit(curStats.damage * 0.15f);
                    animation.Play(attackFirePhase.name);
                    phasePlayed = true;
                }
                else
                {
                    if (!animation.IsPlaying(attackFirePhase.name))
                    {
                        attackStatePhase++;
                        phasePlayed = false;
                    }
                }
            }
            if (attackStatePhase == 3)
            {
                if (phasePlayed == false)
                {
                    animation.Play(attackEndPhase.name);
                    phasePlayed = true;
                }
                else
                {
                    if (!animation.IsPlaying(attackEndPhase.name))
                    {
                        attackStatePhase++;
                        phasePlayed = false;
                    }
                }
            }
            if (attackStatePhase == 4)
            {
                attackStatePhase = 0;
                curState = UnitState.IDLE;
                attackTimeTracker = Time.time + maxAttackInterval;
            }
        }
        else if (curState == UnitState.SPECIALATTACK)
        {
            if (unitSpecialAbility[0].run_ability())
            {
                curState = UnitState.IDLE;
                attackTimeTracker = Time.time + maxAttackInterval;
            }
        }
        //base.manual_update();
    }
}
