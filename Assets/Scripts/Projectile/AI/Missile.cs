using UnityEngine;
using System.Collections;

public class Missile : MyProjectile {
	public float speed;
	public float rotSpeed = 180.0f;
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
		float rotAngleY = Vector3.Angle(get_xz_component(transform.forward), get_xz_component(targetPosition - collider.bounds.center));
		float rotAngleX = Vector3.Angle(Vector3.forward , get_yz_component(transform.InverseTransformPoint(targetPosition)));
		
		
		
		if (transform.InverseTransformPoint(targetPosition).z <= 0)
		{
			Vector3 tempHolder = transform.forward;
			tempHolder.z *= -1;
			rotAngleX = Vector3.Angle(get_yz_component(tempHolder), get_yz_component(targetPosition - collider.bounds.center));
			//Debug.Log("X rotation modfied!");
		}
		
		if (Mathf.Abs(rotAngleY) < 1.0f && Mathf.Abs(rotAngleX) < 1.0f)
		{
			return false;
		}
		float rotDirectionY = transform.InverseTransformPoint(targetPosition).x;
		float rotDirectionX = transform.InverseTransformPoint(targetPosition).y;
		
		//Debug.Log("Y rotation: " + rotAngleY);
		//Debug.Log("X rotation: " + rotAngleX);
		if (Mathf.Abs(rotAngleY) > 1.0f)
		{
			if (rotAngleY > rotSpeed * Time.deltaTime)
			{
				//Debug.Log("Y axis Rotation Rate: " + rotSpeed * Time.deltaTime);
				if (rotDirectionY > 0)
				{
					transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime, Space.World);
				}
				else if (rotDirectionY < 0)
				{
					transform.Rotate(Vector3.down * rotSpeed * Time.deltaTime, Space.World);
				}
			}
			else
			{
				if (rotDirectionY > 0)
					transform.Rotate(Vector3.up * rotAngleY, Space.World);
				else if (rotDirectionY < 0)
					transform.Rotate(Vector3.down * rotAngleY, Space.World);
			}
		}
		if (Mathf.Abs(rotAngleX) > 5.0f)
		{
			if (rotAngleX > rotSpeed * Time.deltaTime)
			{
				//Debug.Log("X axis Rotation Rate: " + rotSpeed * Time.deltaTime);
				if (rotDirectionX > 0)
				{
					transform.Rotate(Vector3.left * rotSpeed * Time.deltaTime/*, Space.World*/);
				}
				else if (rotDirectionX < 0)
				{
					transform.Rotate(Vector3.right * rotSpeed * Time.deltaTime/*, Space.World*/);
				}
			}
			else
			{
				if (rotDirectionX > 0)
					transform.Rotate(Vector3.left * rotAngleX/*, Space.World*/);
				else if (rotDirectionX < 0)
					transform.Rotate(Vector3.right * rotAngleX/*, Space.World*/);
			}
		}
		
		return true;
	}
	
	void OnTriggerEnter (Collider hit) {
		if (hit.gameObject.tag != "Boundary" && hit.gameObject != owner 
            && hit.gameObject.tag != "bullet1") {
            
            if (hit.gameObject.tag == "Character")
			    hit.gameObject.GetComponent<Character>().hit (damage);

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
         if (Time.time > destructionTime)
          {
              Destroy(gameObject);

			if (detonation != null) {
				Instantiate (detonation, transform.position, transform.rotation);
 			}
 
          }
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
		if (target != null)
 			custom_look_at(target.collider.bounds.center);
 		//}
        if (transform.position.y < 0.0f)
        {
            Destroy(gameObject);

            if (detonation != null)
            {
                Instantiate(detonation, transform.position, transform.rotation);
            }
        }
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
		//if (target != null && transform.InverseTransformPoint(target.transform.position).z > 0) {
		if (track == true)
           custom_look_at(target.collider.bounds.center);
		//}
        if ((target.collider.bounds.center - collider.bounds.center).magnitude < trackDist)
        {
            track = false;
        }
	}
}
