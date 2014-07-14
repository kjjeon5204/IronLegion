using UnityEngine;
using System.Collections;

public class LaserDamage : MyProjectile {
	public float damageInterval;
	public float hitDamage;
	float damageIntervalTracker = 0;
	
	void OnTriggerEnter (Collider hit) {
		
		if (hit.gameObject.tag != "Boundary" && hit.gameObject.tag != "Projectile" ) {
			if (damageIntervalTracker < Time.time) {
				if (hit.gameObject.tag == "Character") {
					hit.gameObject.GetComponent<Character>().hit (hitDamage);
					damageIntervalTracker = Time.time + damageInterval;
				}
			}
		}
		if (detonation != null)
			GameObject.Instantiate(detonation, transform.position, Quaternion.identity);
	}

	void Start() {
		damageIntervalTracker = Time.time + damageInterval;
	}
}
