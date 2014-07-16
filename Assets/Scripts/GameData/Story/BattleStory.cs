using UnityEngine;
using System.Collections;


/*base class for all story*/
public class BattleStory : MonoBehaviour {
    public float cutSceneDuration;
    protected float cutSceneEndTime;

    protected GameObject uiCam;
    protected CombatScript combatScript;

    public void set_ui_cam(GameObject myCam)
    {
        uiCam = myCam;
        combatScript = uiCam.GetComponent<CombatScript>();
    }

	// Use this for initialization
	public virtual void manual_start () {
        uiCam = GameObject.Find("Camera");
        cutSceneEndTime = Time.time + cutSceneDuration;
	}
	
	// Update is called once per frame
	public virtual bool manual_update () {
        return true;
	}
}
