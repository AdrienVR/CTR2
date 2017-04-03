using UnityEngine;
using System;

public class MapSelector : MonoBehaviour
{
    [Serializable]
    public class MapModel
    {
        public GameObject GameObject;
#if UNITY_EDITOR
        public UnityEditor.SceneAsset Scene;
#endif
        [HideInInspector]
        public string SceneName;
    }

    public MapModel[] MapModels;

#if UNITY_EDITOR
    public void OnValidate()
    {
        for (int i = 0; i < MapModels.Length; i++)
        {
            MapModels[i].SceneName = MapModels[i].Scene.name;
        }
    }
#endif

    private void OnEnable()
    {
        Debug.Log(MapModels[m_index].SceneName);
        PlayModeManager.Instance.PlayingMap = MapModels[m_index].SceneName;
    }

    public void NextLeft()
    {
        MapModels[m_index].GameObject.SetActive(false);
        m_index = Mod(--m_index, MapModels.Length);
        MapModels[m_index].GameObject.SetActive(true);
        PlayModeManager.Instance.PlayingMap = MapModels[m_index].SceneName;
    }

    public void NextRight()
    {
        MapModels[m_index].GameObject.SetActive(false);
        m_index = Mod(++m_index, MapModels.Length);
        MapModels[m_index].GameObject.SetActive(true);
        PlayModeManager.Instance.PlayingMap = MapModels[m_index].SceneName;
    }

    private int Mod(int x, int m)
    {
        if (m == 0)
            return x;
        return (x % m + m) % m;
    }

    [SerializeField]
    private int m_index;
}
