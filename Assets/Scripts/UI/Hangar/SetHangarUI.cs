using UnityEngine;
using System.Collections;

public class SetHangarUI : MonoBehaviour {
	
	GameObject hangarsign;
	GameObject exit;
	GameObject equip;
	GameObject abilities;
	GameObject armory;
	GameObject allies;
	GameObject equipwindow;
	GameObject decoration;
	GameObject on_screen;
	
	Camera camera;
	public float zValue = 10f;
	// Use this for initialization
	void Awake () {
		exit = GameObject.Find("Exit");
		hangarsign = GameObject.Find("Hangar");
		equip = GameObject.Find("Equip");
		abilities = GameObject.Find("Abilities");
		armory = GameObject.Find("Armory");
		allies = GameObject.Find ("Allies");
		equipwindow = GameObject.Find ("EquipWindow");
		decoration = GameObject.Find("Decoration");
		on_screen = GameObject.Find("OnScreen");
		zValue = 10f;
		
		camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		
		Vector3 positionExit = camera.ViewportToWorldPoint(new Vector3(1f,0f,zValue));
		exit.transform.position = positionExit;
		
		Vector3 positionHangar = camera.ViewportToWorldPoint(new Vector3(0,1f,zValue));
		hangarsign.transform.position = positionHangar;
		
		Vector3 positionEquip = hangarsign.transform.position+new Vector3(2.75f,0,0);//.GetChild(0).position;
		equip.transform.position = positionEquip;
		
		Vector3 positionAbilities = equip.transform.position+new Vector3(3.1f,0,0);//.GetChild(0).position;
		abilities.transform.position = positionAbilities;
		
		Vector3 positionArmory = abilities.transform.position+new Vector3(3.1f,0,0);//.GetChild(0).position;
		armory.transform.position = positionArmory;
		
		Vector3 positionAllies = armory.transform.position+new Vector3(3.1f,0,0);//.GetChild(0).position;
		allies.transform.position = positionAllies;
		
		Vector3 positionWindow = camera.ViewportToWorldPoint(new Vector3 (.5f, .48f, zValue));
		equipwindow.transform.position = positionWindow;
		on_screen.transform.position = equipwindow.transform.position;
		
		decoration.transform.position = camera.ViewportToWorldPoint(new Vector3(1f,1f,zValue));
		
	}
}
