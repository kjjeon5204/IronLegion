using UnityEngine;
using System.Collections;

public class AllyCardInfo : MonoBehaviour {

	public AllyData currentAlly;
	AllyDataList dataList;
	
	public GameObject modelRotate;
	public GameObject equip;
	public GameObject equipped;
	public TextMesh cardName;
	public TextMesh description;
	public TextMesh level;
	public TextMesh hp;
	public TextMesh armor;
	public TextMesh dmg;
	bool inUse = false;
	
	public AIStatScript aiStatScript;
	
	// Use this for initialization
	void Start () {
		dataList = new AllyDataList();
		currentAlly = new AllyData();
		dataList.load_cur_equipped_ally();
		currentAlly = dataList.get_cur_equipped_ally();
		
		hp.GetComponent<Renderer>().sortingLayerName = "Tiles";
		cardName.GetComponent<Renderer>().sortingLayerName = "Tiles";
		description.GetComponent<Renderer>().sortingLayerName = "Tiles";
		armor.GetComponent<Renderer>().sortingLayerName = "Tiles";
		dmg.GetComponent<Renderer>().sortingLayerName = "Tiles";
		level.GetComponent<Renderer>().sortingLayerName = "Tiles";
		
		cardName.text = currentAlly.unitName;
		description.text = "The Minotaur mech is a heavy\narmored unit capable of taking\nmultiple hits to protect allies.";
			
		hp.text = aiStatScript.aiStatTable[currentAlly.level].hp.ToString();
		armor.text = aiStatScript.aiStatTable[currentAlly.level].baseArmor.ToString();
		dmg.text = aiStatScript.aiStatTable[currentAlly.level].baseAttack.ToString();
		level.text = "Lvl. " + aiStatScript.aiStatTable[currentAlly.level].level.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentAlly.unitName == "MINOTAUR" && inUse == false) {
			inUse = true;
		}
		if (inUse == true) {
			equip.active = false;
		}
		else {
			equip.active = true;
		}
		modelRotate.transform.Rotate(Vector3.up * 10 * Time.deltaTime); 
	}
}