using UnityEngine;
using System.Collections;

public class HellKiteMainBody : Character {
    public HellKiteTurret[] myTurrets;
    public HellKiteTurret tailTurret;
    int spawnPhase = 0;
    Vector3 easingDirection;

    public AutomatedMissilePodScript missilePodLW;
    public AutomatedMissilePodScript missilePodLB;
    public AutomatedMissilePodScript missilePodRW;
    public AutomatedMissilePodScript missilePodRB;

    public GameObject tailGunTurretControl;
   

    bool missileControlsLW = false;
    bool missileControlsLB = false;
    bool missileControlsRB = false;
    bool missileControlsRW = false;
    bool turretControls = false;
    bool tailGunControls = false;

    public GameObject missileProjectile;

   
    

    public override void manual_start()
    {
        float damageHolder = 0.1f * curStats.damage;
        foreach (HellKiteTurret turret in myTurrets)
        {
            turret.initialize_turret(target, damageHolder, gameObject);
        }
        tailTurret.initialize_turret(target, damageHolder, gameObject);
        damageHolder = 0.07f * curStats.damage;
        missilePodLW.initialize_pod(target, 15.0f, gameObject);
        missilePodLB.initialize_pod(target, 15.0f, gameObject);
        missilePodRW.initialize_pod(target, 15.0f, gameObject);
        missilePodRB.initialize_pod(target, 15.0f, gameObject);
        base.manual_start();
    }
	
	// Update is called once per frame
	public override void manual_update () {
        float distanceToTarget = (target.transform.position - transform.position).magnitude;
        Vector3 playerRelativePos = transform.InverseTransformPoint(target.transform.position);

        custom_look_at_3D(target.transform.position);
        transform.Translate(3.0f * Vector3.forward * Time.deltaTime);

        //Determine which weapon systems will be online

        missileControlsLW = false;
        missileControlsLB = false;
        missileControlsRB = false;
        missileControlsRW = false;
        turretControls = false;
        tailGunControls = false;

        if (playerRelativePos.y < 0.0f)
        {
            turretControls = true;
        }
        if (playerRelativePos.x > - 5.0f && playerRelativePos.z > 5.0f)
        {
            missileControlsLB = true;
            missileControlsLW = true;
        }

        if (playerRelativePos.x < 5.0f && playerRelativePos.z > 5.0f)
        {

            missileControlsRB = true;
            missileControlsRW = true;
        }

        if (playerRelativePos.z < 0.0f && playerRelativePos.y > -5.0f)
        {
            tailGunControls = true;
        }

        if (turretControls == true)
        {
            foreach (HellKiteTurret turret in myTurrets)
            {
                turret.update_turret();
            }
        }
        if (tailGunControls == true)
            tailTurret.update_turret();
        if (missileControlsLB == true)
            missilePodLB.update_pod();
        if (missileControlsLW == true)
            missilePodLW.update_pod();
        if (missileControlsRB == true)
            missilePodRB.update_pod();
        if (missileControlsRW == true)
            missilePodRW.update_pod();
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
