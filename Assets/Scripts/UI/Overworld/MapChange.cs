using UnityEngine;
using System.Collections;

public class MapChange : MonoBehaviour {
	public enum Side
	{
		LEFT,
		RIGHT
	}
	public Side side;
	public MapControls controls;
	
	void Clicked() {
		if (side == Side.LEFT)
			controls.ChangeMap(-1);
		else if (side == Side.RIGHT)
			controls.ChangeMap(1);
	}
}
