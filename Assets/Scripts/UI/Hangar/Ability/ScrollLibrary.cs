using UnityEngine;
using System.Collections;

public class ScrollLibrary : MonoBehaviour {
	public bool Right;
	private Button button;
	public bool clicking;
	
	public Vector3 change;
	private GameObject leftBound;
	private GameObject rightBound;
	
	private GameObject libraryStart;
	private GameObject libraryEnd;
	// Use this for initialization
	void Start () {
		button = gameObject.GetComponent<Button>();
		leftBound = GameObject.Find("LibraryEndLeft");
		rightBound = GameObject.Find("LibraryEndRight");
		
		libraryStart = GameObject.Find("LibraryControl");
		libraryEnd = GameObject.Find("End Library");
		
		change = new Vector3(3f,0,0);
	}
	
	// Update is called once per frame
	void Update () {
		clicking = button.beginClick;
		
		if (clicking == true && Right && libraryStart.transform.position.x < leftBound.transform.position.x)
		{
			libraryStart.transform.position += (change*Time.deltaTime);
		}
		else if (clicking == true && !Right && libraryEnd.transform.position.x > rightBound.transform.position.x)
		{
			libraryStart.transform.position -= (change*Time.deltaTime);
		}
		
		if (Input.GetKey(KeyCode.U) && libraryEnd.transform.position.x > rightBound.transform.position.x )
		{
			libraryStart.transform.position -= (change*Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.I) && libraryStart.transform.position.x < leftBound.transform.position.x)
		{
			libraryStart.transform.position += (change*Time.deltaTime);
		}
	}
}
