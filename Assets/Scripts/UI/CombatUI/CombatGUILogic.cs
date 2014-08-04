/*using UnityEngine;
using System.Collections;

public class CombatGUILogic : MonoBehaviour {
    public GameObject mainCharacter;
    MainChar mainCharacterScript;



    public GameObject slideBar;
    public GameObject slider;

    public GameObject slideBarActivateBox;

    public GameObject leftLimit;
    public GameObject leftThreshHold;

    public GameObject rightLimit;
    public GameObject rightThreshHold;

    public GameObject camGUI;
    Camera camAcc;



    public GameObject endBattleWindow;

    public float slideSensitivity = 15.0f;

    bool dodgeSlideTouched;

    public GameObject targetIndicator;
    TargetingIndicator targetIndicatorScript;

    public GameObject main3DCam;
    Camera main3DCamAcc;

	GameObject previousTarget;


    public void end_battle_win(MapData mapDataStorage)
    {
        endBattleWindow.SetActive(true);
        EndBattleLogic endBattleScript = endBattleWindow.GetComponent<EndBattleLogic>();
        endBattleScript.initializer(mainCharacterScript.get_hero_stats(), mapDataStorage);
    }

    void texture_resize(GameObject targetObject, Rect targetSize)
    {
        SpriteRenderer target = targetObject.GetComponent<SpriteRenderer>();
        Vector3 targetPos = new Vector3(targetSize.center.x, targetSize.center.y, 10.0f);
        target.transform.position = camAcc.ViewportToWorldPoint(targetPos);
        Vector3 xMin = camAcc.WorldToViewportPoint(target.bounds.min);
        Vector3 xMax = camAcc.WorldToViewportPoint(target.bounds.max);
        Vector3 curSize = xMax - xMin;

        float xScale = targetSize.width / curSize.x;
        float yScale = targetSize.height / curSize.y;
        Vector3 scaleFactor = new Vector3(xScale + 0.05f, yScale + 0.05f, 1.0f);
        target.transform.localScale = Vector3.Scale(target.transform.localScale, scaleFactor);
    }

    float calc_slider_offset(float maxSlide, float currentSlide)
    {
        return (currentSlide / maxSlide);
    }

    

	// Use this for initialization
	void Start () {
        slideBar.SetActive(false);
        camAcc = camGUI.GetComponent<Camera>();
        mainCharacterScript = mainCharacter.GetComponent<MainChar>();
        endBattleWindow.SetActive(false);
        main3DCamAcc = main3DCam.GetComponent<Camera>();
        targetIndicatorScript = targetIndicator.GetComponent<TargetingIndicator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (endBattleWindow.activeInHierarchy == true)
        {
            slideBar.SetActive(false);
        }
       
        //Targeting indicator
        float playerTargetAngle = Vector3.Angle(mainCharacterScript.transform.InverseTransformPoint
            (mainCharacterScript.target.transform.position), mainCharacterScript.transform.forward);

        if (playerTargetAngle < 5.0f && mainCharacterScript.curState == "IDLE")
        {
            Vector3 tempPos = main3DCamAcc.WorldToViewportPoint(mainCharacterScript.target.collider.bounds.center);
            tempPos = camAcc.ViewportToWorldPoint(tempPos);
            targetIndicator.transform.position = tempPos;
            if (targetIndicator.activeInHierarchy == false)
            {
                targetIndicator.SetActive(true);
                targetIndicatorScript.initialize_indicator();
            }
        }
        else
        {
            targetIndicator.SetActive(false);
        }
        
    }
}
*/