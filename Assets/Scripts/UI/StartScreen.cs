using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class StartScreen : MonoBehaviour {
    bool loggedIn;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success == true)
            {
                //successful log in
            }
            else
            {
                //log in failed
            }
        });
	}
}
