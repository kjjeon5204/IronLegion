/*
using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	MainChar.PlayerState curplayerState;
	GameObject MyCam;
	public GameObject player;
	public GameObject target;
	public GameObject previoustarget;
	public CharacterAttackType curAttack;
	public string curattacttype;
	public int count = 1;
	Vector3 aimingpt;
	Vector3 targposition;
	bool close;
	bool move_aside;
	bool move_back;
	bool ischangingtarget;
	public bool isturning;
	bool start = true;
	public enum Camera_State
	{
		IDLE,
		BEAM,
		SLASH,
		GATLING,
		SHATTER,
		BLUT,
		RIFLE
	}
	
	Camera_State curState;
	
	float smooth = 5;
	float stopTime;
	private bool BEAM = false;

	
	// Use this for initialization
	void Start () {
		MyCam = gameObject;
		curState = Camera_State.IDLE;
		close = false;
		MainChar playerscript = player.GetComponent<MainChar>();
		previoustarget = playerscript.target;
		ischangingtarget = false;
		curplayerState = playerscript.curState;
	}
	
	// Update is called once per frame
	void Update () {

		MainChar playerscript = player.GetComponent<MainChar>();
		curAttack = playerscript.curAttack;
		target = playerscript.target;
		curattacttype = curAttack.attackID;
		curplayerState = playerscript.curState;
		if (start) {
			previoustarget = target;
			start = false;
		}


		if (BEAM == true)// beam attack as a sample of  special camera movement for attack
		{

			float dis = 5;

			if (count == 1)
			{
				//MyCam.transform.RotateAround(targposition, Vector3.up, 20 * Time.deltaTime);
				
				MyCam.transform.Translate(1.3F * dis * Vector3.back * Time.deltaTime);
				if (stopTime + 0.3f < Time.time)
				{
					count = 2;
					//MyCam.transform.LookAt(targposition);
				}
			}
			if (count == 2 )
				{
					MyCam.transform.RotateAround(player.transform.position, Vector3.up, -60 * Time.deltaTime);
					if (stopTime + 2.55f < Time.time)
					{
						count = 3 ;
					}
				}

			if (count == 3 )
			{
				MyCam.transform.RotateAround(player.transform.position, Vector3.up, 45 * Time.deltaTime);
				if (stopTime + 5.55f < Time.time)
				{
					count = 4 ;
				}
			}
			
			if (count == 4)
				{
					MyCam.transform.Translate(1.3F * dis * Vector3.forward * Time.deltaTime);	
					if (stopTime + 5.85f < Time.time)
						{
						count = 1;
						BEAM = false;
					}
				}
				
			}
	else {
		
		if (curplayerState == MainChar.PlayerState.BEAM) {
			//BEAM = true;
			stopTime = Time.time;
		}
		else if (previoustarget != target && (curplayerState == MainChar.PlayerState.SHATTER || curplayerState == MainChar.PlayerState.BLUTSAUGER
			                                 || curplayerState == MainChar.PlayerState.GATLING || curplayerState == MainChar.PlayerState.AEGIS
			                                 || curplayerState == MainChar.PlayerState.RIFLE || curplayerState == MainChar.PlayerState.BEAM
			                                 || curplayerState == MainChar.PlayerState.MISSILE || curplayerState == MainChar.PlayerState.SLASH
			)) {

					} // enenemy die while attacking


			else if (curplayerState != MainChar.PlayerState.IDLE && curplayerState != MainChar.PlayerState.TURN)// moving the camera slightly
			{
			           previoustarget = target;
							aimingpt = target.transform.position;
							aimingpt -= (aimingpt - player.transform.position).normalized * 2;
							MyCam.transform.LookAt (aimingpt);

							if (Vector3.Distance (target.transform.position, player.transform.position) < 10 && move_aside == false && close == false) {
									move_aside = true;		
									close = true;
								stopTime = Time.time;
						}
						if (Vector3.Distance (target.transform.position, player.transform.position) > 10 && move_back == false && close == true) {
								move_back = true;		
								close = false;
								stopTime = Time.time;
						}

						if (move_back == true) {
								MyCam.transform.Translate (2f * Vector3.right * Time.deltaTime);
								if (stopTime + 0.4f < Time.time) {
										move_back = false;
								}
						}

						if (move_aside == true) {
								MyCam.transform.Translate (2f * Vector3.left * Time.deltaTime);
								if (stopTime + 0.4f < Time.time) {
										move_aside = false;
								}
						}
				}

		}}

		}
*/
