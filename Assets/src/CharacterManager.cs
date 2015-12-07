using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject[] Selectors;
    public GameObject[] ShowroomCameras;

    public void Enable2PlayersSelectors()
    {
        m_playerStates = new List<bool> { false, false };
        PlayerManager.Instance.CurrentPlayers = new List<string> { "Crash", "Crash" };
        EnableSelectors(2);
    }

    public void Enable3PlayersSelectors()
    {
        m_playerStates = new List<bool> { false, false, false };
        PlayerManager.Instance.CurrentPlayers = new List<string> { "Crash", "Crash", "Crash" };
        EnableSelectors(3);
    }

    public void Enable4PlayersSelectors()
    {
        m_playerStates = new List<bool> { false, false, false, false };
        PlayerManager.Instance.CurrentPlayers = new List<string> { "Crash", "Crash", "Crash", "Crash" };
        EnableSelectors(4);
    }

    private void EnableSelectors(int number)
    {
        for (int i = 0; i < Selectors.Length; i++)
        {
            if (i < number)
            {
                Selectors[i].SetActive(true);
                ShowroomCameras[i].SetActive(true);
            }
            else
            {
                Selectors[i].SetActive(false);
                ShowroomCameras[i].SetActive(false);
            }
        }
    }

    public bool SetPlayerValidationState(int playerIndex, bool state, string name)
    {
        m_playerStates[playerIndex] = state;
        PlayerManager.Instance.CurrentPlayers[playerIndex] = name;
        return CheckPlayersReady();
    }

    private bool CheckPlayersReady()
    {
        foreach (bool playerState in m_playerStates)
        {
            if (playerState == false)
            {
                return false;
            }
        }
        for (int i = 0; i < m_playerStates.Count; i++)
        {
            m_playerStates[i] = false;
        }
        return true;
    }

    private List<bool> m_playerStates;
}