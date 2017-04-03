using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class PlayModeManager : MonoBehaviour
{
    public static PlayModeManager Instance;

    public string PlayingMap;
    public Text[] ScoreText;

    void Start()
    {
        if (!enabled)
            return;
        Instance = this;

        m_playMode = new Battle();
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(PlayingMap))
            PlayingMap = SceneManager.GetActiveScene().name;
#endif
    }

    public void SelectBattle()
    {
        m_playMode = new Battle();
    }

    public void StartBattle()
    {
        SceneManager.LoadScene(PlayingMap);
        AudioManager.Instance.StopLoopingMusic();
    }

    public void BattleScorePlus()
    {
        m_playMode.MaxScore++;
        foreach(Text label in ScoreText)
        {
            label.text = m_playMode.MaxScore.ToString();
        }
    }

    public void BattleScoreLess()
    {
        --m_playMode.MaxScore;
        m_playMode.MaxScore = Mathf.Max(1, m_playMode.MaxScore);
        foreach (Text label in ScoreText)
        {
            label.text = m_playMode.MaxScore.ToString();
        }
    }

    public void UpdateDeath(PlayerController deadPlayer, PlayerController killer)
    {
        if (killer.NbPts >= m_playMode.MaxScore)
        {
            killer.gameObject.AddComponent<Party>();
            KartController.IA_enabled = true;
            Main.statistics.endGame();
            StartCoroutine(BackToMenu());
        }
    }

    private IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("mainmenu");
    }

    private PlayMode m_playMode;
}