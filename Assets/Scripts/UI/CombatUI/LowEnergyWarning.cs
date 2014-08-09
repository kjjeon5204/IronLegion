using UnityEngine;
using System.Collections;

public class LowEnergyWarning : MonoBehaviour {
    float timer;
    float maxVolume;
    public AudioSource thisAudio;

    void OnEnable()
    {
        timer = Time.time + 2.0f;
        thisAudio.volume = 0.0f;
    }

	// Use this for initialization
	void Start () {
        maxVolume = thisAudio.volume;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time < timer - 1.5f)
            thisAudio.volume += 0.5f * Time.deltaTime;
        if (Time.time > timer - 0.5f)
            thisAudio.volume -= 0.5f * Time.deltaTime;
        if (Time.time > timer)
        {
            gameObject.SetActive(false);
        }
	}
}
