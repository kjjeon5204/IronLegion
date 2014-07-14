using UnityEngine;
using System.Collections;

public class Orbit : MyProjectile {
	public float speed;
	public float lifespan;
	public float hitDamage;
	public float xTranslate;
	public float yTranslate;
	public float zTranslate;
	
    float destructionTime;
    
	void LookAwayFrom(Vector3 point)
	{
		point = 2.0f * transform.position - point;
		transform.LookAt(point);
	}

	void OnTriggerEnter (Collider hit) {	
		if (hit.gameObject.tag != "Boundary" && hit.gameObject != owner && 
			hit.gameObject.tag != "Projectile" ) {
			if (hit.gameObject.tag == "Character")
				hit.gameObject.GetComponent<Character>().hit (hitDamage);
		}
			
	    if (detonation != null) 
	        GameObject.Instantiate(detonation, transform.position, Quaternion.identity);
	}
	
	
	// Use this for initialization
	void Start () {
        destructionTime = Time.time + lifespan;
		LookAwayFrom(target.transform.position);
		transform.Translate(xTranslate, yTranslate, zTranslate);
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > destructionTime) {
            Destroy(gameObject);
		}
        else {
			transform.RotateAround(target.transform.position, Vector3.up, speed * Time.deltaTime);
        }
	}
}
