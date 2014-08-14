using UnityEngine;
using System.Collections;

public class HellKiteMainBody : Character {
    public HellKiteTurret[] myTurrets;
    int spawnPhase = 0;
    Vector3 easingDirection;

    public GameObject[] missilePodLW;
    public GameObject[] missilePodLB;
    public GameObject[] missilePodRW;
    public GameObject[] missilePodRB;

    public GameObject tailGunTurretControl;

    bool missileControlsLW;
    bool missileControlsLB;
    bool missileControlsRB;
    bool missileControlsRW;
    

    public override void manual_start()
    {
        foreach (HellKiteTurret turret in myTurrets)
        {
            turret.initialize_turret(target, 30.0f);
        }
        base.manual_start();
    }
	
	// Update is called once per frame
	public override void manual_update () {
        float distanceToTarget = (target.transform.position - transform.position).magnitude;
        Vector3 playerRelativePos = transform.InverseTransformPoint(target.transform.position);

        custom_look_at_3D(target.transform.position);
        transform.Translate(3.0f * Vector3.forward * Time.deltaTime);

        if (playerRelativePos.y < 0.0f)
        {
            foreach (HellKiteTurret turret in myTurrets)
            {
                turret.update_turret();
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
