/*
using UnityEngine;
using System.Collections;

public class DummyBattleGUI : MonoBehaviour {
    public GameObject[] skillButtons;
    // 0 - 3 is melee, 4 - 7 is ranged
    UIObjectTransform[] skillButtonsTransform;
    SKillButtonsData[] skillButtonsData;
    Camera GUICam;
    public GameObject mainCam;
    GameObject world;
    EventControls eventControlsScript;
    public float switchTimer;

    //Helper Functions
    GameObject make_gui_button(GameObject button, Rect rect)
    {
        GameObject returnObject = (GameObject)Instantiate(button, Vector3.zero, Quaternion.identity);
        texture_resize(returnObject, rect);
        return returnObject;
    }
    void texture_resize(GameObject target, Rect targetSize)
    {
        SpriteRenderer targetSprite = target.GetComponent<SpriteRenderer>();
        Vector3 targetPos = new Vector3(targetSize.center.x, 1.0f - targetSize.center.y, 10.0f);
        target.transform.position = GUICam.ViewportToWorldPoint(targetPos);
        Vector3 xMin = GUICam.WorldToViewportPoint(targetSprite.bounds.min);
        Vector3 xMax = GUICam.WorldToViewportPoint(targetSprite.bounds.max);
        Vector3 curSize = xMax - xMin;

        float xScale = targetSize.width / curSize.x;
        float yScale = targetSize.height / curSize.y;
        Vector3 scaleFactor = new Vector3(xScale, yScale, 1.0f);
        target.transform.localScale = Vector3.Scale(target.transform.localScale, scaleFactor);
        //Debug.Log(scaleFactor);
    }


    //Game Initializer
    void setup_GUI()
    {
        Rect rect;
        //Button Layout
        for (int ctr = 0; ctr < skillButtons.Length; ctr++)
        {
            if (ctr < 4)
            {
                rect = new Rect(0.8f, 0.2f + 0.15f * ctr, 0.14f, 0.14f);
                skillButtons[ctr] = make_gui_button(skillButtons[ctr], rect);
                skillButtons[ctr].transform.parent = this.gameObject.transform.parent;
            }
            else
            {
                rect = new Rect(1.2f, 0.2f + 0.15f * (ctr-4), 0.14f, 0.14f);
                skillButtons[ctr] = make_gui_button(skillButtons[ctr], rect);
                skillButtons[ctr].transform.parent = this.gameObject.transform.parent;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        world = (GameObject)GameObject.Find("World");
        eventControlsScript = world.GetComponent<EventControls>();
        GUICam = this.GetComponent<Camera>();
        GUICam.gameObject.transform.position = mainCam.transform.position;
        setup_GUI();
        skillButtonsData = new SKillButtonsData[skillButtons.Length];
        skillButtonsTransform = new UIObjectTransform[skillButtons.Length];
        float dist = (GUICam.ViewportToWorldPoint(new Vector3(1.2f, 0.0f, 0.0f)) - GUICam.ViewportToWorldPoint(new Vector3(0.8f, 0.0f, 0.0f))).magnitude;
        for (int ctr = 0; ctr < skillButtons.Length; ctr++)
        {
            if (ctr < 4)
            {
                skillButtonsData[ctr] = skillButtons[ctr].GetComponent<SKillButtonsData>();
                skillButtonsTransform[ctr] = skillButtons[ctr].GetComponent<UIObjectTransform>();
                skillButtonsData[ctr].buttonID = "button" + (ctr + 1).ToString();
                skillButtonsTransform[ctr].set_inital_condition(dist, true);
            }
            else {
                skillButtonsData[ctr] = skillButtons[ctr].GetComponent<SKillButtonsData>();
                skillButtonsTransform[ctr] = skillButtons[ctr].GetComponent<UIObjectTransform>();
                skillButtonsData[ctr].buttonID = "button" + (ctr + 1).ToString();
                skillButtonsTransform[ctr].set_inital_condition(dist, false);
            }
        }

        switchTimer = 0.0f;
	}



    void check_input(Touch acc)
    {
        int layerMask = 1 << 9;
        RaycastHit touchGUIInteraction;
        Ray touchRay = GUICam.ScreenPointToRay(acc.position);
        if (Physics.Raycast(touchRay, out touchGUIInteraction, 100.0f, layerMask))
        {
            SKillButtonsData buttonPressed = touchGUIInteraction.collider.gameObject.GetComponent<SKillButtonsData>();
            Debug.Log("Button " + gameObject.name + " pressed!");
            if (buttonPressed.is_active() && buttonPressed.is_on_CD() == false)
            {
                eventControlsScript.send_command(buttonPressed.button_pressed());
            }
        }
        else
        {
            if (switchTimer < Time.time)
            {
                switchTimer = Time.time + 1.5f;
                for (int ctr = 0; ctr < skillButtonsTransform.Length; ctr++)
                {
                    skillButtonsTransform[ctr].switch_position();
                }
            }
            Debug.Log("Nothing is pressed");
        }
    }




    // Update is called once per frame
    void Update()
    {
        //Check Button inputs Only
        for (int ctr = 0; ctr < Input.touchCount; ctr++)
        {
            if (Input.GetTouch(ctr).phase == TouchPhase.Began)
                check_input(Input.GetTouch(ctr));
        }
	}
}
*/