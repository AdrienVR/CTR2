
using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    public Transform[] SpawnPoints;

    public void Start()
    {
        PlayerManager.Instance.InstantiatePlayers(SpawnPoints);
    }
}