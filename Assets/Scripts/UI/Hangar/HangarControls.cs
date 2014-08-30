using UnityEngine;
using System.Collections;

public class HangarControls : MonoBehaviour {

	private ClickSpriteCONFIRM.Clicked clicked_object;
	private bool clicking;
	private bool scrolling;
	private ClickSpriteCONFIRM click;
	
	public GameObject frameOnScreen;
	public GameObject waiting;
	public GameObject onScreen;
	
	public ScrollControls inventory_scrolling;
	
	void Start () {
		click = GetComponent<ClickSpriteCONFIRM>();
	}


    void PC_Update()
    {
        
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                clicked_object = click.Click(mousePos);
                if (clicked_object.isClicked && clicked_object.clicked_object.name == "Scrollbar")
                {
                    inventory_scrolling.Input(mousePos);
                    scrolling = true;
                }
                else if (clicked_object.isClicked)
                {
                    clicked_object.clicked_object.SendMessage("BeginClick", SendMessageOptions.DontRequireReceiver);
                    clicking = true;
                }
                else
                {
                    clicking = false;
                }

            }
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            Vector2 mousePos = Input.mousePosition;
            if (scrolling && clicked_object.clicked_object.name == "Scrollbar")
            {
                clicked_object.clicked_object.SendMessage("EndClick", SendMessageOptions.DontRequireReceiver);
                scrolling = false;
            }
            else if (clicking && clicked_object.clicked_object == click.Click(mousePos).clicked_object)
            {
                clicked_object.clicked_object.SendMessage("EndClick", SendMessageOptions.DontRequireReceiver);
                clicking = false;
            }
            else if (!clicking)
            {
                scrolling = false;
            }
            else
            {
                scrolling = false;
                clicking = false;
            }
        }
    }





	// Update is called once per frame
	void Update () {
        //Comment out for build!
        PC_Update();
		Touch[] touches = Input.touches;
		if (touches.Length == 1)
		{
			switch (touches[0].phase)
			{
			case TouchPhase.Began:
				clicked_object = click.Click(touches[0].position);
				if (clicked_object.isClicked && clicked_object.clicked_object.name == "Scrollbar")
				{
					inventory_scrolling.Input(touches[0].position);
					scrolling = true;
				}
				else if (clicked_object.isClicked)
				{
					clicked_object.clicked_object.SendMessage("BeginClick",SendMessageOptions.DontRequireReceiver);
					clicking = true;
				}
				else
				{
					clicking = false;
				}
				break;
			case TouchPhase.Moved:
			case TouchPhase.Stationary:
				if (scrolling && click.Click(touches[0].position).clicked_object.name == "Scrollbar")
				{
					inventory_scrolling.Input(touches[0].position);
				}
				else if (clicked_object.clicked_object != null && !scrolling && clicked_object.clicked_object.name == "Scrollbar")
				{
					clicked_object.clicked_object.SendMessage("CanceledClick",SendMessageOptions.DontRequireReceiver);
					scrolling = false;
				}
				else if (clicking && clicked_object.clicked_object.name != click.Click(touches[0].position).clicked_object.name)
				{
					clicked_object.clicked_object.SendMessage("CanceledClick",SendMessageOptions.DontRequireReceiver);
					clicking = false;
				}
				else if (!clicking)
				{
					scrolling = false;
				}
				break;
			case TouchPhase.Ended:
				if (scrolling  && clicked_object.clicked_object.name == "Scrollbar")
				{
					clicked_object.clicked_object.SendMessage("EndClick",SendMessageOptions.DontRequireReceiver);
					scrolling = false;
				}
				else if (clicking && clicked_object.clicked_object == click.Click(touches[0].position).clicked_object)
				{
					clicked_object.clicked_object.SendMessage("EndClick",SendMessageOptions.DontRequireReceiver);
					clicking = false;
				}
				else if (!clicking) 
				{
					scrolling = false;
				}
				else
				{
					scrolling = false;
					clicking = false;
				}
				break;
			default:
				break;
			}
		}
	}
	
	public void SetObjectOnScreen(GameObject obj) {
		frameOnScreen = obj;
		obj.transform.position = onScreen.transform.position;
        frameOnScreen.SetActive(true);
		Debug.Log(frameOnScreen);
	}
	
	public void MoveOffScreen() {
		frameOnScreen.transform.position = waiting.transform.position;
        frameOnScreen.SetActive(false);
	}
}
	
	
