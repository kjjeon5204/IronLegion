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
            if (detonation != null)
            {
                if (hitEnemyScript.can_receive_detonator())
                {
                    GameObject temp = (GameObject)Instantiate(detonation, transform.position, Quaternion.identity);
                    temp.transform.parent = hit.gameObject.transform;
                }   
            }
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
            Vector3 distanceToMove = bulletSpeed * Vector3.forward * Time.deltaTime;
            Ray myRay = new Ray(transform.position, distanceToMove);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, distanceToMove.normalized, out hit, distanceToMove.magnitude)) 
            {
                Character hitCharacter = hit.collider.gameObject.GetComponent<Character>();
                if (hitCharacter != null && hit.collider.gameObject != owner)
                {
                    hitCharacter.hit(damage, transform.position);
                    if (detonation != null)
                    {
                        if (hitCharacter.can_receive_detonator())
                        {
                            GameObject temp = (GameObject)Instantiate(detonation, transform.position, Quaternion.identity);
                            temp.transform.parent = hit.collider.gameObject.transform;
                        }
                    }
                    Destroy(gameObject);
                }
            }

            transform.Translate(distanceToMove);
            if (destroyTime + 10.0f < Time.time)
            {
                Destroy(this.gameObject);
            }
        }
	}
}
