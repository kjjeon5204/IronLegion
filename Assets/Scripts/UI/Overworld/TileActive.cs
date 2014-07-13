using UnityEngine;
using System.Collections;

public class TileActive : MonoBehaviour {
	
	public bool isBoss;
	public int level;
	public bool isUsable;

	private MapData mapdata;
	private Animator anim;

    public string mapID;
    public int[] unlockedLevels;
	
	int isBossHASH = Animator.StringToHash("isBoss");
	int isUsableHASH = Animator.StringToHash("isUsable");
	
	void Awake()
	{
		isUsable = false;
	}
	
	// Use this for initialization
	void Start() 
	{
		anim = gameObject.GetComponent<Animator>();
		
		anim.SetBool(isBossHASH,isBoss);
	}
	void Clicked()
	{
		if (isUsable)
		{
			ActivateConfirmation confirm = GameObject.Find("Confirm Level").GetComponent<ActivateConfirmation>();
			confirm.Confirm( mapID, level,"LEVEL");
		}
	}

	public void TileOn()
	{
		anim = gameObject.GetComponent<Animator>();
		isUsable = true;
		anim.SetBool(isUsableHASH,isUsable);
		
		gameObject.GetComponent<PolygonCollider2D>().enabled = true;
	}
}
