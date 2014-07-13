using UnityEngine;
using System.Collections;

public class SetCamera : MonoBehaviour {

	GameObject topBound;
	GameObject botBound;
	GameObject leftBound;
	GameObject rightBound;
	Camera cam;
	Camera mainCam;
	
	private GameObject camTop;
	private GameObject camBot;
	private GameObject camLeft;
	private GameObject camRight;
	
	private BoxCollider2D colliderTop;
	private BoxCollider2D colliderBot;
	private BoxCollider2D colliderLeft;
	private BoxCollider2D colliderRight;
	
	private Vector3 leftPos;
	private Vector3 rightPos;
	private Vector3 topPos;
	private Vector3 botPos;
	private float zValue;
	
	private float edgeWidth;
	private float edgeHeight;

	// Use this for initialization
	void Start () {
		topBound = GameObject.Find("CheckTop");
		botBound = GameObject.Find("CheckBot");
		leftBound = GameObject.Find("CheckLeft");
		rightBound = GameObject.Find("CheckRight");
		cam = GameObject.Find("Camera").GetComponent<Camera>();
		mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
		
		Vector3 top = cam.WorldToViewportPoint(topBound.transform.position);
		Vector3 bot = cam.WorldToViewportPoint(botBound.transform.position);
		Vector3 left = cam.WorldToViewportPoint(leftBound.transform.position);
		Vector3 right = cam.WorldToViewportPoint(rightBound.transform.position);
		
		GameObject.Find("UI").GetComponent<MapControls>().SetViewportBoundary(left.x,bot.y,right.x,top.y);
		
		mainCam.rect = new Rect(left.x,bot.y,right.x-left.x,top.y-bot.y);
		
		camTop = GameObject.Find("CamTop");
		camBot = GameObject.Find("CamBot");
		camLeft = GameObject.Find("CamLeft");
		camRight = GameObject.Find("CamRight");
		
		colliderTop = camTop.GetComponent<BoxCollider2D>();
		colliderBot = camBot.GetComponent<BoxCollider2D>();
		colliderLeft = camLeft.GetComponent<BoxCollider2D>();
		colliderRight = camRight.GetComponent<BoxCollider2D>();
		
		zValue = 0f;
		topPos = new Vector3(0.5f,1f,zValue);
		botPos = new Vector3(0.5f,0f,zValue);
		leftPos = new Vector3(0f,0.5f,zValue);
		rightPos = new Vector3(1f,0.5f,zValue);
	}
	
	void Update() {
		camTop.transform.position = mainCam.ViewportToWorldPoint(topPos);//-mainCam.transform.position;
		camBot.transform.position = mainCam.ViewportToWorldPoint(botPos);//-mainCam.transform.position;
		camLeft.transform.position = mainCam.ViewportToWorldPoint(leftPos);//-mainCam.transform.position;
		camRight.transform.position = mainCam.ViewportToWorldPoint(rightPos);//-mainCam.transform.position;
		
		edgeHeight = camTop.transform.position.y-camBot.transform.position.y;
		edgeWidth = camRight.transform.position.x-camLeft.transform.position.x;
		
		colliderTop.size = new Vector2(edgeWidth,0.1f);
		colliderBot.size = new Vector2(edgeWidth,0.1f);
		colliderLeft.size = new Vector2(0.1f,edgeHeight);
		colliderRight.size = new Vector2(0.1f,edgeHeight);
	}
}
