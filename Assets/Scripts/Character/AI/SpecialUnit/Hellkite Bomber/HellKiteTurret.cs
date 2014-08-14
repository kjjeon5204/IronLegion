using UnityEngine;
using System.Collections;

public class HellKiteTurret : MonoBehaviour {
    bool initialized = false;

    GameObject myTarget;

    public enum TurretState
    {
        IDLE,
        ATTACK
    }

    TurretState myState;

    public GameObject check_line_of_sight()
    {
        Ray ray = new Ray();
        ray.direction = (myTarget.collider.bounds.center - collider.bounds.center).normalized;
        ray.origin = collider.bounds.center;
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000.0f))
        {
            if (hitData.collider.tag == "Character")
            {
                
                return hitData.collider.gameObject;
            }
        }
        return null;
    }

    float attackInterval = 5.0f;

    public int maxAttackInterval = 5;
    float aggressiveLevel = 0.0f;
    float nextPollTime;
    int machineGunAttackCount;
    float machineGunFireInterval;

    public GameObject muzzle;
    public GameObject projectile;

    float myDamage;

    public void initialize_turret(GameObject target, float damage)
    {
        myTarget = target;
        initialized = true;
        myDamage = damage;
    }



	// Use this for initialization
	void Start () {
	    
	}
	

    public void update_turret()
    {
        if (initialized == true)
        {
            if (myState == TurretState.IDLE)
            {
                aggressiveLevel += 100.0f / (float)maxAttackInterval * Time.deltaTime;
                if (Time.time > nextPollTime)
                {
                    nextPollTime += 1.0f;
                    int randNumber = Random.Range(0, 100);
                    if ((int)aggressiveLevel > randNumber)
                    {
                        Debug.Log("attackTriggered!");
                        //if (check_line_of_sight() == myTarget)
                        myState = TurretState.ATTACK;
                        machineGunAttackCount = 0;
                        machineGunFireInterval = Time.time;
                    }
                }
            }
            else if (myState == TurretState.ATTACK)
            {
                if (machineGunAttackCount < 5 && Time.time > machineGunFireInterval)
                {
                    GameObject projectileHolder = (GameObject)Instantiate(projectile,
                        muzzle.transform.position, muzzle.transform.rotation);

                    projectileHolder.GetComponent<MyProjectile>().set_projectile(myTarget,
                        gameObject, myDamage);
                    machineGunAttackCount++;
                    machineGunFireInterval = Time.time + 0.2f;
                }
                if (machineGunAttackCount >= 5)
                {
                    myState = TurretState.IDLE;

                    aggressiveLevel = 0.0f;
                }
            }
        }
    }
}
