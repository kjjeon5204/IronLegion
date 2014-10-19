using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BasicTextCutScene : BattleStory {
    public GameObject cutSceneCamObject;
    Camera cutSceneCam;


    public GameObject cutSceneDialogueBoxObject;
    public GameObject cutSceneCharacterTextObject;
    TextMesh cutSceneCharacterText;
    public GameObject cutSceneDialogueTextObject;
    TextMesh cutSceneDialogueText;
    public GameObject cutScenePortraitObject;
    SpriteRenderer cutScenePortrait;
    public GameObject cutSceneDialogueObject;
    CombatDialogue cutSceneDialogue;





    UIStringModifier textModifier;

    int dialogueCtr;


    void texture_resize(GameObject target, Rect targetSize)
    {
        SpriteRenderer targetSprite = target.GetComponent<SpriteRenderer>();
        Vector3 targetPos = new Vector3(targetSize.center.x, 1.0f - targetSize.center.y, 10.0f);
        target.transform.position = cutSceneCam.ViewportToWorldPoint(targetPos);
        Vector3 xMin = cutSceneCam.WorldToViewportPoint(targetSprite.bounds.min);
        Vector3 xMax = cutSceneCam.WorldToViewportPoint(targetSprite.bounds.max);
        Vector3 curSize = xMax - xMin;

        float xScale = targetSize.width / curSize.x;
        float yScale = targetSize.height / curSize.y;
        Vector3 scaleFactor = new Vector3(xScale, yScale, 1.0f);
        target.transform.localScale = Vector3.Scale(target.transform.localScale, scaleFactor);
        //Debug.Log(scaleFactor);
    }



	// Use this for initialization
	public override void manual_start () {
        base.manual_start();
		cutSceneCam = cutSceneCamObject.GetComponent<Camera>();
		Rect dialogueBoxSize = new Rect(0.1f, 0.6f, 0.8f, 0.35f);
        texture_resize(cutSceneDialogueBoxObject, dialogueBoxSize);

        

        //get dialogue text mesh
        cutSceneDialogueText = cutSceneDialogueTextObject.GetComponent<TextMesh>();

        //get character text mesh
        cutSceneCharacterText = cutSceneCharacterTextObject.GetComponent<TextMesh>();

        //Get portrait object
        cutScenePortrait = cutScenePortraitObject.GetComponent<SpriteRenderer>();

        //Get dialogue data
        cutSceneDialogue = cutSceneDialogueObject.GetComponent<CombatDialogue>();
        dialogueCtr = 0;
        get_next_text();




	}

    
    void disable_dialogue()
    {
        cutSceneDialogueBoxObject.SetActive(false);
    }

    void enable_dialogu()
    {
        cutSceneDialogueBoxObject.SetActive(true);
    }

    //true successful
    //false no dialogue remaining
    bool get_next_text()
    {
        if (dialogueCtr == cutSceneDialogue.dialogues.Length)
        {
            return false;
        }
        else
        {
            cutScenePortrait.sprite = cutSceneDialogue.dialogues[dialogueCtr].characterSprite;
            cutSceneDialogueText.text = cutSceneDialogue.dialogues[dialogueCtr].text;
            cutSceneCharacterText.text = cutSceneDialogue.dialogues[dialogueCtr].characterName;
            dialogueCtr++;
            return true;
        }

    }


    // Update is called once per frame
    public override bool manual_update()
    {
        //PC
        if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 &&
            Input.GetTouch(0).phase == TouchPhase.Began))
        {
            if (!get_next_text())
            {
                if (customCutsceneAudio != null)
                    customCutsceneAudio.Stop();
                return true;
            }
        }

        return false;
	}
}
