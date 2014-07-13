using UnityEngine;
using System.Collections;


/*base class for all story*/
public class BattleStory : MonoBehaviour {
    public float cutSceneDuration;
    protected float cutSceneEndTime;

	// Use this for initialization
	public virtual void manual_start () {
        cutSceneEndTime = Time.time + cutSceneDuration;
	}
	
	// Update is called once per frame
	public virtual bool manual_update () {
        return true;
	}
}
