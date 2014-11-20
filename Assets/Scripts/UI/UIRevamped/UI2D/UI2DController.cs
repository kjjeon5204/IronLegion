using UnityEngine;
using System.Collections;

public class UI2DController : MonoBehaviour {
    public LayerMask layerClicked;
    public Camera clickCam;

    BaseUIButton itemCurTouching;

	// Use this for initialization
	void Start () {
        if (clickCam == null)
        {
            if (Camera.main != null)
                clickCam = Camera.main;
        }
	}

    BaseUIButton get_item_cur_touching(Touch curTouch)
    {
        BaseUIButton myButton = null;
        Ray touchPos = clickCam.ScreenPointToRay(curTouch.position);
        Vector3 checkWithinCamBoundary = clickCam.ScreenToViewportPoint(curTouch.position);
        if (checkWithinCamBoundary.x >= 0.0f && checkWithinCamBoundary.x <= 1.0f &&
            checkWithinCamBoundary.y >= 0.0f && checkWithinCamBoundary.y <= 1.0f)
        {
            RaycastHit2D myRayCast;
            if (layerClicked == LayerMask.NameToLayer("Nothing") ||
                layerClicked == LayerMask.NameToLayer("Everything"))
            {
                myRayCast = Physics2D.GetRayIntersection(touchPos, 50.0f);
            }
            else
            {
                myRayCast = Physics2D.GetRayIntersection(touchPos, 50.0f, layerClicked);
            }
            if (myRayCast.collider != null)
            {
                myButton = myRayCast.collider.gameObject.GetComponent<BaseUIButton>();
            }
        }
        return myButton;
    }

    void continuously_press_button(Touch curTouch)
    {
        BaseUIButton curButton = get_item_cur_touching(curTouch);
        if (itemCurTouching != null && curButton == itemCurTouching)
        {
            itemCurTouching.button_held_action(curTouch);
        }
    }

    void start_button_touch(Touch curTouch)
    {
        BaseUIButton curButton = get_item_cur_touching(curTouch);
        if (curButton != null)
        {
            itemCurTouching = curButton;
            itemCurTouching.button_pressed();
        }
    }

    void touch_release(Touch curTouch)
    {
        BaseUIButton latestHoverButton = get_item_cur_touching(curTouch);
        if (latestHoverButton != null) {
            if (latestHoverButton == itemCurTouching) {
                itemCurTouching.button_released_action();
            }
            itemCurTouching.button_released();
            itemCurTouching = null;
        }
    }

	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0)
        {
            Touch curTouch = Input.GetTouch(0);
            if (curTouch.phase == TouchPhase.Began)
            {
                start_button_touch(curTouch);
            }
            else if (curTouch.phase == TouchPhase.Moved ||
                curTouch.phase == TouchPhase.Stationary)
            {
                continuously_press_button(curTouch);
            }
            else if (curTouch.phase == TouchPhase.Ended ||
                curTouch.phase == TouchPhase.Canceled)
            {
                touch_release(curTouch);
            }
        }
	}
}
