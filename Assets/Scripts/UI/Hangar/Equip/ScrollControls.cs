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
	}
	
	void EndClick() {
		slider.transform.position = gameObject.transform.position;
	}
	
	void CanceledClick() {
		slider.transform.position = gameObject.transform.position;
	}
	//------------------------------------------
	void Update() {
		float percentage = (slider.transform.position.y-gameObject.transform.position.y)/scrollbar_offset;
		if (percentage != 0)
		{
			ScrollBox(percentage);
		}
	}
	//------------------------------------------
	public void ScrollBox(float percentage) {
		if (percentage  > 0 && inventory_top.transform.position.y > inventory_top_bound.transform.position.y)
		{
			//Things to scroll
		}
		else if (inventory_top.transform.position.y > inventory_top_bound.transform.position.y)
		{
			//Things to scroll
		}
	}
}
