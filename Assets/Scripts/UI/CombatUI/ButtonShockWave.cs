using UnityEngine;
using System.Collections;

public class ButtonShockWave : MonoBehaviour {
    public SpriteRenderer mySprite;

    public void initialize_button()
    {
        mySprite.enabled = true;
        enabled = true;
    }

    float maxScaleSize;

    void Start()
    {
        maxScaleSize = 2.0f * transform.localScale.x;
    }

	// Update is called once per frame
	void Update () {
        transform.localScale += 3.0f * (new Vector3(1.0f, 1.0f, 0.0f) * Time.deltaTime);
        Color tempColor = mySprite.color;
        tempColor.a -= 6.0f * Time.deltaTime;
        mySprite.color = tempColor;
        if (transform.localScale.x > maxScaleSize)
        {
            Destroy(gameObject);
        }
	}

}