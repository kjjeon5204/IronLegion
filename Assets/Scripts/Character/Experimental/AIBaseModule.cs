using UnityEngine;
using System.Collections;

[System.Serializable]
public struct MultiSequenceAttack {
	//Animation overrides time Duration
	public AnimationClip animationClip;
	public int timeDuration;
	public bool attackState;
	public int[] muzzlesIdsToFire;
}

/*Description 
Used by AIBaseBehavior to modularly construxt AI 
Use this base class in to create different behavior for 
AI*/

public class AIBaseModule : MonoBehaviour {
    public string moduleID;
    protected Character character;


    public virtual void Start()
    {
        character = GetComponent<Character>();
    }

	//intialize all variable to start of the state
    public virtual void initialize_module()
    {
    }

    //return true if behavior is running, false if completed
    public virtual bool run_module()
    {
        return false;
    }
}
