using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MovementData
{
    public string movementID;
    public AnimationClip[] animationClips;

    public float maxVelocity;
    public float cruisePhaseLength;
    public float accelerationValue;
    public float accelerationTime;
    public float decelerationValue;

    public Vector3 movementDirection;
}


public class Movement : MonoBehaviour {
    public MovementData[] movementCollection;

    MovementData curMovement;
    Character curCharacterScript;

    IDictionary<string, MovementData> movementLibrary = new Dictionary<string, MovementData>();


    //Tracker
    float acceleration;
    float accelerationDist;
    float curVelocity;
    float maxVelocity;
    float remainingDist;
    float accDurationTime;
    float decelerationTime;
    float distRange;
    bool checkRelativeDist;

    int curPhase;
    bool phasePlayed;
    int phaseCtr;
    float duration;
    float timeTracker;

    float phaseMaxDist;


    public void initialize_movement(string moevemntName, float dist, float veloctiy, Vector3 movementDirection,
        float targetDist) {
        checkRelativeDist = true;
        distRange = targetDist;
        remainingDist = dist;
        curVelocity = maxVelocity;
        curPhase = 0;
        phasePlayed = false;
        phaseCtr = 0;
        if (movementDirection != Vector3.zero)
            curMovement.movementDirection = movementDirection;
    }


    public void initialize_movement(string movementName, float dist, float velocity, Vector3 movementDirection) {
        checkRelativeDist = false;
        curMovement = movementLibrary[movementName];
        /*used for acceleration
        curVelocity = 0.0f;
        maxVelocity = dist / timeDuration;
        acceleration = maxVelocity / curMovement.accDuration;
        accelerationDist = (1.0f / 2.0f) * acceleration * (curMovement.accDuration) * (curMovement.accDuration);
        remainingDist = dist;
        accDurationTime = timeDuration * (curMovement.accDuration);
         */
        remainingDist = dist;
        maxVelocity = velocity;
        curVelocity = maxVelocity;
        curPhase = 0;
        phasePlayed = false;
        phaseCtr = 0;
        if (movementDirection != Vector3.zero)
            curMovement.movementDirection = movementDirection;
    }


    public void acceleration_phase()
    {
        if (phasePlayed == false)
        {
            phasePlayed = true;
            animation.CrossFade(curMovement.animationClips[0].name, 0.1f);
            
            //animation.Play(curMovement.animationClips[0].name);
            //animation[curMovement.animationClips[0].name].speed = animation[curMovement.animationClips[0].name].length / curMovement.accDuration;
            
            /* Used for acceleration
            curVelocity += acceleration * Time.deltaTime;
            remainingDist -= curVelocity * Time.deltaTime;
            */
        }
        else
        {
            if (!animation.IsPlaying(curMovement.animationClips[0].name))
            {
                curVelocity = maxVelocity;
                phasePlayed = false;
                phaseCtr++;
            }
            /*
            else
            {
                curVelocity += acceleration * Time.deltaTime;
                remainingDist -= curVelocity * Time.deltaTime;

            }
             */ 
        }
        curCharacterScript.custom_translate(curMovement.movementDirection * Time.deltaTime * curVelocity);
        remainingDist -= curVelocity * Time.deltaTime;
    }

    public bool internal_acceleration_phase()
    {
        if (phasePlayed == false)
        {
            curVelocity = 0.0f;
            phasePlayed = true;
            AnimationState accAnimation = animation[curMovement.animationClips[0].name];
            animation.CrossFade(curMovement.animationClips[0].name);
            accAnimation.speed = 0.5f;
            accAnimation.speed = 1.0f * (accAnimation.length * accAnimation.speed) / curMovement.accelerationTime;
            timeTracker = Time.time + curMovement.accelerationTime;
            phaseMaxDist = (curVelocity * curMovement.accelerationTime + (1.0f / 2.0f) * curMovement.accelerationValue
            * curMovement.accelerationTime * curMovement.accelerationTime);
        }
        else
        {
            if (Time.time > timeTracker) {
                phasePlayed = false;
                phaseCtr++;
            }
        }
        curVelocity += (1.0f) * curMovement.accelerationValue * Time.deltaTime;
        float accurateDist = (curVelocity * Time.deltaTime + (1.0f / 2.0f) * curMovement.accelerationValue
            * Time.deltaTime * Time.deltaTime);
        if (phaseMaxDist > accurateDist)
        {
            transform.Translate(curMovement.movementDirection * accurateDist);
        }
        else
        {
            transform.Translate(curMovement.movementDirection * phaseMaxDist);
        }
        return true;
    }




    public void cruising_phase() 
    {
        if (phasePlayed == true) 
        {
            float timeRemaining = curMovement.animationClips[2].length * curVelocity;
        }
        
        if (remainingDist > 0.0f)
        {
            animation.CrossFade(curMovement.animationClips[1].name, 0.1f);
            //animation.Play(curMovement.animationClips[1].name);
            remainingDist -= curVelocity * Time.deltaTime;
        }
        else
        {
            phasePlayed = false;
            phaseCtr++;
        }
        curCharacterScript.custom_translate(curMovement.movementDirection * Time.deltaTime * curVelocity);
    }


    public void internal_cruise_phase()
    {
        if (curMovement.cruisePhaseLength > 0.0f)
        {
            animation.CrossFade(curMovement.animationClips[1].name, 0.1f);
            curCharacterScript.custom_translate(curMovement.movementDirection * Time.deltaTime * curVelocity);
        }
        else
        {
            phasePlayed = false;
            phaseCtr++;
        }
    }


    public void deceleration_phase(AnimationClip phaseClip)
    {
        if (phasePlayed == false)
        {
            animation.CrossFade(phaseClip.name, 0.1f);
            //animation.Play(curMovement.animationClips[2].name);
            //animation[curMovement.animationClips[2].name].speed = curMovement.animationClips[2].length / curMovement.accDuration;
            /*
            curVelocity -= acceleration * Time.deltaTime;
            remainingDist -= curVelocity;
             */ 
            phasePlayed = true;
        }
        else
        {
            if (!animation.IsPlaying(phaseClip.name))
            {
                phaseCtr++;
            }
                /*
            else
            {
                curVelocity -= acceleration * Time.deltaTime;
                if (curVelocity < 0.0f)
                    curVelocity = 0.0f;
                remainingDist -= curVelocity * Time.deltaTime;
            }
                 */
        }
        /*
        curCharacterScript.custom_translate(curMovement.movementDirection * Time.deltaTime * curVelocity);
        remainingDist -= curVelocity * Time.deltaTime;
        */
    }


    void internal_deceleration()
    {
        if (phasePlayed == false)
        {

            decelerationTime = 1.0f * curVelocity / curMovement.decelerationValue;
            animation.CrossFade(curMovement.animationClips[curMovement.animationClips.Length - 1].name, 0.1f);
            AnimationState accAnimation = animation[curMovement.animationClips[curMovement.animationClips.Length - 1].name];
            accAnimation.speed = 0.5f;
            accAnimation.speed = 1.0f * (accAnimation.speed * accAnimation.length) / decelerationTime;
            phaseMaxDist =  ((decelerationTime * curVelocity)
                    + (1.0f / 2.0f) * (-curMovement.decelerationValue) * decelerationTime * decelerationTime);
            Debug.Log("Phase max dist on deceleration: " + phaseMaxDist);
            phasePlayed = true;
        }
        if (curVelocity > 0.0f)
        {
            float tempVelocityHolder = curVelocity;
            curVelocity -= (1.0f) * curMovement.decelerationValue * Time.deltaTime;
            float distanceTraveled = ((Time.deltaTime * curVelocity)
                + (1.0f / 2.0f) * (-curMovement.decelerationValue) * Time.deltaTime * Time.deltaTime);
           

            if (phaseMaxDist > distanceTraveled && distanceTraveled > 0.0f)
            {
                transform.Translate(curMovement.movementDirection * distanceTraveled);
            }
            else
            {
                transform.Translate(curMovement.movementDirection * phaseMaxDist);
                curVelocity = 0.0f;
            }
            phaseMaxDist -= distanceTraveled;
        }
        else
        {
            phasePlayed = false;
            phaseCtr++;
        }
    }


    public bool run_movement_3()
    {
        if (phaseCtr == 0)
        {
            acceleration_phase();
        }
        else if (phaseCtr == 1)
        {
            cruising_phase();
        }
        else if (phaseCtr == 2)
        {
            deceleration_phase(curMovement.animationClips[2]);
        }
        else
        {
            return false;
        }
        return true; 
    }

    public bool run_movement_2()
    {
        if (remainingDist < 0.0f && remainingDist != -1.0f) {
            return false;
        }
        if (remainingDist != -1.0f)
        {
            if (phaseCtr == 0)
            {
                acceleration_phase();
            }
            else if (phaseCtr == 1)
            {
                deceleration_phase(curMovement.animationClips[1]);
            }
            else
            {
                return false;
            }
            return true;
        }
        else
        {
            if (phaseCtr == 0)
                internal_acceleration_phase();
            else if (phaseCtr == 1)
                internal_deceleration();
            else
            {
                return false;
            }
            
            
            return true;
        }
    }

    public bool run_movement_1()
    {
        if (phaseCtr == 0 && remainingDist > 0.0f)
        {
            animation.CrossFade(curMovement.animationClips[0].name, 0.1f);
            //animation.Play(curMovement.animationClips[0].name);
            if (remainingDist <= 0.0f)
            {
                phaseCtr++;
            }
        }
        else
        {
            return false;
        }
        curCharacterScript.custom_translate(curMovement.movementDirection.normalized * curVelocity * Time.deltaTime);
        remainingDist -= curVelocity * Time.deltaTime;
        return true;
    }


    public bool no_animation_movement()
    {
        if (remainingDist > 0.0f)
        {
            remainingDist -= (curMovement.movementDirection.normalized * curVelocity * Time.deltaTime).magnitude;
            curCharacterScript.custom_translate(curMovement.movementDirection.normalized * curVelocity * Time.deltaTime);
            return true;
        }
        return false;
    }

    public bool charge_enemy()
    {
        if ((curCharacterScript.target.transform.position - transform.position).magnitude > distRange)
        {
            animation.CrossFade(curMovement.animationClips[0].name, 0.1f);
        }
        else
        {
            phaseCtr++;
            return false;
        }
        curCharacterScript.custom_translate(curMovement.movementDirection.normalized * curVelocity * Time.deltaTime);
        remainingDist -= curVelocity * Time.deltaTime;
        return true;
    }

    


    public bool run_movement()
    {
        if (remainingDist <= 0.0f && remainingDist != -1 &&
            phaseCtr < curMovement.animationClips.Length - 1)
        {
            
            phasePlayed = false;
            phaseCtr = curMovement.animationClips.Length - 1;
        }
        if (checkRelativeDist == true)
        {
            return charge_enemy();
        }
        else if (curMovement.animationClips.Length == 3)
        {
            return run_movement_3();
        }
        else if (curMovement.animationClips.Length == 2)
        {
            return run_movement_2();
        }
        else if (curMovement.animationClips.Length == 1)
        {
            return run_movement_1();
        }
        else if (curMovement.animationClips.Length == 0)
        {
            return no_animation_movement();
        }
        return false;
    }


	// Use this for initialization
	public void initialize_script () {
        curCharacterScript = this.GetComponent<Character>();
        foreach (MovementData moveData in movementCollection)
        {
            movementLibrary[moveData.movementID] = moveData;
            Debug.Log("Movement " + moveData.movementID + " Initialized");
        }

	}
	
	// Update is called once per frame
	void Update () {
	}
}
