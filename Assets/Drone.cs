using UnityEngine;
using System.Collections;

public class Drone : MonoBehaviour {

	public GameObject projectile;
	public GameObject muzzle;
	public GameObject target;
	private MonoBehaviour targetScript;

	public float damage;
	public float initialCD = 1.0f;
	float initialCDTracker;
	float globalCD = 3.0f;
	float globalCDTracker;
	
	// Use this for initialization
	void Start() {
		target = GameObject.FindWithTag("Character");
		targetScript = target.GetComponent<MainChar>();
		initialCDTracker = Time.time + initialCD;
		globalCDTracker = Time.time + globalCD;
	}

	// Update is called once per frame
	void Update() {
		//Event Checker
		transform.LookAt(target.transform.position);
		if (globalCDTracker < Time.time && initialCDTracker < Time.time) {
			GameObject projectileAcc = (GameObject)Instantiate(projectile, muzzle.transform.position,
			                                                   muzzle.transform.rotation);
			projectileAcc.GetComponent<MyProjectile>().set_projectile(target.GetComponent<MainChar>(), this.gameObject,
			                                                          damage); 	

			globalCDTracker = globalCD + Time.time;
		}
	}
	
	
}