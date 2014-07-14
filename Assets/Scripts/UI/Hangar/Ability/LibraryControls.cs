using UnityEngine;
using System.Collections;

public class LibraryControls : MonoBehaviour {
	private AbilityData abilities;
	private HeroLevelData hero;
	private int hero_level;
	public GameObject library_frame;
	private AbilityControls controls;
	private Vector3 offset;
	public int num_of_available_skills;
	private GameObject end_library;
	private GameObject separator;
	

	void Awake () {
		abilities = new AbilityData();
		abilities.load_data();
		
		hero = GetComponent<HeroLevelData>();
		hero.load_file();
		hero_level = hero.get_player_level();
		
		controls = GameObject.Find("AbilityFrame").GetComponent<AbilityControls>();
		num_of_available_skills = 0;
		offset = new Vector3(1.25f,0,0);
		
		end_library = GameObject.Find("End Library");
		
		separator = GameObject.Find("Separator");
	}
	
	void LoadLibrary() {
		GameObject cur_library_frame;
		LibraryAbility cur_library_ability;
		for (int i = 0; i < abilities.num_of_close; i++)
		{
			if (abilities.GetAbilityInfo(i).level <= hero_level)
			{
				cur_library_frame = (GameObject)Instantiate(library_frame,gameObject.transform.position+(offset*num_of_available_skills),Quaternion.identity);
				cur_library_ability = cur_library_frame.GetComponent<LibraryAbility>();
				cur_library_ability.id = i;
				cur_library_ability.icon_sprite = controls.GetSprite(i);
				cur_library_frame.SendMessage("SetSprite",SendMessageOptions.DontRequireReceiver);
				num_of_available_skills++;
			}
		}
		separator.transform.position = gameObject.transform.position+(offset*(num_of_available_skills-0.5f));
		//Enter divider
		for (int i = 0; i < abilities.num_of_far; i++)
		{
			if (abilities.GetAbilityInfo(i+100).level <= hero_level)
			{
				cur_library_frame = (GameObject)Instantiate(library_frame,gameObject.transform.position+(offset*num_of_available_skills),Quaternion.identity);
				cur_library_ability = cur_library_frame.GetComponent<LibraryAbility>();
				cur_library_ability.id = i+100;
				cur_library_ability.icon_sprite = controls.GetSprite(i+100);
				cur_library_frame.SendMessage("SetSprite",SendMessageOptions.DontRequireReceiver);
				num_of_available_skills++;
				
				end_library.transform.position = cur_library_frame.transform.position;
			}
		}
		gameObject.transform.localScale = new Vector3(1.7f,1.7f,0);
	}
}
