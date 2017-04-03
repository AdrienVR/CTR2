
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public Camera m_pauseCamera;
    public PauseMenuManager m_pauseManager;
    private bool m_paused;

    void Awake()
    {
        if (!enabled)
            return;
        ControllerManager.Instance.OnMenuInput += OnMenuInputCB;
        Instance = this;
    }

    private void OnMenuInputCB(MenuInput _input, int _controllerIndex)
    {
        if (_input == MenuInput.Start)
        {
            if (m_paused)
                Resume();
            else
                Pause();
            m_paused = !m_paused;
        }
    }

    public void Pause()
    {
        m_pauseCamera.enabled = true;
        m_pauseManager.Pause();
        AudioManager.Instance.Play("downMenu");
        AudioManager.Instance.PauseLoopingMusic();
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        m_pauseCamera.enabled = false;
        m_pauseManager.Resume();
        AudioManager.Instance.Play("cancelMenu");
        AudioManager.Instance.ResumeLoopingMusic();
        Time.timeScale = 1f;
    }
}
