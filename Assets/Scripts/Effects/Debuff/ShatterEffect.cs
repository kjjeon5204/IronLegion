using UnityEngine;
using System.Collections;

public class ShatterEffect : MonoBehaviour {
    public SkinnedMeshRenderer planeTexture;

    void OnEnable()
    {
        animation.Play("shatter");
        Color tempStore = planeTexture.material.GetColor("_TintColor");
        tempStore.a = 0.75f;
        planeTexture.material.SetColor("_TintColor", tempStore);
    }

	// Use this for initialization
	void Start () {
        animation.Play("shatter");
        animation["shatter"].speed = 0.6f;
	}
	
	// Update is called once per frame
	void Update () {
        if (!animation.IsPlaying("shatter"))
        {
            gameObject.SetActive(false);
        }
        
        if (planeTexture.material != null)
        {
            Color tempStore = planeTexture.material.GetColor("_TintColor");
            tempStore.a -= 0.75f * Time.deltaTime;
            planeTexture.material.SetColor("_TintColor", tempStore);
        }
	}
}
