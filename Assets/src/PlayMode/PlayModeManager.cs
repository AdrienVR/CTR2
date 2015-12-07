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
        m_playMode = new Battle();
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

    private PlayMode m_playMode;
}