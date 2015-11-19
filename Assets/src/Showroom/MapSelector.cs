using UnityEngine;
using System;

public class MapSelector : MonoBehaviour
{
    [Serializable]
    public class MapModel
    {
        public GameObject GameObject;
        public string SceneName;
    }

    public MapModel[] MapModels;
    public string SelectedMap;

    public void NextLeft()
    {
        MapModels[m_index].GameObject.SetActive(false);
        m_index = Mod(--m_index, MapModels.Length);
        MapModels[m_index].GameObject.SetActive(true);
        SelectedMap = MapModels[m_index].SceneName;
    }

    public void NextRight()
    {
        MapModels[m_index].GameObject.SetActive(false);
        m_index = Mod(++m_index, MapModels.Length);
        MapModels[m_index].GameObject.SetActive(true);
        SelectedMap = MapModels[m_index].SceneName;
    }

    private int Mod(int x, int m)
    {
        if (m == 0)
            return x;
        return (x % m + m) % m;
    }

    private int m_index;
}
