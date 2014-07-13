using UnityEngine;
using System.Collections;

public class MapControls : MonoBehaviour {

	private ClickSpriteCONFIRM click;
	public bool confirmingLevel;
	
	public float maxZoom = 4.4f;
	public float minZoom = 2.75f;
	private float zoomSpeed = 0.05f;
	
	public bool topBound;
	public bool botBound;
	public bool leftBound;
	public bool rightBound;
	public float moveCamSpeed = 0.2f;
	
	private CheckBoundary camLeft;
	private CheckBoundary camRight;
	private CheckBoundary camTop;
	private CheckBoundary camBot;
	
	public bool clicking;
	private ClickSpriteCONFIRM.Clicked clicked_object;
	
	private Camera cam;
	private Vector4 viewportBoundary;
	// Use this for initialization
	
	void Awake () {
		viewportBoundary = new Vector4(0,0,0,0);
		cam = GameObject.Find("Camera").GetComponent<Camera>();
	}
	
	void Start () {
		click = GetComponent<ClickSpriteCONFIRM>();
		confirmingLevel = false;
		
		topBound = false;
		botBound = false;
		leftBound = false;
		rightBound = false;
		
		camLeft = GameObject.Find("CamLeft").GetComponent<CheckBoundary>();
		camRight = GameObject.Find("CamRight").GetComponent<CheckBoundary>();
		camTop = GameObject.Find("CamTop").GetComponent<CheckBoundary>();
		camBot = GameObject.Find("CamBot").GetComponent<CheckBoundary>();
		
		clicking = false;
	}
	
	// Update is called once per frame
	void Update () {
		CheckView();
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
					else //move map
					{
						if (CheckMovePosition(touches[0].position))
						ChangeCameraPosition(touches[0].deltaPosition);
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
					else if (!clicking) //move map
					{
						if (CheckMovePosition(touches[0].position))
						ChangeCameraPosition(touches[0].deltaPosition);
					}
					break;
				case TouchPhase.Ended:
					if (clicking && clicked_object.clicked_object == click.Click(touches[0].position).clicked_object)
					{
						clicked_object.clicked_object.SendMessage("EndClick",SendMessageOptions.DontRequireReceiver);
						clicking = false;
					}
					else if (!clicking) //move map
					{
						if (CheckMovePosition(touches[0].position))
						ChangeCameraPosition(touches[0].deltaPosition);
					}
					else
					clicking = false;
					break;
				default:
					break;
			}
		}
		else if (touches.Length == 2)
		{
			Zoom(touches[0], touches[1]);
		}
	}
	
	public void ChangeCameraPosition(Vector2 change) {
		Vector3 moveX = Camera.main.ScreenToWorldPoint(new Vector3(change.x*-2f,0,0))-Camera.main.ScreenToWorldPoint(new Vector3(0,0,0));
		Vector3 moveY = Camera.main.ScreenToWorldPoint(new Vector3(0,change.y*-2f,0))-Camera.main.ScreenToWorldPoint(new Vector3(0,0,0));
		
		if ( ((change.x < 0f && !rightBound) || (change.x > 0f && !leftBound)) && !WillGoOutOfMap(moveX,"X"))
		Camera.main.transform.position = Camera.main.transform.position+moveX;
		if ( ((change.y < 0f && !topBound) || (change.y > 0f && !botBound)) && !WillGoOutOfMap(moveY,"Y"))
		Camera.main.transform.position = Camera.main.transform.position+moveY;
	}
	
	void Zoom(Touch t1, Touch t2) {
		
		Vector2 t1_prev = t1.position-t1.deltaPosition;
		Vector2 t2_prev = t2.position-t2.deltaPosition;
		
		float prevMag = (t1_prev-t2_prev).magnitude;
		float newMag = (t1.position-t2.position).magnitude;
		
		float difference_in_mag = prevMag-newMag;
		
		if (Camera.main.orthographicSize != minZoom || Camera.main.orthographicSize != maxZoom)
		{
			Camera.main.orthographicSize += difference_in_mag*zoomSpeed;
		}
		if (Camera.main.orthographicSize > maxZoom)
		{
			Camera.main.orthographicSize = maxZoom;
		}
		if (Camera.main.orthographicSize < minZoom)
		{
			Camera.main.orthographicSize = minZoom;
		}
	}
	
	void CheckView() {
		if (topBound)
		{
			Camera.main.transform.position += camTop.returnDistance();
		}
		if (botBound)
		{
			Camera.main.transform.position += camBot.returnDistance();
		}
		if (leftBound)
		{
			Camera.main.transform.position += camLeft.returnDistance();
		}
		if (rightBound)
		{
			Camera.main.transform.position += camRight.returnDistance();
		}
		if (Camera.main.orthographicSize > maxZoom)
		{
			Camera.main.orthographicSize = maxZoom;
		}
		if (Camera.main.orthographicSize < minZoom)
		{
			Camera.main.orthographicSize = minZoom;
		}
	}
	
	public void SetViewportBoundary(float x, float y, float z, float w) {
		viewportBoundary = new Vector4(x,y,z,w);
	}
	
	bool CheckMovePosition(Vector2 pos) {
		Vector3 viewPos = cam.ScreenToViewportPoint(new Vector3(pos.x,pos.y,0));
		if (viewPos.x > this.viewportBoundary.x && viewPos.x < this.viewportBoundary.z)
		{
			if (viewPos.y > this.viewportBoundary.y && viewPos.y < this.viewportBoundary.w)
			{
				return true;
			}
		}
		return false;
	}
	
	bool WillGoOutOfMap(Vector3 movement,string direction) {
		if (direction == "X")
		{
			if (!camLeft.CheckOutOfBounds(movement))
			return false;
			else
			Camera.main.transform.position += camLeft.returnDistance();
			
			if (!camRight.CheckOutOfBounds(movement))
			return false;
			else
			Camera.main.transform.position += camRight.returnDistance();
		}
		else if (direction == "Y")
		{
			if (!camBot.CheckOutOfBounds(movement))
			return false;
			else
			Camera.main.transform.position += camBot.returnDistance();
			
			if (!camTop.CheckOutOfBounds(movement))
			return false;
			else
			Camera.main.transform.position += camTop.returnDistance();
		}
		return true;
	}
}
