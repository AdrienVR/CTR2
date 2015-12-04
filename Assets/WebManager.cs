using UnityEngine;
using System.IO;

public class WebManager : MonoBehaviour {

    const string relativePath = "Donation";

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void OpenDonationLink()
    {
        string pathPrefix = "file://";
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        pathPrefix = "file:///";
#endif

        string url = Path.Combine(Application.streamingAssetsPath, Path.Combine(relativePath, "donate.html"));
        string absoluteURL = pathPrefix + url.Replace(" ", "%20");

        Application.OpenURL(absoluteURL);
    }
}
