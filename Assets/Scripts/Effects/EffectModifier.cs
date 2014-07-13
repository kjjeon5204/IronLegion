using UnityEngine;
using System.Collections;

public class EffectModifier : MonoBehaviour {
    public float rotationValueX;
    public float rotationValueY;
    public float rotationValueZ;
	public float scaleX;
	public float scaleXFinal;
	public float scaleY;
	public float scaleYFinal;
	public float scaleZ;
	public float scaleZFinal;
	public Texture[] textureChange;
	public float textureSwitchSpeed;
	public GameObject modelObject;
	public bool loop;
	float timeTracker;
	int textureCounter = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		transform.Rotate (rotationValueX * Time.deltaTime, rotationValueY * Time.deltaTime, rotationValueZ * Time.deltaTime);

		if (transform.localScale.x < scaleXFinal ) {
			transform.localScale += new Vector3 (scaleX * Time.deltaTime,0 ,0);
		}
		if (transform.localScale.y < scaleYFinal ) {
			transform.localScale += new Vector3 (0, scaleY * Time.deltaTime, 0);
		}
		if (transform.localScale.z < scaleZFinal ) {
			transform.localScale += new Vector3 (0 ,0 , scaleZ * Time.deltaTime);
		}

		if (loop == false && textureSwitchSpeed > 0) { 
			if (timeTracker < Time.time && textureCounter < textureChange.Length) {
				modelObject.renderer.material.mainTexture = textureChange [textureCounter];
				timeTracker = Time.time + textureSwitchSpeed;
				textureCounter++;
			}
		}

		if (loop == true && textureSwitchSpeed > 0) {
			if (timeTracker < Time.time) {
				modelObject.renderer.material.mainTexture = textureChange [textureCounter];
				timeTracker = Time.time + textureSwitchSpeed;
				textureCounter++;
				if (textureCounter == textureChange.Length) {
					textureCounter = 0;
				}
			}
		}
	}
}
