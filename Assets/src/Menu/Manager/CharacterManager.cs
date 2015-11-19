using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject[] Selectors;

    public void Enable2PlayersSelectors()
    {
        m_playerStates = new List<bool> { false, false };
        PlayerManager.Instance.CurrentPlayers = new List<string> { "Crash", "Crash" };
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
        return true;
    }

    private List<bool> m_playerStates;
}