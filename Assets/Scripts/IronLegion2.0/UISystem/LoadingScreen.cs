using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {
    public string sceneToLoad = "NULL";
    public GameObject loadingCircle;

    public void set_loading_scene(string loadScene)
    {
        sceneToLoad = loadScene;
    }

    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        if (sceneToLoad == "NULL") sceneToLoad = UserData.nextTargetScene;
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
