using UnityEngine;
using System.Collections;

public class LaserDamage : MyProjectile {
	public float damageInterval;
	public float hitDamage;
	float damageIntervalTracker;
	
	void OnTriggerEnter (Collider hit) {
		if (hit.gameObject.tag != "Boundary" && hit.gameObject.tag != "Projectile" && 
			hit.gameObject.tag != "EnemyAI" && projectilePaused == false) {
			if (damageIntervalTracker < Time.time) {
				if (hit.gameObject.tag == "Character") {
					hit.gameObject.GetComponent<Character>().hit (hitDamage);
					damageIntervalTracker = Time.time + damageInterval;
					if (detonation != null)
						GameObject.Instantiate(detonation, transform.position, Quaternion.identity);
				}
			}
		}
	}

	void OnTriggerStay (Collider hit) {
		if (hit.gameObject.tag != "Boundary" && hit.gameObject.tag != "Projectile" && 
		    hit.gameObject.tag != "EnemyAI" && projectilePaused == false) {
			if (damageIntervalTracker < Time.time) {
				if (hit.gameObject.tag == "Character") {
					hit.gameObject.GetComponent<Character>().hit (hitDamage);
					damageIntervalTracker = Time.time + damageInterval;
					if (detonation != null)
						GameObject.Instantiate(detonation, transform.position, Quaternion.identity);
				}
			}
		}
	}

	void Start() {
        if (projectilePaused == false)
        {
            damageIntervalTracker = Time.time + damageInterval;
        }
	}
}
