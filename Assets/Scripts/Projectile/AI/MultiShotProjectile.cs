using UnityEngine;
using System.Collections;

public class MultiShotProjectile : MyProjectile {
    public int shotCount;
    public float timeBetweenShots;

    public GameObject projectile;

    float timeTracker = 0.0f;
    int curShotCount = 0;
	
	// Update is called once per frame
	void Update () {
        if (timeTracker < Time.time && target != null && target.collider != null)
        {
            transform.LookAt(target.collider.bounds.center);
            GameObject childProjectile = (GameObject)Instantiate(projectile, transform.position, transform.rotation);
            childProjectile.GetComponent<MyProjectile>().set_projectile(target, owner, damage / shotCount);
            timeTracker = Time.time + timeBetweenShots;
            curShotCount++;
        }
        if (curShotCount >= shotCount || target == null)
            Destroy(gameObject);
	}
}
