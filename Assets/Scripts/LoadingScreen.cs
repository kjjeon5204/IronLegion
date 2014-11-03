using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {
    public string sceneToLoad;
    public GameObject loadingCircle;

    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        StartCoroutine(run_loading_screen());
    }

	// Update is called once per frame
	void Update () {

	}

    IEnumerator run_loading_screen()
    {
        AsyncOperation async = Application.LoadLevelAsync(sceneToLoad);

        while (!async.isDone)
        {
            if (loadingCircle != null)
                loadingCircle.transform.Rotate(Vector3.forward * 45.0f * Time.deltaTime);
            yield return null;
        }
    }
}
