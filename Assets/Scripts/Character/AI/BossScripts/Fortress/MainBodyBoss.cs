using UnityEngine;
using System.Collections;

public class MainBodyBoss : Character {
    public GameObject missileProjectile;
    public float missileProjectileDamage;
    public GameObject[] group1MissileMuzzle;
    public GameObject[] group2MissileMuzzle;
    public GameObject[] group3MissileMuzzle;
    public GameObject[] group4MissileMuzzle;

    

    int currentWaveNum;

    public float missileFireRate;
    float missileIntervalTracker;
    float missileTimeTracker;
    float cannonTimeTracker;
    bool phasePlayedMissile;
    int curPhaseMissile;

    bool missileActive = false;
    bool cannonActive= false;

    bool run_missile_attack()
    {
        if (curPhaseMissile == 0)
        {
            if (phasePlayedMissile == false)
            {
                missileIntervalTracker = Time.time + missileFireRate;
                foreach (GameObject muzzle in group1MissileMuzzle)
                {
                    GameObject projectileHolder = (GameObject)Instantiate(missileProjectile,
                        muzzle.transform.position, muzzle.transform.rotation);
                    MyProjectile projectileScript = projectileHolder.GetComponent<MyProjectile>();
                    float damage = missileProjectileDamage * curStats.baseDamage / 100.0f;
                   
                    projectileScript.set_projectile(targetScript, gameObject, damage);
                    
                }
                phasePlayedMissile = true;
            }
            else
            {
                if (Time.time > missileIntervalTracker)
                {
                    phasePlayedMissile = false;
                    curPhaseMissile++;
                }
            }
            return false;
        }
        else if (curPhaseMissile == 1)
        {
            if (phasePlayedMissile == false)
            {
                missileIntervalTracker = Time.time + missileFireRate;
                foreach (GameObject muzzle in group2MissileMuzzle)
                {
                    GameObject projectileHolder = (GameObject)Instantiate(missileProjectile,
                        muzzle.transform.position, muzzle.transform.rotation);
                    MyProjectile projectileScript = projectileHolder.GetComponent<MyProjectile>();
                    float damage = missileProjectileDamage * curStats.baseDamage / 100.0f;

                    projectileScript.set_projectile(targetScript, gameObject, damage);

                }
                phasePlayedMissile = true;
            }
            else
            {
                if (Time.time > missileIntervalTracker)
                {
                    phasePlayedMissile = false;
                    curPhaseMissile++;
                }
            }
            return false;
        }
        else if (curPhaseMissile == 2)
        {
            if (phasePlayedMissile == false)
            {
                missileIntervalTracker = Time.time + missileFireRate;
                foreach (GameObject muzzle in group3MissileMuzzle)
                {
                    GameObject projectileHolder = (GameObject)Instantiate(missileProjectile,
                        muzzle.transform.position, muzzle.transform.rotation);
                    MyProjectile projectileScript = projectileHolder.GetComponent<MyProjectile>();
                    float damage = missileProjectileDamage * curStats.baseDamage / 100.0f;

                    projectileScript.set_projectile(targetScript, gameObject, damage);

                }
                phasePlayedMissile = true;
            }
            else
            {
                if (Time.time > missileIntervalTracker)
                {
                    phasePlayedMissile = false;
                    curPhaseMissile++;
                }
            }
            return false;
        }
        else if (curPhaseMissile == 3)
        {
            if (phasePlayedMissile == false)
            {
                missileIntervalTracker = Time.time + missileFireRate;
                foreach (GameObject muzzle in group4MissileMuzzle)
                {
                    GameObject projectileHolder = (GameObject)Instantiate(missileProjectile,
                        muzzle.transform.position, muzzle.transform.rotation);
                    MyProjectile projectileScript = projectileHolder.GetComponent<MyProjectile>();
                    float damage = missileProjectileDamage * curStats.baseDamage / 100.0f;

                    projectileScript.set_projectile(targetScript, gameObject, damage);

                }
                phasePlayedMissile = true;
            }
            else
            {
                if (Time.time > missileIntervalTracker)
                {
                    phasePlayedMissile = false;
                    curPhaseMissile++;
                }
            }
        }
        return true;
    }


    public override void first_wave()
    {
        
        if (missileTimeTracker < Time.time && missileActive == false)
        {
            missileActive = true;
            phasePlayedMissile = false;
            curPhaseMissile = 0;
        }
        if (cannonTimeTracker < Time.time)
        {
            cannonActive = true;
        }

        if (missileActive == true)
        {
            if (run_missile_attack())
            {
                missileActive = false;
                missileTimeTracker = Time.time + 3.0f;
            }
        }
        
    }


    public void passive_phase(int waveNum)
    {
        if (waveNum == 0)
        {
            first_wave();
        }
    }

	// Use this for initialization
    public override void manual_start()
    {
        base.manual_start();
        currentWaveNum = 0;
    }
	
	// Update is called once per frame
    public override void manual_update()
    {
        base.manual_update();
    }
}
