using UnityEngine;
using System.Collections;

public class AutomatedMissilePodScript : MonoBehaviour
{
    bool initialized = false;
    public GameObject[] missileLaunchMuzzle;
    public GameObject missileProjectile;

    GameObject myTarget;

    float attackTimeTracker;
    float missileFireIntervalTracker;
    int missileFireCount;
    float missileDamage;

    bool missileFiring = false;

    GameObject weaponOwner;

    public void initialize_pod(GameObject target, float damage, GameObject owner)
    {
        myTarget = target;
        initialized = true;
        missileDamage = damage;
        weaponOwner = owner;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public void update_pod()
    {
        if (missileFiring == false && attackTimeTracker < Time.time)
        {
            missileFiring = true;
            missileFireIntervalTracker = Time.time;
            missileFireCount = 0;
        }
        if (missileFiring == true)
        {
            if (missileFireIntervalTracker < Time.time && missileFireCount < missileLaunchMuzzle.Length)
            {
                Vector3 eulerAngles = missileLaunchMuzzle[missileFireCount].transform.eulerAngles;
                eulerAngles.z = 0.0f;
                Quaternion missileOrientation = Quaternion.Euler(eulerAngles);

                GameObject missile = (GameObject)Instantiate(missileProjectile,
                    missileLaunchMuzzle[missileFireCount].transform.position,
                    missileOrientation);

                missile.GetComponent<MyProjectile>().set_projectile(myTarget, weaponOwner, missileDamage);

                missileFireIntervalTracker = Time.time + 0.3f;
                missileFireCount++;
            }
            if (missileFireCount >= missileLaunchMuzzle.Length)
            {
                missileFiring = false;
                attackTimeTracker = Time.time + 5.0f;
            }
        }
    }
}
