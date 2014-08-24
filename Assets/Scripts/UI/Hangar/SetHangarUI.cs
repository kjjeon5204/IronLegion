using UnityEngine;
using System.Collections;

public class SetHangarUI : MonoBehaviour {
	
	public GameObject hangarsign;
	public GameObject exit;
	public GameObject equip;
	public GameObject abilities;
	public GameObject armory;
	public GameObject allies;
	public GameObject equipwindow;
	public GameObject decoration;
	public GameObject on_screen;
	
	public Camera camera;
	public float zValue;
	// Use this for initialization
	
	private PlayerDataReader tutorial_data;
	void Awake () {
		zValue = 10f;
		
		Vector3 positionExit = camera.ViewportToWorldPoint(new Vector3(1f,0f,zValue));
		exit.transform.position = positionExit;
		
		Vector3 positionHangar = camera.ViewportToWorldPoint(new Vector3(0,1f,zValue));
		hangarsign.transform.position = positionHangar;
		
		Vector3 positionEquip = hangarsign.transform.position+new Vector3(2.75f,0,0);//.GetChild(0).position;
		equip.transform.position = positionEquip;
		
		Vector3 positionAbilities = equip.transform.position+new Vector3(2.75f,0,0);//.GetChild(0).position;
		abilities.transform.position = positionAbilities;
		
		Vector3 positionWindow = camera.ViewportToWorldPoint(new Vector3 (.5f, .48f, zValue));
		equipwindow.transform.position = positionWindow;
		on_screen.transform.position = equipwindow.transform.position;
		
		decoration.transform.position = camera.ViewportToWorldPoint(new Vector3(1f,1f,zValue));
		
	}
	
	void Start() {
		tutorial_data = new PlayerDataReader(Application.persistentDataPath);
		
		if (tutorial_data.check_event_played("HANGAR_SECOND"))
		{
            tutorial_data.event_played("HANGAR_SECOND");
			ActivateAllies();
		}
		if (tutorial_data.check_event_played("STORE"))
		{
            tutorial_data.event_played("STORE");
			ActivateArmory();
		}
        tutorial_data.save_data();
	}
	
	public void ActivateArmory() {
		Vector3 positionArmory = allies.transform.position+new Vector3(2.75f,0,0);//.GetChild(0).position;
		armory.transform.position = positionArmory;
	}
	
	public void ActivateAllies() {
		Vector3 positionAllies = abilities.transform.position+new Vector3(2.75f,0,0);//.GetChild(0).position;
		allies.transform.position = positionAllies;
	}
}
