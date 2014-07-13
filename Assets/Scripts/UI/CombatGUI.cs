/*
using UnityEngine;
using System.Collections;

public class CombatGUI : MonoBehaviour {

	public float maxHP;
	public float curHP;

	private Vector2 HPbarPOS;
	private Vector2 HPbarSIZE;
	private float maxHPbarLEN;
	private float curHPbarLEN;
	public Texture2D HPframe;
	public Texture2D HPfill_high;
	public Texture2D HPfill_med;
	public Texture2D HPfill_low;

	private Vector2 changeSIZE;
	private Vector2 isDistanceSIZE;
	public Texture2D change_to_Close;
	public Texture2D change_to_Far;
	public Texture2D isClose;
	public Texture2D isFar;
	public Texture2D changeTarget;

	private SkillData[] close_skills;
	private SkillData[] far_skills;

	private Vector2 slotSIZE;
	private Vector2 slot1POS;
	private Vector2 slot2POS;
	private Vector2 slot3POS;
	private Vector2 slot4POS;
	private Texture2D[] abilityLIST;
	private Texture2D[] abilityOffLIST;
	private GameObject world;
	private EventControls world_controls;
	private GameData data;

	public Texture2D skill_fill;
	public Vector2 skill_fillPOS;
	public Vector2 skill_fillSIZE;
	private float[] skill_CD;
	public GUISkin blank;
	public Texture2D skill1;
	public Texture2D skill2;
	public Texture2D skill3;
	public Texture2D skill4;
	public Texture2D skill5;
	public Texture2D skill6;
	public Texture2D skill7;
	public Texture2D skill8;
	public Texture2D skill1off;
	public Texture2D skill2off;
	public Texture2D skill3off;
	public Texture2D skill4off;
	public Texture2D skill5off;
	public Texture2D skill6off;
	public Texture2D skill7off;
	public Texture2D skill8off;

	private float scale;

	// Use this for initialization
	void Start () {
		abilityLIST = new Texture2D[8];
		abilityLIST[0] = skill1;
		abilityLIST[1] = skill2; 
		abilityLIST[2] = skill3; 
		abilityLIST[3] = skill4; 
		abilityLIST[4] = skill5; 
		abilityLIST[5] = skill6; 
		abilityLIST[6] = skill7; 
		abilityLIST[7] = skill8;

		abilityOffLIST = new Texture2D[8];
		abilityOffLIST[0] = skill1off;
		abilityOffLIST[1] = skill2off; 
		abilityOffLIST[2] = skill3off; 
		abilityOffLIST[3] = skill4off; 
		abilityOffLIST[4] = skill5off; 
		abilityOffLIST[5] = skill6off; 
		abilityOffLIST[6] = skill7off; 
		abilityOffLIST[7] = skill8off; 
		
		slotSIZE = new Vector2(150f,88f);
		scale = Screen.height*0.25f/slotSIZE.y;
		slotSIZE.x = slotSIZE.x*scale;
		slotSIZE.y = slotSIZE.y*scale;
		slot1POS = new Vector2(Screen.width-slotSIZE.x,0);
		slot2POS = new Vector2(Screen.width-slotSIZE.x,Screen.height*0.25f);
		slot3POS = new Vector2(Screen.width-slotSIZE.x,Screen.height*0.5f);
		slot4POS = new Vector2(Screen.width-slotSIZE.x,Screen.height*0.75f);
		skill_CD = new float[8];

		skill_fillPOS = new Vector2(slot1POS.x+(30f*scale), 10f*scale);
		skill_fillSIZE = new Vector2(90f*scale, 10f*scale);

		HPbarPOS = new Vector2(0,0);
		HPbarSIZE = new Vector2(600f*scale,25f*scale);
		maxHPbarLEN = 580f*scale;

        changeSIZE = new Vector2(116f*scale, 102f*scale);
        isDistanceSIZE = new Vector2(109f*scale, 41f*scale);

		world = GameObject.Find("World");
		world_controls = world.GetComponent<EventControls>();
		data = world_controls.get_game_data();

		curHP = (float)data.curStats.baseHp;
		maxHP = (float)data.baseStats.baseHp;
		close_skills = data.closeRangeSkills;
		far_skills = data.farRangeSkills;
	}

	void OnGUI() {

		GUI.skin = blank;
		data = world_controls.get_game_data();
		curHPbarLEN = maxHPbarLEN*(curHP/maxHP);
		
		curHP = (float)data.curStats.baseHp;
		maxHP = (float)data.baseStats.baseHp;
		
		close_skills = data.closeRangeSkills;
		far_skills = data.farRangeSkills;
        //-----------------------------------------
		if (curHP/maxHP > 0.67f)
			GUI.DrawTexture(new Rect(HPbarPOS.x, HPbarPOS.y, curHPbarLEN, HPbarSIZE.y), HPfill_high);
		else if (curHP/maxHP > 0.34f)
			GUI.DrawTexture(new Rect(HPbarPOS.x, HPbarPOS.y, curHPbarLEN, HPbarSIZE.y), HPfill_med);
		else
			GUI.DrawTexture(new Rect(HPbarPOS.x, HPbarPOS.y, curHPbarLEN, HPbarSIZE.y), HPfill_low);
		GUI.DrawTexture(new Rect(HPbarPOS.x, HPbarPOS.y, HPbarSIZE.x, HPbarSIZE.y), HPframe);
        //------------------------------------------
		GUI.DrawTexture(new Rect(0,Screen.height-(2f*changeSIZE.y),changeSIZE.x,changeSIZE.y),changeTarget);
		if (GUI.Button(new Rect(0,Screen.height-(2f*changeSIZE.y),changeSIZE.x,changeSIZE.y),"")) {
			world_controls.send_command("button9");
		}
		if (GUI.Button(new Rect(0, Screen.height-changeSIZE.y,changeSIZE.x,changeSIZE.y), "")) {
			world_controls.send_command("button10");
		}

		if (data.close) {
			skill_CD[0] = (close_skills[0].maxCD-close_skills[0].curCD)/close_skills[0].maxCD*skill_fillSIZE.x;
			skill_CD[1] = (close_skills[1].maxCD-close_skills[1].curCD)/close_skills[1].maxCD*skill_fillSIZE.x;
			skill_CD[2] = (close_skills[2].maxCD-close_skills[2].curCD)/close_skills[2].maxCD*skill_fillSIZE.x;
			skill_CD[3] = (close_skills[3].maxCD-close_skills[3].curCD)/close_skills[3].maxCD*skill_fillSIZE.x;

			GUI.DrawTexture(new Rect(skill_fillPOS.x,skill_fillPOS.y+slot1POS.y,skill_CD[0],skill_fillSIZE.y),skill_fill);
			GUI.DrawTexture(new Rect(skill_fillPOS.x,skill_fillPOS.y+slot2POS.y,skill_CD[1],skill_fillSIZE.y),skill_fill);
			GUI.DrawTexture(new Rect(skill_fillPOS.x,skill_fillPOS.y+slot3POS.y,skill_CD[2],skill_fillSIZE.y),skill_fill);
			GUI.DrawTexture(new Rect(skill_fillPOS.x,skill_fillPOS.y+slot4POS.y,skill_CD[3],skill_fillSIZE.y),skill_fill);
			
			GUI.DrawTexture(new Rect(0, Screen.height-changeSIZE.y,changeSIZE.x,changeSIZE.y), change_to_Far);
			GUI.DrawTexture(new Rect(changeSIZE.x, Screen.height-(changeSIZE.y*0.5f), isDistanceSIZE.x,isDistanceSIZE.y), isClose);

			if (close_skills[0].curCD <= 0) {
				GUI.DrawTexture(new Rect(slot1POS.x, slot1POS.y, slotSIZE.x, slotSIZE.y), abilityLIST[close_skills[0].indice]);
				if (GUI.Button(new Rect(slot1POS.x, slot1POS.y, slotSIZE.x, slotSIZE.y), "")) {
					world_controls.send_command("button1");
				}
			}
			else {
				GUI.DrawTexture(new Rect(slot1POS.x, slot1POS.y, slotSIZE.x, slotSIZE.y), abilityOffLIST[close_skills[0].indice]);
			}
			//---------
			if (close_skills[1].curCD <= 0) {
				GUI.DrawTexture(new Rect(slot2POS.x, slot2POS.y, slotSIZE.x, slotSIZE.y), abilityLIST[close_skills[1].indice]);
				if (GUI.Button(new Rect(slot2POS.x, slot2POS.y, slotSIZE.x, slotSIZE.y), "")) {
					world_controls.send_command("button2");
				}
			}
			else {
				GUI.DrawTexture(new Rect(slot2POS.x, slot2POS.y, slotSIZE.x, slotSIZE.y), abilityOffLIST[close_skills[1].indice]);
			}
			//---------
			if (close_skills[2].curCD <= 0) {
				GUI.DrawTexture(new Rect(slot3POS.x, slot3POS.y, slotSIZE.x, slotSIZE.y), abilityLIST[close_skills[2].indice]);
				if (GUI.Button(new Rect(slot3POS.x, slot3POS.y, slotSIZE.x, slotSIZE.y), "")) {
					world_controls.send_command("button3");
				}
			}
			else {
				GUI.DrawTexture(new Rect(slot3POS.x, slot3POS.y, slotSIZE.x, slotSIZE.y), abilityOffLIST[close_skills[2].indice]);
			}
			//---------
			if (close_skills[3].curCD <= 0) {
				GUI.DrawTexture(new Rect(slot4POS.x, slot4POS.y, slotSIZE.x, slotSIZE.y), abilityLIST[close_skills[3].indice]);
				if (GUI.Button(new Rect(slot4POS.x, slot4POS.y, slotSIZE.x, slotSIZE.y), "")) {
					world_controls.send_command("button4");
				}
			}
			else {
				GUI.DrawTexture(new Rect(slot4POS.x, slot4POS.y, slotSIZE.x, slotSIZE.y), abilityOffLIST[close_skills[3].indice]);
			}
			//-------------------------------------------------------------------------------------------
		}
		else {
			skill_CD[4] = (far_skills[0].maxCD-far_skills[0].curCD)/far_skills[0].maxCD*skill_fillSIZE.x;
			skill_CD[5] = (far_skills[1].maxCD-far_skills[1].curCD)/far_skills[1].maxCD*skill_fillSIZE.x;
			skill_CD[6] = (far_skills[2].maxCD-far_skills[2].curCD)/far_skills[2].maxCD*skill_fillSIZE.x;
			skill_CD[7] = (far_skills[3].maxCD-far_skills[3].curCD)/far_skills[3].maxCD*skill_fillSIZE.x;
			
			GUI.DrawTexture(new Rect(skill_fillPOS.x,skill_fillPOS.y+slot1POS.y,skill_CD[4],skill_fillSIZE.y),skill_fill);
			GUI.DrawTexture(new Rect(skill_fillPOS.x,skill_fillPOS.y+slot2POS.y,skill_CD[5],skill_fillSIZE.y),skill_fill);
			GUI.DrawTexture(new Rect(skill_fillPOS.x,skill_fillPOS.y+slot3POS.y,skill_CD[6],skill_fillSIZE.y),skill_fill);
			GUI.DrawTexture(new Rect(skill_fillPOS.x,skill_fillPOS.y+slot4POS.y,skill_CD[7],skill_fillSIZE.y),skill_fill);
			GUI.DrawTexture(new Rect(0, Screen.height-changeSIZE.y,changeSIZE.x,changeSIZE.y), change_to_Close);
			GUI.DrawTexture(new Rect(changeSIZE.x, Screen.height-(changeSIZE.y*0.5f), isDistanceSIZE.x,isDistanceSIZE.y), isFar);

			if (far_skills[0].curCD <= 0) {
				GUI.DrawTexture(new Rect(slot1POS.x, slot1POS.y, slotSIZE.x, slotSIZE.y), abilityLIST[far_skills[0].indice]);
				if (GUI.Button(new Rect(slot1POS.x, slot1POS.y, slotSIZE.x, slotSIZE.y),"")) {
					world_controls.send_command("button5");
				}
			}
			else {
				GUI.DrawTexture(new Rect(slot1POS.x, slot1POS.y, slotSIZE.x, slotSIZE.y), abilityOffLIST[far_skills[0].indice]);
			}
			//---------
			if (far_skills[1].curCD <= 0) {
				GUI.DrawTexture(new Rect(slot2POS.x, slot2POS.y, slotSIZE.x, slotSIZE.y), abilityLIST[far_skills[1].indice]);
				if (GUI.Button(new Rect(slot2POS.x, slot2POS.y, slotSIZE.x, slotSIZE.y), "")) {
					world_controls.send_command("button6");
				}
			}
			else {
				GUI.DrawTexture(new Rect(slot2POS.x, slot2POS.y, slotSIZE.x, slotSIZE.y), abilityOffLIST[far_skills[1].indice]);
			}
			//---------
			if (far_skills[2].curCD <= 0) {
				GUI.DrawTexture(new Rect(slot3POS.x, slot3POS.y, slotSIZE.x, slotSIZE.y), abilityLIST[far_skills[2].indice]);
				if (GUI.Button(new Rect(slot3POS.x, slot3POS.y, slotSIZE.x, slotSIZE.y), "")) {
					world_controls.send_command("button7");
				}
			}
			else {
				GUI.DrawTexture(new Rect(slot3POS.x, slot3POS.y, slotSIZE.x, slotSIZE.y), abilityOffLIST[far_skills[2].indice]);
			}
			//---------
			if (far_skills[3].curCD <= 0) {
				GUI.DrawTexture(new Rect(slot4POS.x, slot4POS.y, slotSIZE.x, slotSIZE.y), abilityLIST[far_skills[3].indice]);
				if (GUI.Button(new Rect(slot4POS.x, slot4POS.y, slotSIZE.x, slotSIZE.y), "")) {
					world_controls.send_command("button8");
				}
			}
			else {
				GUI.DrawTexture(new Rect(slot4POS.x, slot4POS.y, slotSIZE.x, slotSIZE.y), abilityOffLIST[far_skills[3].indice]);
			}
			//-------------------------------------------------------------------------------------------
		}
	}
}
 * */
