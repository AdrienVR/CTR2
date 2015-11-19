
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Singleton
    public static PlayerManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                GameObject go = PrefabReferences.Instance.PlayerManager;
                DontDestroyOnLoad(go);
                s_instance = go.GetComponent<PlayerManager>();
            }
            return s_instance;
        }

    }
    private static PlayerManager s_instance;
    public List<string> CurrentPlayers;
}