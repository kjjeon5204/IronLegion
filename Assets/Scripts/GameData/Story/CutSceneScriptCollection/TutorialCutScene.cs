using UnityEngine;
using System.Collections;

public class TutorialCutScene : BattleStory {
    public GameObject abilityTutorialIcon;
    public GameObject dodgeTutorialIcon;
    public GameObject changeStateTutorialIcon;
    public GameObject regularAttackButton;


    public MainChar heroMech;
    public Camera tutorialUICam;
   

    int tutorialProgress = 0;
    //0 = regular attack
    //1 = Dodge
    //2 = change state
    bool regularAttackTutorial = false;
    bool abilityAttackTutorial = false;
    bool dodgeTutorial = false;
    

    bool dialoguePhase = true;
    bool tutorialPhaseComplete;

    

	// Use this for initialization
    public override void manual_start()
    {
        //heroMech.manual_start();
    }

   

    // Update is called once per frame
    public override bool manual_update()
    {
       // heroMech.manual_update();
        if (tutorialProgress == 0)
        {
            //regular attack
        }
        else if (tutorialProgress == 1)
        {
        }
        return false;
    }
}
