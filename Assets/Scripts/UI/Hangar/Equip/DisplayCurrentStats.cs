using UnityEngine;
using System.Collections;

public class DisplayCurrentStats : MonoBehaviour {
	
	public HeroLevelData hero_info;
	private Stats hero_stats;
	private HeroStats hero;
	private PlayerStat item_stats;
	
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
		hero.load_data();
		item_stats = hero.get_item_stats();
		
		level = hero_stats.level;
		cur_xp = hero_stats.curExp;
		tot_xp = hero_stats.totalExp;
		hp = hero_stats.baseHp+item_stats.item_hp;
		armor = item_stats.item_armor;
		damage = item_stats.item_damage;
		energy = hero_stats.item_energy + item_stats.item_energy;
		penetration = item_stats.item_penetration;
		luck = item_stats.item_luck;
		
		UpdateImage();
	}
	
	void UpdateImage() {
		exp_bar_fill.transform.localScale = new Vector3(cur_xp/tot_xp,1f,1f);
		
		exp_indicator.text = (int)cur_xp + "<b>/</b>" + (int)tot_xp;
		level_indicator.text = "<b><color=lime>Level: </color></b>" + (int)level;
		hp_indicator.text = "<b><color=lime>HP: </color></b>" + (int)hp;
		armor_indicator.text = "<b><color=lime>Armor: </color></b>" + (int)armor;
		damage_indicator.text = "<b><color=lime>Damage: </color></b>" + (int)damage;
		energy_indicator.text = "<b><color=lime>Energy: </color></b>" + (int)energy;
		penetration_indicator.text = "<b><color=lime>Penetration: </color></b>" + (int)penetration;
		luck_indicator.text = "<b><color=lime>Luck: </color></b>" + (int)luck;
	}
}
