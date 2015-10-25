using UnityEngine;
using System.Collections;

public class tester : MonoBehaviour {

    public Color cac;

	// Use this for initialization
	void Start () {
        AudioManager.Instance.Play("skullrock");
        StartCoroutine(caca());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator caca ()
    {
        yield return new WaitForSeconds(3);
        Application.LoadLevel("plage");
    }
}
