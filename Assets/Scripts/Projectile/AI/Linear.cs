using UnityEngine;
using System.Collections;

public class Linear : MyProjectile {
    float destroyTime;
    public float bulletSpeed;

    public GameObject playerObj;

    


    void OnTriggerEnter(Collider hit)
    {
        Character hitEnemyScript = hit.gameObject.GetComponent<Character>();
        if (hit.gameObject.tag != "Boundary" && hit.gameObject != owner
            && hit.gameObject.tag == "Character" && hit.gameObject.tag != "Projectile")
        {
            hitEnemyScript.hit(damage, transform.position);

            Debug.Log("Hit " + owner);
            if (detonation != null)
                GameObject.Instantiate(detonation, transform.position, Quaternion.identity);
            Destroy(gameObject);

        }
    }

	// Use this for initialization
	void Start () {
        destroyTime = Time.time;
        if (playerObj != null)
        {
            MainChar playerScript = playerObj.GetComponent<MainChar>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (projectilePaused == false)
        {
            transform.Translate(bulletSpeed * Vector3.forward * Time.deltaTime);
            if (destroyTime + 10.0f < Time.time)
            {
                Destroy(this.gameObject);
            }
        }
	}
}
