using UnityEngine;
using System.Collections;

public class TileActive : MonoBehaviour {
	
	public bool isBoss;
	public int level;
	public bool isUsable;
	public bool second_tile_type;

	private MapData mapdata;
	private Animator anim;

	private DialogueControls dialogue;
	private DialogueData attached_dialogue;
	
    public string mapID;
    public int[] unlockedLevels;
	
	int isBossHASH = Animator.StringToHash("isBoss");
	int isUsableHASH = Animator.StringToHash("isUsable");
	int tile_typeHASH = Animator.StringToHash("secondTileType");
	
	void Awake()
	{
		isUsable = false;
		dialogue = GameObject.Find("Dialogue").GetComponent<DialogueControls>();
		attached_dialogue = gameObject.GetComponent<OverworldDialogue>().dialogue;
	}
	
	// Use this for initialization
	void Start() 
	{
		anim = gameObject.GetComponent<Animator>();
		anim.SetBool(tile_typeHASH,second_tile_type);
		anim.SetBool(isBossHASH,isBoss);
	}
	void Clicked()
	{
		if (isUsable)
		{
			ActivateConfirmation confirm = GameObject.Find("Confirm Level").GetComponent<ActivateConfirmation>();
			confirm.Confirm( mapID, level,"LEVEL");
		}
		dialogue.SetDialogue(attached_dialogue);
	}

	public void TileOn()
	{
		anim = gameObject.GetComponent<Animator>();
		isUsable = true;
		anim.SetBool(isUsableHASH,isUsable);
		
		gameObject.GetComponent<PolygonCollider2D>().enabled = true;
	}
}
