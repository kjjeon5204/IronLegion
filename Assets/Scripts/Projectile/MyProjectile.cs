using UnityEngine;
using System.Collections;

public struct ProjectileDataInput {
	//projectile initialization data here
	public Character inTargetScript;
	public GameObject inOwner;
	public float inDamage;
	public GameObject square;
	public float aimAngle;
	public float radius;
}

public class MyProjectile : MonoBehaviour {
    public GameObject detonation;
    protected float damage;
    protected Character targetScript;
    public GameObject target;
	protected GameObject owner;
	protected GameObject Aimingsquare;
	protected float angle;
	protected float radius;
	public float rotSpeed = 180.0f;
    protected bool projectilePaused = false;
    public bool activateProjectile = false;
    public float delayTime;

    public void pause_projectile(bool inputOption)
    {
        projectilePaused = inputOption;
    }

	public void set_projectile(Character inTargetScript, GameObject inOwner, float inDamage, GameObject radarTrack) {
		damage = inDamage;
		owner = inOwner;
		targetScript = inTargetScript;
		target = targetScript.gameObject;
		Radar radarAcc = radarTrack.GetComponent<Radar>();

	}

    public virtual void set_projectile(GameObject targetObject, GameObject inOwner, float inDamage)
    {
        damage = inDamage;
        targetScript = targetObject.GetComponent<Character>();
        owner = inOwner;
        target = targetScript.gameObject;
        if (target == null)
        {
            Debug.LogError("Null target failed to initialize!");
        }
        else
        {
            Debug.Log(name + "succesfully initialized, set to target: " + target.name);
        }
    }


    public virtual void set_projectile(Character inTargetScript, GameObject inOwner, float inDamage)
    {
        damage = inDamage;
        targetScript = inTargetScript;
        owner = inOwner;
        target = targetScript.gameObject;
        if (target == null)
        {
            //Debug.LogError("Null target failed to initialize!");
        }
        else
        {
            //Debug.Log(name + "succesfully initialized, set to target: " + target.name);
        }
    }

	public void set_projectile(ProjectileDataInput myInput, GameObject radarTrack) {
		targetScript = myInput.inTargetScript;
		target = targetScript.gameObject;
		owner = myInput.inOwner;
		if (owner == null)
		{
			Debug.LogError("Not a valid target");
		}
		damage = myInput.inDamage;
		if (myInput.square != null)
			Aimingsquare = myInput.square;
		angle = myInput.aimAngle;
		radius = myInput.radius;
		Radar radarAcc = radarTrack.GetComponent<Radar>();
	}

    public void set_projectile(ProjectileDataInput myInput)
    {
        targetScript = myInput.inTargetScript;
        target = targetScript.gameObject;
		owner = myInput.inOwner;
		if (owner == null)
        {
            Debug.LogError("Not a valid target");
        }
        damage = myInput.inDamage;
		Aimingsquare = myInput.square;
		angle = myInput.aimAngle;
		radius = myInput.radius;
    }
}
