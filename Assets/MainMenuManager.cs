using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        AudioManager.Instance.Play("menu", true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideStaticElements()
    {

    }

    public void StartBattle()
    {
        AudioManager.Instance.StopLoopingSound();
        Application.LoadLevel("plage");
    }
}
