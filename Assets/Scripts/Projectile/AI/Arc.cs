using UnityEngine;
using System.Collections;

public class Arc : MyProjectile {

    float destroyTime;
    Vector3 targetPos;
	float gravity = 100.0f;
	float velvertical = 0;
    public float bulletSpeed = 0.05f;
    Vector3 fireDirection;
	float travelTime = 0;

	
	public float risingDegree (){
		return Mathf.Atan2 (bulletSpeed, velvertical);
	}

	bool hitOntheGround ()
	{
		if (transform.position.y <= 0) {
			if (Aimingsquare != null)
				Aimingsquare.SetActive(false);
			float distance = (target.transform.position - targetPos).magnitude;
			if (distance < radius){
				Character hitEnemyScript = target.gameObject.GetComponent<Character>();
				hitEnemyScript.hit(damage);
				if (detonation != null)
					GameObject.Instantiate(detonation, transform.position, Quaternion.identity);
				Destroy(gameObject);
			}
			//effects!!!!
			return true;
		}
		else {
			return false;
		}
	}

    // Use this for initialization
    void Start()
    {
		//Debug.Log (angle);
		float adjust = 2.4f;
		if (angle > 80){
			angle = 80;
			adjust = 1;
			bulletSpeed = bulletSpeed / 5;
		}
		angle = angle * Mathf.PI / 180;
		velvertical = Mathf.Tan(angle) * bulletSpeed;
        destroyTime = Time.time;
        targetPos = Aimingsquare.transform.position;
        fireDirection = targetPos - transform.position;
        fireDirection.y = 0.0f;
		float dist = fireDirection.magnitude - adjust;
        fireDirection.Normalize();
        fireDirection *= bulletSpeed;
        travelTime = dist / bulletSpeed;
//		velvertical = gravity * travelTime / 2;
		gravity = 2 * velvertical / travelTime;
//		Debug.Log (" projectile : " + angle);
    } 

    // Update is called once per frame
    void Update()
    {
		if (destroyTime + travelTime > Time.time){
			Vector3 move = new Vector3(fireDirection.x, velvertical, fireDirection.z);
			transform.LookAt(transform.position + move);
			move = transform.InverseTransformDirection(move);
			transform.Translate(move * Time.deltaTime);
			velvertical -= gravity * Time.deltaTime;
		} else {
			transform.Translate(Vector3.forward);
			transform.LookAt(targetPos);
		}
		if (hitOntheGround() && destroyTime + 2.0f < Time.time)
		{
			Destroy(this.gameObject);
		}
    }
}
