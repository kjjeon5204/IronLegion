using UnityEngine;
using System.Collections;

public class LaserDamage : MyProjectile {
	public float damageInterval;
	float damageIntervalTracker;
	
	void OnTriggerEnter (Collider hit) {
		
		if (hit.gameObject.tag != "Boundary" && hit.gameObject != owner && hit.gameObject.tag != "Projectile" ) {
			if (damageIntervalTracker < Time.time) {
				hit.gameObject.GetComponent<Character>().hit (damage);
				damageIntervalTracker = Time.time + damageInterval;
			}
		}
		if (detonation != null)
			GameObject.Instantiate(detonation, transform.position, Quaternion.identity);
	}

	void Start() {
		damageIntervalTracker = Time.time + damageInterval;
	}
}
