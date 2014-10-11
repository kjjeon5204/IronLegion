using UnityEngine;
using System.Collections;

public class CreateOnStart : MonoBehaviour {
	public GameObject create;
	public bool follow;
	public bool parent;
	public GameObject look_at_object;
	void Start() {
		if (create != null)
		{
			GameObject temp = (GameObject)Instantiate(create,transform.position,transform.rotation);
			if (parent)
			temp.transform.parent = transform;
			if (follow)
			{
				temp.GetComponent<FollowObject>().follow = look_at_object;
				temp.SendMessage("Follow", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
