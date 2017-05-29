using UnityEngine;

public static class GameObjectUtils
{
#if UNITY_EDITOR
    public static bool IsPrefabAsset(GameObject _gameObject)
    {
        return (UnityEditor.PrefabUtility.GetPrefabType(_gameObject) == UnityEditor.PrefabType.Prefab);
    }
#endif
}