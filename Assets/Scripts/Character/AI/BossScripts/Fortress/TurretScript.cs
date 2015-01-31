using UnityEngine;
using System.Collections;

public class TurretScript : Character {
    public AnimationClip attackAnimation;
    public AnimationClip idleAnimation;
    public GameObject[] muzzle;
    public GameObject projectile;

    string currentState;

    float nextPollTime;
    public float attackPercentage;
    public float pollTime;

    bool phasePlayed = false;

	// Use this for initialization
	public override void manual_start () {
        nextPollTime = Time.time + pollTime;
        currentState = "IDLE";
		base.manual_start();
	}
	
	// Update is called once per frame
	public override void manual_update () {
        if (curStats.hp <= 0.0f)
        {
            Destroy(gameObject);
        }
        else
        {
            if (check_line_of_sight() == target)
            {
                custom_look_at_3D(target.transform.position);
                if (nextPollTime < Time.time && currentState == "IDLE")
                {
                    nextPollTime = Time.time + pollTime;
                    float pollVal = Random.Range(0, 3);
                    if (check_line_of_sight() == target && muzzle != null
                        && pollVal == 0)
                    {
                        currentState = "ATTACK";
                        phasePlayed = false;
                    }
                }
            }
            if (currentState == "IDLE")
            {
                animation.Play(idleAnimation.name);
            }
            else if (currentState == "ATTACK")
            {
                if (phasePlayed == false)
                {
                    phasePlayed = true;
                    foreach (GameObject muzzleAcc in muzzle)
                    {
                        muzzleAcc.transform.LookAt(target.collider.bounds.center);
                        MyProjectile projectileScript = ((GameObject)Instantiate(projectile,
                            muzzleAcc.transform.position, muzzleAcc.transform.rotation)).
                            GetComponent<MyProjectile>();

                        projectileScript.set_projectile(target, gameObject, curStats.damage * attackPercentage / 100.0f);
                    }
                }
                else
                {
                    if (!animation.IsPlaying(attackAnimation.name))
                    {
                        currentState = "IDLE";
                    }
                }
            }
        }
	}
}
