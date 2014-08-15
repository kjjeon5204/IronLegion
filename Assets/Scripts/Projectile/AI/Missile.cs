using UnityEngine;
using System.Collections;

public class Missile : MyProjectile {
	public float speed;
    public float trackDist = 5.0f;
    bool track = true;
    float destructionTime;
	
		
	
	Vector3 get_xz_component(Vector3 input) {
		Vector3 ret = new Vector3 (input.x, 0.0f, input.z);
		return ret.normalized;
	}
	
	Vector3 get_yz_component(Vector3 input) {
		Vector3 ret = new Vector3 (0.0f, input.y, input.z);
		return ret.normalized;
	}

    public bool custom_look_at(Vector3 inPosition)
    {
        Vector3 targetPosition = inPosition;
        targetPosition.y -= 0.0f;
        Vector3 playerPos = collider.bounds.center;
        float rotAngleY = Vector3.Angle(Vector3.forward, get_xz_component(transform.InverseTransformPoint(targetPosition)));
        float rotAngleX = Vector3.Angle(Vector3.forward, get_yz_component(transform.InverseTransformPoint(targetPosition)));




        if (transform.InverseTransformPoint(targetPosition).z < 0)
        { /*
			Vector3 tempHolder = transform.InverseTransformPoint(targetPosition);
            tempHolder.z *= -1;
            rotAngleX = Vector3.Angle(Vector3.forward, get_yz_component(tempHolder));
            Debug.Log("X rotation modfied!");
            */
            rotAngleX = 0.0f;
        }

        float xRotationValue = rotSpeed * (rotAngleX / (rotAngleX + rotAngleY));
        float yRotationValue = rotSpeed * (rotAngleY / (rotAngleX + rotAngleY));
        /*
        if (Mathf.Abs(rotAngleY) <= yRotationValue * Time.deltaTime &&
            Mathf.Abs(rotAngleX) <= xRotationValue * Time.deltaTime)
        {
            //Debug.Log ("rotation completed");
            //transform.LookAt(targetPosition);
            return false;
        }
        */
        //Debug.Log ("rotation not completed");
        float rotDirectionY = transform.InverseTransformPoint(targetPosition).x;
        float rotDirectionX = transform.InverseTransformPoint(targetPosition).y;

        //Debug.Log ("Rotation value: " + rotSpeed * Time.deltaTime);
        //Debug.Log("Y rotation: " + rotAngleY);
        //Debug.Log("X rotation: " + rotAngleX);
        if (Mathf.Abs(rotAngleY) > 0.0f)
        {
            if (rotAngleY > rotSpeed * Time.deltaTime)
            {
                //Debug.Log("Y axis Rotation Rate: " + rotSpeed * Time.deltaTime);
                if (rotDirectionY > 0)
                {
                    transform.Rotate(Vector3.up, yRotationValue * Time.deltaTime, Space.World);
                }
                else if (rotDirectionY < 0)
                {
                    transform.Rotate(Vector3.down, yRotationValue * Time.deltaTime, Space.World);
                }
            }
            else
            {

                if (rotDirectionY > 0)
                    transform.Rotate(Vector3.down, rotAngleY, Space.World);
                else if (rotDirectionY < 0)
                    transform.Rotate(Vector3.up, rotAngleY, Space.World);
            }
        }
        if (Mathf.Abs(rotAngleX) > 0.0f)
        {
            if (rotAngleX > rotSpeed * Time.deltaTime)
            {
                //Debug.Log("X axis Rotation Rate: " + rotSpeed * Time.deltaTime);
                if (rotDirectionX > 0)
                {
                    transform.Rotate(Vector3.left, xRotationValue * Time.deltaTime/*, Space.World*/);
                }
                else if (rotDirectionX < 0)
                {
                    transform.Rotate(Vector3.right, xRotationValue * Time.deltaTime/*, Space.World*/);
                }
            }
            else
            {
                if (rotDirectionX > 0)
                    transform.Rotate(Vector3.left, rotAngleX/*, Space.World*/);
                else if (rotDirectionX < 0)
                    transform.Rotate(Vector3.right, rotAngleX/*, Space.World*/);
            }
        }

        return true;
    }
	
	void OnTriggerEnter (Collider hit) {
		if (hit.gameObject.tag != "Boundary" && hit.gameObject != owner 
            && hit.gameObject.tag != "Projectile") {
            
            if (hit.gameObject.tag == "Character")
			    hit.gameObject.GetComponent<Character>().hit (damage, transform.position);

            if (detonation != null)
                GameObject.Instantiate(detonation, transform.position, Quaternion.identity);
            Debug.Log("Hit gameobject: " + hit.gameObject);
            Destroy (gameObject);
		}
	}
	
	
	// Use this for initialization
	void Start () {
        destructionTime = Time.time + 4.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (projectilePaused == false)
        {
            if (Time.time > destructionTime)
            {
                Destroy(gameObject);

                if (detonation != null)
                {
                    Instantiate(detonation, transform.position, transform.rotation);
                }

            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            /*
            if (transform.position.y < 0.0f)
            {
                Destroy(gameObject);

                if (detonation != null)
                {
                    Instantiate(detonation, transform.position, transform.rotation);
                }
            }
             * */
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            //if (target != null && transform.InverseTransformPoint(target.transform.position).z > 0) {
            if (track == true && target != null)
                custom_look_at(target.collider.bounds.center);
            //}

            else if (target != null && (target.collider.bounds.center - collider.bounds.center).magnitude < trackDist &&
                transform.InverseTransformPoint(target.collider.bounds.center).z < 3.0f)
            {
                track = false;
            }
        }
	}
}
