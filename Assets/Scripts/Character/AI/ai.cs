using UnityEngine;
using System.Collections;

public class ai : Character {
	private int first = 0;
	public enum AIState{
		IDLE,
		HIT,
		ATTACK1,
		ATTACK2,
		ATTACK3,
		MOVE_FORWARD,
		MOVE_BACK,
		MOVE_LEFT_RIGHT,
		DODGE,
		DEATH,
		ANYSTATE
	}
	
	public enum PlayerState{
		IDLE,
		DEATH,
		DODGELEEFT,
		DODGERIGHT,
		SWITCHCLOSE,
		SWITCHFAR,
		GATTLING_GUN,
		SHATTER,
		BLUTSAUGER,
		ENERGY_BLADE,
		SHOTGUN,
		BARRAGE,
		AEGIS,
		BEAM_CANNON,
		GATTLING_GUN_Other,
		SHATTER_Other,
		BLUTSAUGER_Other,
		ENERGY_BLADE_Other,
		SHOTGUN_Other,
		BARRAGE_Other,
		AEGIS_Other,
		BEAM_CANNON_Other,
		ANYSTATE,
		ATTACK_ME,
		ATTACK_OTHER
	}

	public enum moveType{
		move_forward,
		move_back,
		move_left_right,
		dodge
	}
	
	[System.Serializable]
	public class effect
	{
		public GameObject muzzle;
		public GameObject gunEffect;
	}

	[System.Serializable]
	public class aoeAttack{
		public GameObject AoeSquare;
		public float radias;
	}
	
	[System.Serializable]
	public class attPhase {
		public string phaseID;
		public GameObject bullet;
		public GameObject[] muzzles;
		public bool muzzleAimingPlayer;
		public int DamagePrecentage;
		public AnimationClip animation;
		public effect[] eff;
	}
	
	[System.Serializable]
	public class AttackForm
	{
		public int aoeNum;
		public string attName;
		public attPhase[] attphases;
		public float coolDown;
	}
	
	[System.Serializable]
	public class reactionFSM
	{
		public PlayerState pState;
		public AIState myState;
		public myReaction[] myreactions;
	}
	
	[System.Serializable]
	public class myReaction
	{
		public AIState nextState;
		public float probability;
	}


	

	[System.Serializable]
	public class movePhase
	{
		public GameObject[] moveEffect;
		public AnimationClip animation;
		public float speed;
		public float duration;
	}

	[System.Serializable]
	public class movementForms
	{
		public moveType[] moveTypes;
		public movePhase[] moveAnimation;
	}


	public aoeAttack[] aoe;
	public movementForms[] moveForms;
	private movePhase[] moveAnimationTemp;
	public AttackForm[] attackForms;
	public reactionFSM[] reactions;
	public bool hasTurret;
	public GameObject turret;
	public float turretRotSpeed;
	public float bodyRotSpeed;
	public int setHealth;
	public int setArmor;
	public int setDamage;

	private GameObject[] squares;
	private float attackCD = -1;
	private float starttime;
	private bool animPlaying_death = false;
	private bool animPlaying = false;
	public AIState curState;
	private int transferPhase = 1;
	private int attCount = 0;
	private bool aiming = false;
	private bool shotted = false;
	private string previousAnimation;
	private string previousMoveAnimation;
	private float coolDownTimeStart = Time.time;
	private bool effectDone = false;
	private GameObject AimingSquare;
	private Vector3 movementDir;
	private float explosionRadius = 0;
	private int moveNum = 0;

    

	// s1 <= s2
	bool matchState(PlayerState s1, PlayerState s2){
		if(s1 == s2) return true;
		if(s2 == PlayerState.ANYSTATE) return true;
		if(s2 == PlayerState.ATTACK_ME && 
		   s1 <= PlayerState.BEAM_CANNON && s1 >= PlayerState.GATTLING_GUN) return true;
		if(s2 == PlayerState.ATTACK_OTHER && s1 <= PlayerState.BEAM_CANNON_Other
		   && s1 >= PlayerState.GATTLING_GUN_Other) return true;
		return false;
	}
	
	PlayerState getPlayerState(){
		MainChar playerScript = player.GetComponent<MainChar>();
		string curplayerState = playerScript.curState;
		GameObject	playerTarget = playerScript.target;
		PlayerState tmpPstate = PlayerState.IDLE;
		if (curplayerState ==  "IDLE"){
			tmpPstate = PlayerState.IDLE;
		}
		
		else if (curplayerState ==  "DEATH"){
			tmpPstate = PlayerState.DEATH;
		}
		
		else if (curplayerState ==  "DODGELEEFT"){
			tmpPstate = PlayerState.DODGELEEFT;
		}
		
		else if (curplayerState ==  "DODGERIGHT"){
			tmpPstate = PlayerState.DODGERIGHT;
		}
		
		else if (curplayerState ==  "SWITCHCLOSE"){
			tmpPstate = PlayerState.SWITCHCLOSE;
		}
		
		else if (curplayerState ==  "SWITCHFAR"){
			tmpPstate = PlayerState.SWITCHFAR;
		}
		
		else if (curplayerState ==  "GATTLING_GUN"){
			tmpPstate = PlayerState.GATTLING_GUN;
		}
		
		else if (curplayerState ==  "SHATTER"){
			tmpPstate = PlayerState.SHATTER;
		}
		
		else if (curplayerState ==  "BLUTSAUGER"){
			tmpPstate = PlayerState.BLUTSAUGER;
		}
		
		else if (curplayerState ==  "ENERGY_BLADE"){
			tmpPstate = PlayerState.ENERGY_BLADE;
		}
		
		else if (curplayerState ==  "SHOTGUN"){
			tmpPstate = PlayerState.SHOTGUN;
		}
		
		else if (curplayerState ==  "BARRAGE"){
			tmpPstate = PlayerState.BARRAGE;
		}
		
		else if (curplayerState ==  "AEGIS"){
			tmpPstate = PlayerState.AEGIS;
		}
		
		else if (curplayerState ==  "BEAM_CANNON"){
			tmpPstate = PlayerState.BEAM_CANNON;
		}

		if (tmpPstate <= PlayerState.BEAM_CANNON && tmpPstate >= PlayerState.GATTLING_GUN && gameObject != playerTarget) 
			return tmpPstate + 8;
		
		return tmpPstate;
	}
	
	// get the nextState based on current state and player state
	AIState nextState(){
		//Debug.Log("haoe");
		// go through the fsm array
		for(int i = 0; i < reactions.Length; ++i){
			if((curState == reactions[i].myState || reactions[i].myState == AIState.ANYSTATE) && matchState(getPlayerState(), reactions[i].pState)){
				int randompro = Random.Range(0, 100); 
				float pro = 0;
				for (int j = 0 ; j < reactions[i].myreactions.Length; j++){
					pro += reactions[i].myreactions[j].probability;

					if(randompro <= pro){
						if(reactions[i].myreactions[j].nextState >= AIState.ATTACK1 && reactions[i].myreactions[j].nextState <= AIState.ATTACK3){
							if(coolDownAI()) {
								return reactions[i].myreactions[j].nextState;
							} else {
								return AIState.IDLE;
							} 
						} else if(reactions[i].myreactions[j].nextState == AIState.MOVE_FORWARD){
								return AIState.MOVE_FORWARD;
						} else if (reactions[i].myreactions[j].nextState == AIState.MOVE_BACK){
								return AIState.MOVE_BACK;
						} else if (reactions[i].myreactions[j].nextState == AIState.MOVE_LEFT_RIGHT){
								return AIState.MOVE_LEFT_RIGHT;
						} else if (reactions[i].myreactions[j].nextState == AIState.DODGE){
								return AIState.DODGE;
						}
					}
				}
			}
		}
		
		return AIState.IDLE;
	}
	
	/*********IDLE*********/
	bool idle(){
		animation.Play("idle");
		return true;
	}
	/*********HIT*********/
	bool getHit(){
		if (messageReceived == true){
			messageReceived = false;
			return true;
		}
		return false;
	}
	bool hit(){
		//Debug.Log(name + "remaining health: " + curStats.baseHp);
		if (!animPlaying){
			animation.Play("flinch");
			animPlaying = true;
			return false;
		} else if (!animation.IsPlaying("flinch")){
			animPlaying = false; 
			return true;
		}
		return false;
	}
	/*********ATTACK*********/
	// handler for the bullet
	void bullet_handler(attPhase attack, int aoe){
		GameObject bulletAcc;
		MyProjectile bulletScript;
		Vector3 aim = player.transform.position;
		aim.y = aim.y + 1.0f;
		GameObject[] muzzles = attack.muzzles;
		GameObject bullet = attack.bullet;
		for (int i = 0; i < muzzles.Length; i++) {
			if (attack.muzzleAimingPlayer)
				muzzles[i].transform.LookAt (aim);
			bulletAcc = (GameObject)Instantiate (bullet, muzzles[i].transform.position, muzzles[i].transform.rotation);
			bulletScript = bulletAcc.GetComponent<MyProjectile> ();

			ProjectileDataInput tempData;
			tempData.inTargetScript = playerScript;
			tempData.inOwner = gameObject;
			tempData.inDamage = curStats.baseDamage * attack.DamagePrecentage / 100;
			tempData.aimAngle = Mathf.Abs(muzzles[i].transform.localRotation.eulerAngles.x);
			if (tempData.aimAngle > 180) tempData.aimAngle = 360  - tempData.aimAngle;
			tempData.radius = explosionRadius;
			if (aoe != 0)
				tempData.square = AimingSquare;
			else 
				tempData.square = null;
			bulletScript.set_projectile (tempData);
		}
	}

	float risingDegree (){ // for rotate the turret for shotting arc projectile
		Vector3 fireDirection = player.transform.position - transform.position;
		fireDirection.y = 0.0f;
		float dist = fireDirection.magnitude;
		fireDirection.Normalize();
		fireDirection *= 20.0f;
		float travelTime = dist / 20.0f;
		float velvertical = 10.0f * travelTime / 2;
		return Mathf.Atan2 (20.0f, velvertical);
	}
	
	
	
	// cooldown 
	bool coolDownAI(){
		if(attackCD + coolDownTimeStart < Time.time){
			return true;
		}
		else return false;
	}
	
	//attack phase true for new animaiton, false for the old one
	
	int attackPhase (int numOfAtt, int phase)
	{
		//Debug.Log ("Attack: " + numOfAtt + " Phase: " + phase);
		if (numOfAtt >= attackForms.Length) {
			curState = AIState.IDLE;
			return phase;
		}
		if (attackForms [numOfAtt].attphases.Length <= phase) {
			curState = AIState.IDLE;
			return phase;
		}
		//Debug.Log ("Number of Phase: " + phase);
		if (animation.IsPlaying (previousAnimation)) {
			return phase;
		}
		
		else if (attackForms[numOfAtt].attphases[phase].eff.Length != 0 && effectDone == false)
		{
			for (int i =0 ; i < attackForms[numOfAtt].attphases[phase].eff.Length; i ++)
			{
				GameObject tempObject = (GameObject)Instantiate(attackForms[numOfAtt].attphases[phase].eff[i].gunEffect, attackForms[numOfAtt].attphases[phase].eff[i].muzzle.transform.position,
				            attackForms[numOfAtt].attphases[phase].eff[i].muzzle.transform.rotation);
				tempObject.transform.parent = null;
			}
			effectDone = true;
		}
		else if (attackForms[numOfAtt].attphases[phase].bullet != null && shotted == false)
		{
			bullet_handler(attackForms[numOfAtt].attphases[phase], attackForms[numOfAtt].aoeNum);
			phase ++;
			shotted = true;
			effectDone = false;
		}
		else {
			animation.Play(attackForms[numOfAtt].attphases[phase].animation.name);
			phase ++;
			if (phase < attackForms[numOfAtt].attphases.Length)
				previousAnimation  = attackForms[numOfAtt].attphases[phase].animation.name;
		}
		return phase;
	}
	
	
	
	// attack
	bool attack(int numOfAtt){
		if(!aiming){// aiming 
			if (attackForms[numOfAtt].aoeNum != 0 && effectDone == false) {
				AimingSquare = squares[attackForms[numOfAtt].aoeNum - 1];
				AimingSquare.transform.position = player.transform.position;
				AimingSquare.transform.localScale = new Vector3(1,1,1);
				explosionRadius = aoe[attackForms[numOfAtt].aoeNum - 1].radias;
				AimingSquare.transform.localScale *= explosionRadius;
				AimingSquare.SetActive(true);
				effectDone = true;
			}

			if (!hasTurret) {
				if (!custom_lookAt(player.transform.position - transform.position, 
				                   transform.forward, transform,bodyRotSpeed)) return false;
			}
			else {
				if (!custom_lookAt(player.transform.position - transform.position, 
				                   turret.transform.forward, turret.transform,turretRotSpeed))return false;
			}
			effectDone = false;
			aiming = true;
			animation.Play(attackForms[numOfAtt].attphases[0].animation.name);
			previousAnimation = attackForms[numOfAtt].attphases[0].animation.name;
			attCount = 0;
		} else if (attCount != attackForms[numOfAtt].attphases.Length){
			attCount = attackPhase(numOfAtt, attCount);
		}else {
			aiming = false;
			attCount = 0;
			shotted = false;
			effectDone = false;
			return true;
		}
		attackCD = attackForms[numOfAtt].coolDown;
		coolDownTimeStart = Time.time;
		return false;
	} 
	
	
	/*********MOVE*********/

	bool transfer( moveType type){
//		Debug.Log (transferPhase);
		if (transferPhase == 1)
		{
			moveNum = changeTempAnimation(type);
			transferPhase ++;
			starttime = Time.time;
		}
		if (hasTurret) custom_lookAt (transform.forward, turret.transform.forward, turret.transform, turretRotSpeed);
		int tempPhase = transferPhase / 2 - 1;
        modifyPath = true;
        //Debug.Log("use look at move");
		if (modifyPath == true /*&& type != moveType.move_back*/) {
			//Debug.Log ("Run algorithm");
			movementDir = find_movement_vector();
			//modifyPath = false;
		}

	
		else if (type == moveType.move_back)
			movementDir = player.transform.position - transform.position;



		custom_lookAt (movementDir, transform.forward,  transform, bodyRotSpeed);

		if ((transferPhase % 2) == 0){
			if (moveAnimationTemp.Length > tempPhase ){
				animation.CrossFade(moveAnimationTemp[tempPhase].animation.name, 0.1f);
				previousMoveAnimation = moveAnimationTemp[tempPhase].animation.name;
				starttime = Time.time;
				transferPhase ++;

				if (moveForms[moveNum].moveAnimation[tempPhase].moveEffect.Length != 0) {
					for (int i = 0; i < moveForms[moveNum].moveAnimation[tempPhase].moveEffect.Length; i ++){
						moveForms[moveNum].moveAnimation[tempPhase].moveEffect[i].SetActive(true);
					}
				}

			} else transferPhase = 100;
		}
		else {
			if (!custom_translate(moveAnimationTemp[tempPhase].speed * Vector3.forward * Time.deltaTime)){
				gameObject.transform.LookAt(player.transform.position);
				transform.Translate(Vector3.forward);
			}
			if (starttime + moveAnimationTemp[tempPhase].duration < Time.time && 
			    !animation.IsPlaying(previousMoveAnimation)){
				if (moveForms[moveNum].moveAnimation[tempPhase].moveEffect.Length != 0) {
					for (int i = 0; i < moveForms[moveNum].moveAnimation[tempPhase].moveEffect.Length; i ++){
						moveForms[moveNum].moveAnimation[tempPhase].moveEffect[i].SetActive(false);
					}
				}
				transferPhase ++;
			}
		}

	
		if (transferPhase == 100 ){
			transferPhase = 1;
			return true;
		}
		return false;
	}

	
	bool move_left_right(){
		return transfer(moveType.move_left_right);
	}
	bool move_back(){
		return transfer(moveType.move_back);
	}
	bool move_forward(){
		return transfer(moveType.move_forward);
	}

	bool dodge(){
		return transfer( moveType.dodge);
	}




	bool custom_lookAt (Vector3 toVector, Vector3 fromVector, Transform obj, float rotateSpeed){
		float rotAngle = Vector3.Angle(fromVector, toVector);
		if (Mathf.Abs (rotAngle) < 2) return true;
		//Debug.Log("remaining rotation " + rotAngle);
		Vector3 direction = toVector.normalized;
		//create the rotation we need to be in to look at the target
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		//rotate us over time according to speed until we are in the required rotation
		obj.rotation = Quaternion.Slerp(obj.rotation, lookRotation, Time.deltaTime * rotateSpeed);
		return false;
	}
		
	private int changeTempAnimation (moveType type){
		for (int i = 0 ; i < moveForms.Length; i ++){
			for (int j = 0; j < moveForms[i].moveTypes.Length; j ++){
				if (moveForms[i].moveTypes[j] == type)
				{
					for (int k = 0 ; k < moveForms[i].moveAnimation.Length; k++){
						moveAnimationTemp[k] = moveForms[i].moveAnimation[k];
					}
					return i;
				}
			}
		}
		return 0;
	}

	public override void manual_start()
	{
        AIStatElement aiStat = GetComponent<AIStatScript>().getLevelData(initLevel);
		curState = AIState.IDLE;
        base.manual_start();
		starttime = Time.time;
		gameObject.transform.LookAt(player.transform);
		base.manual_start();
		moveAnimationTemp = new movePhase[3];
		squares = new GameObject[aoe.Length];
		for (int i = 0; i < aoe.Length; i++) {
			squares[i] = (GameObject)Instantiate (aoe[i].AoeSquare,
		                      Vector3.zero, Quaternion.identity);
			squares[i].SetActive (false);
		}
		if (moveForms.Length != 0){
		    for (int i = 0; i < moveForms.Length; i++) {
				if (moveForms[i].moveAnimation.Length != 0){
					for (int k = 0 ; k < moveForms[i].moveAnimation.Length; k++){
						if (moveForms[i].moveAnimation[k].moveEffect.Length != 0){
							for (int j = 0; j < moveForms[i].moveAnimation[k].moveEffect.Length; j ++){
								moveForms[i].moveAnimation[k].moveEffect[j].SetActive(false);
							}
						}
					}
				}
			}
		}
	}
	public override void manual_update(){
			if (curStats.baseHp <= 0){
				curState = AIState.DEATH;
			}
			else if(getHit()){
				curState = AIState.HIT;
			}

			if(curState == AIState.IDLE){
				idle();
				curState = nextState();
			} else if(curState == AIState.HIT){
				if (hit()){
					curState = AIState.IDLE;
					curState = nextState();
				}
			} else if(curState == AIState.ATTACK1){
				//Debug.Log ("start attack--1!!");
				if(attack(0)){
					curState = AIState.IDLE;
					curState = nextState();
				}
			} else if(curState == AIState.ATTACK2){
				if(attack(1) && attackForms.Length >= 2){
					curState = AIState.IDLE;
					curState = nextState();
				}
			} else if(curState == AIState.ATTACK3){
				if(attack(2) && attackForms.Length == 3){
					curState = AIState.IDLE;
					curState = nextState();
				}
			}else if(curState == AIState.MOVE_FORWARD){
				if(move_forward()){
					curState = AIState.IDLE;
					curState = nextState();
				}
			} else if(curState == AIState.MOVE_BACK){
				if(move_back()){
					curState = AIState.IDLE;
					curState = nextState();
				}
			} else if(curState == AIState.MOVE_LEFT_RIGHT){
				if (move_left_right()){
					curState = AIState.IDLE;
					curState = nextState();
				}
			} else if(curState == AIState.DODGE){
				if (dodge()){
					curState = AIState.IDLE;
					curState = nextState();
				}
			} else if(curState == AIState.DEATH){
				death_state();
			}
		}		
}