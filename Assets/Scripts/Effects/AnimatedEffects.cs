using UnityEngine;
using System.Collections;

public class AnimatedEffects : MonoBehaviour {
    public bool animateTexture;
    public Material[] materials;
    int index = 0;
    public bool spinX;
	public float spinXRate = 1f;
    public bool spinY;
	public float spinYRate = 1f;
	public bool spinZ;
	public float spinZRate = 1f;
	public Vector3 scaleStartSize = Vector3.one;
	public bool scaleX;
	public float scaleXFinalSize;
	public float scaleXTime;
	public bool scaleY;
	public float scaleYFinalSize;
	public float scaleYTime;
	public bool scaleZ;
	public float scaleZFinalSize;
	public float scaleZTime;

    public bool pulsate;
    public Color firstColor = new Color(1, 1, 1, 1);
    public Color secondColor = new Color(1, 1, 1, .75f);
    public float rate = 1.0f;

	public bool fadeIn;
	public Color fadeFrom = new Color(0,0,0,0);
	public Color fadeTo = new Color(1,1,1,1);
	public float fadeTime = 1f;
	private Color tweenAmnt;

	public bool fadeOut;
	public Color fadeOutTo = new Color(0,0,0,0);
	public float fadeOutDuration;
	public float fadeOutEnd;
	private float startTime;

	private float spawnTime;
	private float _time = 0.0f;

	public bool slideTexture;
	public int materialIndex = 0;
    public Vector2 uvAnimationRate = new Vector2(0.0f, 1.0f);
    public string textureName = "_MainTex";

    Vector2 uvOffset = Vector2.zero;
    void OnEnable()
    {
		MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
        if (renderer!= null)
        {
			if(fadeIn)
			{
				tweenAmnt = fadeFrom - fadeTo;
				renderer.material.SetColor("_Multiplier", this.fadeFrom);
			}
			if(fadeOut)
			{
				startTime = Time.time;
			}
        }
		else
		{
			fadeOut = false;
			fadeIn = false;
			pulsate = false;
		}
		transform.localScale = scaleStartSize;
		spawnTime = Time.time;
    }
    void LateUpdate()
    {
        if (spinX)
        {
            transform.rotation *= Quaternion.Euler(Vector3.right * Time.deltaTime * 360 / spinYRate);
        }
        if (spinY)
        {
            transform.rotation *= Quaternion.Euler(Vector3.up * Time.deltaTime * 360 / spinYRate);
        }
		if (spinZ)
		{
			transform.rotation *= Quaternion.Euler(Vector3.forward * Time.deltaTime * 360 / spinXRate);			
		}
		if(scaleX||scaleY||scaleZ)
		{
			Vector3 scale = Vector3.zero;
			if (scaleX)
			{
				scale.x = ((scaleXFinalSize - transform.localScale.x)*(Time.deltaTime/scaleXTime));
			}
			if (scaleY)
			{
				scale.y = ((scaleYFinalSize - transform.localScale.y)*(Time.deltaTime/scaleYTime));
			}
			if (scaleZ)
			{
				scale.z = ((scaleZFinalSize - transform.localScale.z)*(Time.deltaTime/scaleZTime));
			}
			transform.localScale += scale;
		}
		if(pulsate||fadeIn||animateTexture)
		{
			foreach(MeshRenderer renderer in this.gameObject.GetComponentsInChildren<Renderer>())
			{
				if (!!renderer)
				{
					if (animateTexture)
					{
						renderer.material = materials[index];
						index++;
						if (index >= materials.Length)
						{
							index = 0;
						}
					}
					if (pulsate)
					{
						float a = Mathf.Cos(2 * Mathf.PI * this._time * this.rate) * 0.5f + 0.5f;
						renderer.material.SetColor("_Multiplier", a * this.firstColor + (1.0f - a) * this.secondColor);
						this._time = Mathf.Repeat(this._time + Time.deltaTime, 1.0f);
					}
					if(fadeIn)
					{
						renderer.material.SetColor("_Multiplier", renderer.material.GetColor("_Multiplier") + tweenAmnt*Time.deltaTime/fadeTime);
						if(Time.time - startTime >= fadeTime)
						{
							fadeIn = false;
						}
					}
					if(fadeOut)
					{
						if(Time.time - startTime >= fadeOutEnd - fadeOutDuration)
						{
							renderer.material.SetColor("_Multiplier",(renderer.material.GetColor("_Multiplier") - fadeOutTo)*(fadeOutEnd-Time.time)/fadeOutDuration);
						}
					}
					if(slideTexture)
					{
						uvOffset += (uvAnimationRate * Time.deltaTime);
						renderer.material.SetTextureOffset(textureName, uvOffset);
					}
				}
			}
		}
    }
}
[System.Serializable]
public class FiredEffect
{
    public GameObject spawnedObject;
    public GameObject spawnedLocation;
}
