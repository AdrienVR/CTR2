using System.Collections;
using UnityEngine;
using UnityEngine.UI;

class PlayModeManager : MonoBehaviour
{
    public static PlayModeManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                GameObject go = PrefabReferences.Instance.PlayerManager;
                DontDestroyOnLoad(go);
                s_instance = go.GetComponent<PlayModeManager>();
            }
            return s_instance;
        }
    }

    private static PlayModeManager s_instance;

    public MapSelector MapSelector;
    public Text[] ScoreText;

    void Start()
    {
        s_instance = this;
        m_playMode = new Battle();
        DontDestroyOnLoad(gameObject);
    }

    public void SelectBattle()
    {
        m_playMode = new Battle();
    }

    public void StartBattle()
    {
        DontDestroyOnLoad(gameObject);

        Application.LoadLevel(MapSelector.SelectedMap);
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
        Application.LoadLevel("mainmenu");
    }

    private PlayMode m_playMode;
}