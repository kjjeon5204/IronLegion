using UnityEngine;
using System.Collections;
using System;

public class DescriptionControl : MonoBehaviour {
	public Sprite icon_sprite;
	private SpriteRenderer icon;
	
	private TextMesh name;
	private TextMesh range;
	private TextMesh target;
	private TextMesh damage;
	private TextMesh leech;
	private TextMesh armor;
	private TextMesh cooldown;
	private TextMesh level;
	private TextMesh message;
	
	public Abilities ability_info;
	// Use this for initialization
	void Start () {
		icon = transform.Find("Icon").GetComponent<SpriteRenderer>();
		
		name = transform.Find("Name").GetComponent<TextMesh>();
		range = transform.Find("Range").GetComponent<TextMesh>();
		target = transform.Find("Target").GetComponent<TextMesh>();
		damage = transform.Find("Damage").GetComponent<TextMesh>();
		leech = transform.Find("Leech").GetComponent<TextMesh>();
		armor = transform.Find("Armor").GetComponent<TextMesh>();
		cooldown = transform.Find("Cooldown").GetComponent<TextMesh>();
		level = transform.Find("Level").GetComponent<TextMesh>();
		message = transform.Find("Message").GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	public void UpdateDescription () {
		icon.sprite = icon_sprite;
	
		string output_string;
		
		output_string = ability_info.name.Replace("_", " ");
		name.text = output_string;
		
		if (ability_info.isClose)
		output_string = "Range: Close";
		else
		output_string = "Range: Long";
		range.text = output_string;
		
		output_string = "Target: "+ability_info.target.Replace("SINGLE","ENEMY");
		target.text = output_string;
		
		if (ability_info.damage_percent != 0)
		output_string = "Damage: "+ ability_info.damage_percent.ToString() + "%";
		else
		output_string = "Heal: " + ability_info.hp_modifier.ToString();
		if (ability_info.frequency > 1)
		output_string = output_string + " x "+ability_info.frequency.ToString();
		damage.text = output_string;
		
		if (ability_info.hp_leech > 0)
		output_string = "Leech: "+ ability_info.hp_leech.ToString() + "%";
		else
		output_string = "";
		leech.text = output_string;
		
		if (ability_info.armor_modifier > 0)
		output_string = "Armor: +" + ability_info.armor_modifier.ToString() + " for " + ability_info.armor_mod_duration.ToString() + "sec";
		else if (ability_info.armor_modifier < 0)
		output_string = "Armor: -" + ability_info.armor_modifier.ToString() + " for " + ability_info.armor_mod_duration.ToString() + "sec";
		else
		output_string = "";
		armor.text = output_string;
		
		output_string = "Cooldown: "+ ability_info.cooldown.ToString() + " sec";
		cooldown.text = output_string;
		
		output_string = "Level: "+ability_info.level.ToString();
		level.text = output_string;
		
		output_string = "Select slot to put in";
		message.text = output_string;
	}
}
