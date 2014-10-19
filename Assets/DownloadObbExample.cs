using UnityEngine;
using System.Collections;

public class DownloadObbExample : MonoBehaviour {
	
	void OnGUI()
	{
		if (!GooglePlayDownloader.RunningOnAndroid())
		{
			GUI.Label(new Rect(10, 10, Screen.width-10, 20), "Use GooglePlayDownloader only on Android device!");
			return;
		}
		
		string expPath = GooglePlayDownloader.GetExpansionFilePath();
		if (expPath == null)
		{
				GUI.Label(new Rect(10, 10, Screen.width-10, 20), "External storage is not available!");
		}
		else
		{
			string mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
			//string patchPath = GooglePlayDownloader.GetPatchOBBPath(expPath);
			
			//GUI.Label(new Rect(10, 10, Screen.width-10, 20), "Main = ..."  + ( mainPath == null ? " NOT AVAILABLE" :  mainPath.Substring(expPath.Length)));
			//GUI.Label(new Rect(10, 25, Screen.width-10, 20), "Patch = ..." + (patchPath == null ? " NOT AVAILABLE" : patchPath.Substring(expPath.Length)));
            if (Input.touchCount > 0) 
            {
                if (mainPath == null)
                {
                    GooglePlayDownloader.FetchOBB();
                }
                else
                {
                    Application.LoadLevel(1);
                    StartCoroutine(LoadLevel(1));
                }
            }
		}
	}
    
    protected IEnumerator LoadLevel(int strSceneName)
    {
        string expPath = GooglePlayDownloader.GetExpansionFilePath();
        string mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);

        string uri = string.Empty;
        uri = "jar:file://" + mainPath + "!/" + "scene.unity3d";

        Debug.Log("downloading " + uri);
        WWW www = WWW.LoadFromCacheOrDownload(uri, 0);

        // Wait for download to complete
        yield return www;

        if (www.error != null)
        {
            Debug.Log("wwww error " + www.error);
        }
        else
        {
            AssetBundle assetBundle = www.assetBundle;
            assetBundle.LoadAll();
            Application.LoadLevel(1);
        }
    }
     
}
