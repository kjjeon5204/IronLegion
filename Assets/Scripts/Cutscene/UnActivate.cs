using UnityEngine;
using System.Collections;

public class UnActivate : MonoBehaviour {

	int activateHASH = Animator.StringToHash("Activate");
	private Animator anim;
	
	void Start() {
		anim = gameObject.GetComponent<Animator>();
	}
	
	public void TurnOffAnimation() {
		anim.SetBool(activateHASH,false);
	}
}
