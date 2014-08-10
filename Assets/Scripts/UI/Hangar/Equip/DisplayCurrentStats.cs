using UnityEngine;
using System.Collections;

public class DisplayCurrentStats : MonoBehaviour {
	
	public ItemControls stat_controller;
	
	public HeroLevelData hero_info;
	private Stats hero_stats;
	private HeroStats hero;
	private Stats item_stats;
	
	public GameObject[] equipped_slots;
	private Item item_to_add;
	
	private float level;
	private float cur_xp;
	private float tot_xp;
	private float hp;
	private float armor;
	private float damage;
	private float energy;
	private float penetration;
	private float luck;
	
	public GameObject exp_bar_fill;
	public TextMesh exp_indicator;
	public TextMesh level_indicator;
	public TextMesh hp_indicator;
	public TextMesh armor_indicator;
	public TextMesh damage_indicator;
	public TextMesh energy_indicator;
	public TextMesh penetration_indicator;
	public TextMesh luck_indicator;

	// Use this for initialization
	void Start () {
		hero_info.load_file();
		hero = new HeroStats();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateStats();
	}
	
	public void UpdateStats() {
	
		hero_stats = hero_info.get_player_stat_all();
		item_stats = stat_controller.stats;
		
		level = hero_stats.level;
		cur_xp = hero_stats.curExp;
		tot_xp = hero_stats.totalExp;
		/*hp = hero_stats.baseHp+item_stats.item_hp;
		armor = item_stats.item_armor;
		damage = hero_stats.item_damage+item_stats.item_damage;
		energy = hero_stats.item_energy + item_stats.item_energy;
		penetration = item_stats.item_penetration;
		luck = item_stats.item_luck;*/
		hp = hero_stats.baseHp;
		armor = 0;
		damage = hero_stats.item_damage;
		energy = hero_stats.item_energy;
		penetration = 0;
		luck = 0;
		
		for (int i = 0; i < 5; i++)
		{
			item_to_add = equipped_slots[i].GetComponentInChildren<Item>();
			hp += item_to_add.hp;
			armor += item_to_add.armor;
			damage += item_to_add.damage;
			energy += item_to_add.energy;
			penetration += item_to_add.penetration;
			luck += item_to_add.luck;
		}
		
		UpdateImage();
	}
	
	void UpdateImage() {
		exp_bar_fill.transform.localScale = new Vector3(cur_xp/tot_xp,1f,1f);
		
		exp_indicator.text = (int)cur_xp + "<b>/</b>" + (int)tot_xp;
		level_indicator.text = "<b><color=lime>Level: </color></b>" + (int)level;
		hp_indicator.text = "<b><color=lime>HP: </color></b>" + (int)hp;
		if (armor >= 0)
		armor_indicator.text = "<b><color=lime>Armor: </color></b>" + (int)armor;
		else
		armor_indicator.text = "<b><color=lime>Armor: </color></b>0";
		damage_indicator.text = "<b><color=lime>Damage: </color></b>" + (int)damage;
		energy_indicator.text = "<b><color=lime>Energy: </color></b>" + (int)energy;
		penetration_indicator.text = "<b><color=lime>Penetration: </color></b>" + (int)penetration;
		luck_indicator.text = "<b><color=lime>Luck: </color></b>" + (int)luck;
	}
}
