using UnityEngine;

public static class GameObjectUtils
{
    public static bool IsPrefabAsset(GameObject _gameObject)
    {
        return (UnityEditor.PrefabUtility.GetPrefabType(_gameObject) == UnityEditor.PrefabType.Prefab);
    }
}