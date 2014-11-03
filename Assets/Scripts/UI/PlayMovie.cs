using UnityEngine;
using System.Collections;

public class PlayMovie : MonoBehaviour {
    //public MovieTexture movTexture;
    //public AudioSource movieSpeakers;
    public GameObject loadingScreen;
    bool loadNextSceneEnabled = false;

	// Use this for initialization
	void Start () {
        StartCoroutine(play_my_video());
	}
	
	// Update is called once per frame
	void Update () {
        if (loadNextSceneEnabled == false && (/*movTexture.isPlaying == false || */
            Input.touchCount > 0))
        {
            loadingScreen.SetActive(true);
            loadNextSceneEnabled = true;
        }
	}

    IEnumerator play_my_video()
    {
        Handheld.PlayFullScreenMovie("IntroVideo.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.AspectFit);

        if (loadNextSceneEnabled == false && 
           Input.touchCount > 0)
        {
            loadingScreen.SetActive(true);
            loadNextSceneEnabled = true;
        }
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        loadingScreen.SetActive(true);
        loadNextSceneEnabled = true;
    }
}
