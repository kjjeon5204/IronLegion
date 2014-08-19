﻿using UnityEngine;
using System.Collections;

public class MainBodyBoss : Character {
    public GameObject missileProjectile;
    public float missileProjectileDamage;
    public GameObject[] group1MissileMuzzle;
    public GameObject[] group2MissileMuzzle;
    public GameObject[] group3MissileMuzzle;
    public GameObject[] group4MissileMuzzle;

    public AutomatedMissilePodScript missilePodLeft;
    public AutomatedMissilePodScript missilePodRight;
    

    int currentWaveNum;


    public float missileFireRate;
    float missileIntervalTracker;
    float missileTimeTracker;
    float cannonTimeTracker;
    bool phasePlayedMissile;
    int curPhaseMissile;

    bool missileActive = false;
    bool cannonActive= false;
    bool energyBallActive = false;

    public float massiveCannonDamage;
    public GameObject[] massiveMuzzle;
    public GameObject massiveProjectile;
    public GameObject mainBodyCollider;

    public float energyBallDamage;
    public GameObject energyBallProjectile;
    public GameObject[] energyBallMuzzles;
    float energyBallTimeTracker;

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
                   
                    projectileScript.set_projectile(targetScript, gameObject, curStats.baseDamage * missileProjectileDamage / 100.0f);
                    
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

                    projectileScript.set_projectile(targetScript, gameObject, curStats.baseDamage * missileProjectileDamage / 100.0f);

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

                    projectileScript.set_projectile(targetScript, gameObject, curStats.baseDamage * missileProjectileDamage / 100.0f);

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

                    projectileScript.set_projectile(targetScript, gameObject, curStats.baseDamage * missileProjectileDamage / 100.0f);

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
            //scannonActive = true;
            cannonTimeTracker = Time.time + 5.0f;
        }

        if (missileActive == true)
        {
            if (run_missile_attack())
            {
                missileActive = false;
                missileTimeTracker = Time.time + 3.0f;
            }
        }
        if (cannonActive == true && (target.transform.position - transform.position).magnitude > 20.0f)
        {
            foreach (GameObject mainGunMuzzle in massiveMuzzle)
            {

                Vector3 targetPosition = target.transform.position;
                targetPosition.x += Random.Range(-2.0f, 2.0f);
                targetPosition.y += Random.Range(-2.0f, 2.0f);
                targetPosition.z += Random.Range(-2.0f, 2.0f);
                mainGunMuzzle.transform.LookAt(targetPosition);
                GameObject tempProjectile = (GameObject)Instantiate(massiveProjectile,
                    mainGunMuzzle.transform.position, mainGunMuzzle.transform.rotation);

                tempProjectile.GetComponent<MyProjectile>().set_projectile(target, gameObject, curStats.baseDamage * massiveCannonDamage / 100.0f);
                cannonActive = false;
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
        missilePodRight.initialize_pod(target, curStats.baseDamage * missileProjectileDamage / 100.0f, mainBodyCollider);
        missilePodLeft.initialize_pod(target, curStats.baseDamage * missileProjectileDamage / 100.0f, mainBodyCollider);
    }
	
	// Update is called once per frame
    public override void manual_update()
    {
        Vector3 playerRelativePos = transform.InverseTransformPoint(target.transform.position);
        if (missileTimeTracker < Time.time && missileActive == false)
        {
            //missileActive = true;
            //phasePlayedMissile = false;
            //curPhaseMissile = 0;
        }

        if (energyBallTimeTracker < Time.time)
        {

        }
        
        if (missileActive == true)
        {
            if (run_missile_attack())
            {
                missileActive = false;
                missileTimeTracker = Time.time + 3.0f;
            }
        }

        if (energyBallActive == true)
        {
            foreach (GameObject energyMuzzle in energyBallMuzzles)
            {
                GameObject tempProjectile = (GameObject)Instantiate(energyBallProjectile, 
                    energyMuzzle.transform.position, energyMuzzle.transform.rotation);

                tempProjectile.GetComponent<MyProjectile>().set_projectile(target, gameObject, curStats.baseDamage * energyBallDamage / 100.0f);
            }
            energyBallActive = false;
        }
        if (playerRelativePos.x > 0.0f)
            missilePodLeft.update_pod();
        if (playerRelativePos.x < 0.0f)
            missilePodRight.update_pod();
        base.manual_update();
    }
}
