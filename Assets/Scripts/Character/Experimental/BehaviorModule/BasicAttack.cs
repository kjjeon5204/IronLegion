using UnityEngine;
using System.Collections;

public class BasicAttack : AIBaseModule {
	MultiSequenceAttack[] multiAttack;
	public AnimationClip[] animations;
	int phaseTracker;
	bool phasePlayed;
	public GameObject projectile;
	public GameObject muzzle;
	public float attackPercentage;
	public float timeTracker;

	//fires projectile
	void projectile_fire() {
	}

	void run_state() {
		if (phasePlayed == false) {
			//check animation
			if (multiAttack[phaseTracker].animationClip != null) {
				//play animation
                animation.Play(multiAttack[phaseTracker].animationClip.name);
			}
			else {
				timeTracker = Time.time + multiAttack[phaseTracker].timeDuration;
			}

			//check projectile
			if (multiAttack[phaseTracker].attackState == true) {
                GameObject projectileObject = (GameObject)Instantiate(projectile, muzzle.transform.position, muzzle.transform.rotation);
                projectileObject.GetComponent<MyProjectile>().set_projectile(character.target, gameObject, character.baseDamage * attackPercentage / 100.0f);
			}
			phasePlayed = true;
		}
		else  {
			//animation condition
			if (multiAttack[phaseTracker].animationClip != null && 
			    !animation.IsPlaying(multiAttack[phaseTracker].animationClip.name)) {
				phaseTracker++;
				phasePlayed = false;
			}
			//time condition
			if (multiAttack[phaseTracker].animationClip == null) {
				if (Time.time > timeTracker) {
					phaseTracker++;
					phasePlayed = false;
				}
			}
		}
	}

	void attack_phase1() {
		//play attack ready animation
		if (phasePlayed == false) {
			phasePlayed = true;
			if (animations.Length > 0 && animations[0] != null)
				animation.Play(animations[0].name);
			else 
				Debug.LogError ("NO ANIMATION SPECIFIED!");
		}
		else {
			if (!animation.IsPlaying(animations[0].name)) {
				phaseTracker++;
				phasePlayed = false;
			}
		}
	}
	
	void attack_phase2() {
		//play attack ready animation
		if (phasePlayed == false) {
			phasePlayed = true;
			if (animations.Length > 0 && animations[1] != null)
				animation.Play(animations[1].name);
			else 
				Debug.LogError ("NO ANIMATION SPECIFIED!");

			//fire projectile
			GameObject projectileObject = (GameObject)Instantiate (projectile, muzzle.transform.position, muzzle.transform.rotation);
			projectileObject.GetComponent<MyProjectile>().set_projectile(character.target, gameObject, character.baseDamage * attackPercentage / 100.0f);
		}
		else {
			if (!animation.IsPlaying(animations[1].name)) {
				phaseTracker++;
				phasePlayed = false;
			}
		}
	}

	void attack_phase3() {
		//play attack ready animation
		if (phasePlayed == false) {
			phasePlayed = true;
			if (animations.Length > 0 && animations[2] != null)
				animation.Play(animations[2].name);
			else 
				Debug.LogError ("NO ANIMATION SPECIFIED!");
		}
		else {
			if (!animation.IsPlaying(animations[2].name)) {
				phaseTracker++;
				phasePlayed = false;
			}
		}
	}
	public override void initialize_module() {
		phaseTracker = 0;
		phasePlayed = false;
	}

	public override bool run_module() {
		/*
		if (phaseTracker < multiAttack.Length) {
			run_state();
			return true;
		}
		else return false;
		*/
		if (phaseTracker == 0)
			attack_phase1();
		else if (phaseTracker == 1)
			attack_phase2();
		else if (phaseTracker == 2)
			attack_phase3();
		else 
			return false;
		return true;
	}

	public override void Start() {
		base.Start();
	}
}
