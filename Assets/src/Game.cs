using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public static Game Instance
    {
        get
        {
            if (s_instance == null)
            {
                GameObject go = new GameObject("Game");
                s_instance = go.AddComponent<Game>();
            }
            return s_instance;
        }

    }

    public float thresholdAxis = 0.3f;

    public List<string> Players;

    public List<WeaponSelector> WeaponsConfig;
    
    public int MaxScore = 8;

    private static Game s_instance;


}

