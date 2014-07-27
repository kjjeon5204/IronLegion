using UnityEngine;
using System.Collections;

public class CheckBoundary : MonoBehaviour {

	public string side;
	
	public GameObject boundary;
	
	private MapControls map;
	// Use this for initialization
	void Start () {
		map = GameObject.Find("UI").GetComponent<MapControls>();
	}
	
	void Update () {
		switch (side)
		{
			case "LEFT":
				if (transform.position.x < boundary.transform.position.x)
					map.leftBound = true;
				else
					map.leftBound = false;
				break;
			case "RIGHT":
				if (transform.position.x > boundary.transform.position.x)
					map.rightBound = true;
				else
					map.rightBound = false;
				break;
			case "TOP":
				if (transform.position.y > boundary.transform.position.y)
					map.topBound = true;
				else
					map.topBound = false;
				break;
			case "BOT":
				if (transform.position.y < boundary.transform.position.y)
					map.botBound = true;
				else
					map.botBound = false;
				break;
			default:
				break;
		}
	}
	
	public bool CheckOutOfBounds(Vector3 change) {
		switch (this.side)
		{
			case "LEFT":
				if ((this.transform.position+change).x < boundary.transform.position.x)
					return true;
				else
					return false;
			case "RIGHT":
				if ((this.transform.position+change).x > boundary.transform.position.x)
					return true;
				else
					return false;
			case "TOP":
				if ((this.transform.position+change).y > boundary.transform.position.y)
					return true;
				else
					return false;
			case "BOT":
				if ((this.transform.position+change).y < boundary.transform.position.y)
					return true;
				else
					return false;
			default:
				return true;
		}
	}
	public Vector3 returnDistance() {
		Vector3 distance;
		switch (this.side)
		{
		case "LEFT":
			distance = boundary.transform.position-this.transform.position;
			distance = new Vector3(distance.x,0,0);
			break;
		case "RIGHT":
			distance = boundary.transform.position-this.transform.position;
			distance = new Vector3(distance.x,0,0);
			break;
		case "TOP":
			distance = boundary.transform.position-this.transform.position;
			distance = new Vector3(0,distance.y,0);
			break;
		case "BOT":
			distance = boundary.transform.position-this.transform.position;
			distance = new Vector3(0,distance.y,0);
			break;
		default:
			distance = Vector3.zero;
			break;
		}
		return distance;
	}
	/*
	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "Boundary")
		{
			switch (side)
			{
				case "LEFT":
				if (coll.gameObject.name == "LeftBound")
				{
					map.leftBound = true;
					Debug.Log("Collision with "+side);
				}
				break;
				case "RIGHT":
				if (coll.gameObject.name == "RightBound")
				{
					map.rightBound = true;
					Debug.Log("Collision with "+side);
				}
				break;
				case "TOP":
				if (coll.gameObject.name == "TopBound")
				{
					map.topBound = true;
					Debug.Log("Collision with "+side);
				}
				break;
				case "BOT":
				if (coll.gameObject.name == "BotBound")
				{
					map.botBound = true;
					Debug.Log("Collision with "+side);
				}
				break;
				default:
				break;
			}
		}
	}
	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.tag == "Boundary")
		{
			switch (side)
			{
			case "LEFT":
				if (coll.gameObject.name == "LeftBound")
				{
					map.leftBound = false;
					Debug.Log("No more collision with "+side);
				}
				break;
			case "RIGHT":
				if (coll.gameObject.name == "RightBound")
				{
					map.rightBound = false;
					Debug.Log("No more collision with "+side);
				}
				break;
			case "TOP":
				if (coll.gameObject.name == "TopBound")
				{
					map.topBound = false;
					Debug.Log("No more collision with "+side);
				}
				break;
			case "BOT":
				if (coll.gameObject.name == "BotBound")
				{
					map.botBound = false;
					Debug.Log("No more collision with "+side);
				}
				break;
			default:
				break;
			}
		}
	}*/
}
