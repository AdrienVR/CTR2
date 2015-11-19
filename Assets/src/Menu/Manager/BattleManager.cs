using UnityEngine;

class BattleManager : MonoBehaviour
{
    public MapSelector MapSelector;

    public void StartBattle()
    {
        Application.LoadLevel(MapSelector.SelectedMap);
        PlayerManager.Instance.InstantiatePlayers();
    }
}