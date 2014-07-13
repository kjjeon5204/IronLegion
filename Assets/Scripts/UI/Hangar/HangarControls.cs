using UnityEngine;
using System.Collections;

public class HangarControls : MonoBehaviour {

	private ClickSpriteCONFIRM.Clicked clicked_object;
	private bool clicking;
	private ClickSpriteCONFIRM click;
	
	private GameObject frameOnScreen;
	private GameObject waiting;
	private Vector3 onScreen;
	
	void Start () {
		waiting = GameObject.Find("WaitingArea");
		onScreen = GameObject.Find("OnScreen").transform.position;
		click = GetComponent<ClickSpriteCONFIRM>();
		frameOnScreen = GameObject.Find("Blank");
	}
	
	// Update is called once per frame
	void Update () {
		Touch[] touches = Input.touches;
		if (touches.Length == 1)
		{
			switch (touches[0].phase)
			{
			case TouchPhase.Began:
				clicked_object = click.Click(touches[0].position);
				if (clicked_object.isClicked)
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
				if (clicking && clicked_object.clicked_object.name != click.Click(touches[0].position).clicked_object.name)
				{
					clicked_object.clicked_object.SendMessage("CanceledClick",SendMessageOptions.DontRequireReceiver);
					clicking = false;
				}
				else if (!clicking)
				{
				}
				break;
			case TouchPhase.Ended:
				if (clicking && clicked_object.clicked_object == click.Click(touches[0].position).clicked_object)
				{
					clicked_object.clicked_object.SendMessage("EndClick",SendMessageOptions.DontRequireReceiver);
					clicking = false;
				}
				else if (!clicking) 
				{
				}
				else
					clicking = false;
				break;
			default:
				break;
			}
		}
	}
	
	public void SetObjectOnScreen(GameObject obj) {
		frameOnScreen = obj;
		obj.transform.position = onScreen;
		Debug.Log(frameOnScreen);
	}
	
	public void MoveOffScreen() {
		frameOnScreen.transform.position = waiting.transform.position;
	}
}
	
	
