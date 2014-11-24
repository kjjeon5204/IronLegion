using UnityEngine;
using System.Collections;

public class MouseClickData
{
    public Vector3 position;
    public Vector3 deltaPosition;
    public enum MouseState
    {
        PRESSED,
        RELEASED,
        HELD,
        NO_PRESS
    }
    public MouseState phase;

    public void update_mouse(KeyCode check)
    {
        deltaPosition = Input.mousePosition - position;
        position = Input.mousePosition;
        if (Input.GetKeyDown(check))
        {
            phase = MouseState.PRESSED;
        }
        else if (Input.GetKeyUp(check))
        {
            phase = MouseState.RELEASED;
        }
        else if (Input.GetKey(check))
        {
            phase = MouseState.HELD;
        }
        else
        {
            phase = MouseState.NO_PRESS;
        }
    }
}

public class CustomInput
{
    public Vector3 position;
    public Vector3 deltaPosition;
    public enum InputPhase
    {
        PRESSED,
        RELEASED,
        HELD,
        HOVER
    }
    public InputPhase phase;

    public void translate_unity_to_custom(Touch myTouch)
    {
        position = myTouch.position;
        deltaPosition = myTouch.deltaPosition;
        if (myTouch.phase == TouchPhase.Began)
        {
            phase = InputPhase.PRESSED;
        }
        else if (myTouch.phase == TouchPhase.Canceled ||
            myTouch.phase == TouchPhase.Ended)
        {
            phase = InputPhase.RELEASED;
        }
        else if (myTouch.phase == TouchPhase.Moved ||
            myTouch.phase == TouchPhase.Stationary)
        {
            phase = InputPhase.HELD;
        }
    }

    public void translate_unity_to_custom(MouseClickData myMouseData)
    {
        position = myMouseData.position;
        deltaPosition = myMouseData.deltaPosition;
        if (myMouseData.phase == MouseClickData.MouseState.PRESSED)
        {
            phase = InputPhase.PRESSED;
        }
        else if (myMouseData.phase == MouseClickData.MouseState.HELD)
        {
            phase = InputPhase.HELD;
        }
        else if (myMouseData.phase == MouseClickData.MouseState.RELEASED)
        {
            phase = InputPhase.RELEASED;
        }
        else if (myMouseData.phase == MouseClickData.MouseState.NO_PRESS)
        {
            phase = InputPhase.HOVER;
        }
    }
}

public enum TouchControllerType
{
    TOUCH_ACTIVATE,
    DRAG_AND_DROP
}


public class UI2DController : MonoBehaviour {
    public LayerMask layerClicked;
    public Camera clickCam;

    BaseUIButton itemCurTouching;
    MouseClickData curMouseData = new MouseClickData();

    


	// Use this for initialization
	void Start () {
        if (clickCam == null)
        {
            if (Camera.main != null)
                clickCam = Camera.main;
        }
	}

    BaseUIButton get_item_cur_touching(CustomInput curTouch)
    {
        BaseUIButton myButton = null;
        Ray touchPos = clickCam.ScreenPointToRay(curTouch.position);
        Vector3 checkWithinCamBoundary = clickCam.ScreenToViewportPoint(curTouch.position);
        if (checkWithinCamBoundary.x >= 0.0f && checkWithinCamBoundary.x <= 1.0f &&
            checkWithinCamBoundary.y >= 0.0f && checkWithinCamBoundary.y <= 1.0f)
        {
            RaycastHit2D myRayCast;
            if (layerClicked == LayerMask.NameToLayer("Nothing") ||
                layerClicked == LayerMask.NameToLayer("Default"))
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

    void continuously_press_button(CustomInput curTouch)
    {
        itemCurTouching.button_held_action(curTouch);
    }

    void start_button_touch(CustomInput curTouch)
    {
        BaseUIButton curButton = get_item_cur_touching(curTouch);
        if (curButton != null)
        {
            itemCurTouching = curButton;
            CustomInput myInput = new CustomInput();
            itemCurTouching.button_pressed(myInput);
            itemCurTouching.button_pressed_action();
        }
    }

    void touch_release(CustomInput curTouch)
    {
        BaseUIButton latestHoverButton = get_item_cur_touching(curTouch);
        if (latestHoverButton != null) {
            if (myType == TouchControllerType.DRAG_AND_DROP || latestHoverButton == itemCurTouching) {
                itemCurTouching.button_released_action();
            }
            itemCurTouching.button_released();
            itemCurTouching = null;
        }
    }


    public TouchControllerType myType; 

    void touch_and_activate(Touch curTouch)
    {
        if (curTouch.phase == TouchPhase.Began)
        {
            CustomInput translateInput = new CustomInput();
            translateInput.translate_unity_to_custom(curTouch);
            start_button_touch(translateInput);

        }
        else if (curTouch.phase == TouchPhase.Ended ||
            curTouch.phase == TouchPhase.Canceled)
        {
            CustomInput translateInput = new CustomInput();
            translateInput.translate_unity_to_custom(curTouch);
            touch_release(translateInput);
        }
    }

    void touch_and_activate(MouseClickData curTouch)
    {
        CustomInput translateInput = new CustomInput();
        translateInput.translate_unity_to_custom(curTouch);
        if (curTouch.phase == MouseClickData.MouseState.PRESSED)
        {
            start_button_touch(translateInput);

        }
        else if (curTouch.phase == MouseClickData.MouseState.RELEASED)
        {
            touch_release(translateInput);
        }
    }

    void drag_and_drop(Touch curTouch)
    {
        CustomInput translateInput = new CustomInput();
        translateInput.translate_unity_to_custom(curTouch);
        if (curTouch.phase == TouchPhase.Began)
        {
            start_button_touch(translateInput);    
        }
        else if (curTouch.phase == TouchPhase.Moved ||
            curTouch.phase == TouchPhase.Stationary)
        {
            continuously_press_button(translateInput);
        }
        else if (curTouch.phase == TouchPhase.Ended ||
            curTouch.phase == TouchPhase.Canceled)
        {
            touch_release(translateInput);
        }
    }

    void drag_and_drop(MouseClickData curTouch)
    {
        CustomInput translateInput = new CustomInput();
        translateInput.translate_unity_to_custom(curTouch);
        if (translateInput.phase == CustomInput.InputPhase.PRESSED)
        {
            start_button_touch(translateInput);
        }
        else if (translateInput.phase == CustomInput.InputPhase.HELD)
        {
            continuously_press_button(translateInput);
        }
        else if (translateInput.phase == CustomInput.InputPhase.RELEASED)
        {
            touch_release(translateInput);
        }
    }
	
	// Update is called once per frame
	void Update () {
        curMouseData.update_mouse(KeyCode.Mouse0);
        if (myType == TouchControllerType.TOUCH_ACTIVATE)
        {
            if (Input.touchCount > 0)
                touch_and_activate(Input.GetTouch(0));
            else if (Input.GetKey(KeyCode.Mouse0))
                touch_and_activate(curMouseData);
        }
        else if (myType == TouchControllerType.DRAG_AND_DROP)
        {
            if (Input.touchCount > 0)
                drag_and_drop(Input.GetTouch(0));
            else if (Input.GetKey(KeyCode.Mouse0))
                drag_and_drop(curMouseData);
        }
	}
}
