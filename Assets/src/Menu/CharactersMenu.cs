
using System.Collections.Generic;
using UnityEngine;

public class CharactersMenu : Menu
{
    public GameObject Characters;
    public GameObject ShowRooms;
    public Menu NextMenu;

    protected override void OnEnable()
    {
        base.OnEnable();
        Characters.SetActive(true);
        ShowRooms.SetActive(true);
        SetSelectors(true);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Characters.SetActive(false);
        ShowRooms.SetActive(false);
        SetSelectors(false);
    }

    public GameObject[] Selectors;
    public GameObject[] ShowroomCameras;

    public void EnablePlayersSelectors(int i)
    {
        PlayerManager.Instance.TotalPlayers = i;
    }

    private void SetSelectors(bool _state)
    {
        m_playerStates.Clear();
        for (int i = 0; i < PlayerManager.Instance.TotalPlayers; ++i)
        {
            m_playerStates.Add(_state);
            Selectors[i].SetActive(_state);
            ShowroomCameras[i].SetActive(_state);
        }
    }

    public void SetPlayerValidationState(int playerIndex, bool state, string name)
    {
        m_playerStates[playerIndex] = state;
        PlayerManager.Instance.CurrentPlayers[playerIndex] = name;
        if (CheckPlayersReady())
            MainMenuManager.Instance.MoveToMenu(NextMenu);
    }

    private bool CheckPlayersReady()
    {
        for (int i = 0; i < m_playerStates.Count; i++)
        {
            if (! m_playerStates[i])
                return false;
        }
        return true;
    }

    private List<bool> m_playerStates = new List<bool>(4);
}
