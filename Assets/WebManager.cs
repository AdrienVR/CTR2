using UnityEngine;
using System.Collections;
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
        string url = Path.Combine(Application.dataPath, Path.Combine(relativePath, "donation.html"));
        Application.OpenURL(url);
    }
}
