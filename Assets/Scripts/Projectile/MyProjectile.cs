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
    protected GameObject target;
	protected GameObject owner;
	protected GameObject Aimingsquare;
	protected float angle;
	protected float radius;
	public GameObject radarIcon;
	public bool isStealth = true;
	public float rotSpeed = 180.0f;
    protected bool projectilePaused = false;

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

		if (isStealth == false) {
			radarAcc.add_object(gameObject, radarIcon);
		}
	}




    public void set_projectile(Character inTargetScript, GameObject inOwner, float inDamage)
    {
        damage = inDamage;
        targetScript = inTargetScript;
        owner = inOwner;
        target = targetScript.gameObject;
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
		if (isStealth == false) {
			radarAcc.add_object(gameObject, radarIcon);
		}
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
