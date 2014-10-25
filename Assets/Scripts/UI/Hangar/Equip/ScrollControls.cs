using UnityEngine;
using System.Collections;

public class ScrollControls : MonoBehaviour {
	public GameObject inventory_top;
	public GameObject inventory_bot;
	public GameObject inventory_top_bound;
	public GameObject inventory_bot_bound;
	
	public GameObject scroll_top;
	public GameObject scroll_bot;
	public GameObject slider;
	public float scroll_speed;
	private float scrollbar_offset = 1.85f;
	private Vector3 speed;
	public bool clicking;
	void Awake() {
		speed = new Vector3(0,scroll_speed,0);
	}
	
	public void Input(Vector2 position) {
		Vector3 new_position = Camera.main.ScreenToWorldPoint(new Vector3(position.x,position.y,0));
		if (new_position.y >= gameObject.transform.position.y+scrollbar_offset)
		{
			slider.transform.position = new Vector3(slider.transform.position.x, gameObject.transform.position.y+scrollbar_offset, slider.transform.position.z);
		}
		else if (new_position.y <= gameObject.transform.position.y-scrollbar_offset)
		{
			slider.transform.position = new Vector3(slider.transform.position.x,gameObject.transform.position.y-scrollbar_offset,slider.transform.position.z);
		}
		else
		{
			slider.transform.position = new Vector3(slider.transform.position.x,new_position.y,slider.transform.position.z);
		}
		clicking = true;
	}
	
	void EndClick() {
		slider.transform.position = gameObject.transform.position;
		clicking = false;
	}
	
	void CanceledClick() {
		slider.transform.position = gameObject.transform.position;
		clicking = false;
	}
	//------------------------------------------
	void Update() {
		float percentage = (slider.transform.position.y-gameObject.transform.position.y)/scrollbar_offset;
		if (percentage != 0)
		{
			ScrollBox(percentage);
		}
		if (!clicking)
		{
			slider.transform.position = gameObject.transform.position;
		}
	}
	//------------------------------------------
	public void ScrollBox(float per) {
		if (per  > 0 && inventory_top.transform.position.y > inventory_top_bound.transform.position.y)
		{
			//Things to scroll
			inventory_top.transform.position -= speed*(per*Time.deltaTime);
		}
		else if (per < 0 && inventory_bot.transform.position.y < inventory_bot_bound.transform.position.y)
		{
			//Things to scroll
			inventory_top.transform.position -= speed*(per*Time.deltaTime);
		}
	}
}
