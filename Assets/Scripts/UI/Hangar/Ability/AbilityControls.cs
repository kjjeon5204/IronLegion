using UnityEngine;
using System.Collections;

public class AbilityControls : MonoBehaviour {
	private HeroData hero;
	private AbilityData abilities;
	private Slot[] ability_slots;
	
	public Sprite ability000;
	public Sprite ability001;
	public Sprite ability002;
	public Sprite ability003;
	public Sprite ability100;
	public Sprite ability101;
	public Sprite ability102;
	public Sprite ability103;
	
	private Sprite[] close_range_sprites;
	private Sprite[] long_range_sprites;
	
	private int saved_id;
	private DescriptionControl description;
	private GameObject library;
	
	// Use this for initialization
	void Start () {
		abilities = new AbilityData();
		abilities.load_data();
		close_range_sprites = new Sprite[abilities.num_of_close];
		long_range_sprites = new Sprite[abilities.num_of_far];
		
		close_range_sprites[0] = ability000;
		close_range_sprites[1] = ability001;
		close_range_sprites[2] = ability002;
		close_range_sprites[3] = ability003;
		long_range_sprites[0] = ability100;
		long_range_sprites[1] = ability101;
		long_range_sprites[2] = ability102;
		long_range_sprites[3] = ability103;
	
		hero = new HeroData();
		hero.load_data();
		ability_slots = new Slot[8];
		for (int i = 0; i < 8; i++)
		{
				ability_slots[i] = transform.Find("Frame"+i.ToString()).GetComponent<Slot>();
		}
		UpdateSlots();
		
		saved_id = -1;
		description = transform.Find("AbilityDescription").GetComponent<DescriptionControl>();
		library = GameObject.Find("LibraryControl");
		library.SendMessage("LoadLibrary",SendMessageOptions.DontRequireReceiver);
	}
	
	void UpdateSlots() {
		for (int i = 0; i < 8; i++)
		{
				ability_slots[i].ability_name = hero.ReturnAbilityName(i);
				ability_slots[i].id_num = hero.ReturnAbilityID(i);
				if (ability_slots[i].id_num < 100)
				ability_slots[i].icon_sprite = close_range_sprites[ability_slots[i].id_num];
				else
				ability_slots[i].icon_sprite = long_range_sprites[ability_slots[i].id_num-100];
		}
	}
	
	public void SetAbilityToSwitch(int id) {
		saved_id = id;
		if (id < 100)
		description.icon_sprite = close_range_sprites[id];
		else
		description.icon_sprite = long_range_sprites[id-100];
		description.ability_info = abilities.GetAbilityInfo(id);
		description.UpdateDescription();
	}
	
	public void SlotClicked(int index) {
		bool switched = hero.SetAbility(index,saved_id);
		if (saved_id != -1 && !switched)
		{
			//Tell person not good slot
		}
		else if (saved_id != -1)
		{
			hero.save_data();
			UpdateSlots();
		}
	}
	
	public Sprite GetSprite(int id) {
		if (id < 100)
		return close_range_sprites[id];
		else
		return long_range_sprites[id-100];
	}
}
