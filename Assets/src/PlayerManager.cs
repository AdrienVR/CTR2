
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Singleton
    public static PlayerManager Instance;

    [Serializable]
    public class PlayableCharacter
    {
        public string Name;
        public CharacterSide Side;
        public GameObject Prefab;
    }

    public PlayableCharacter[] PlayableCharacters;

    public int TotalPlayers = 2;

    public List<string> CurrentPlayers = new List<string>(4);

    void Awake()
    {
        if (!enabled)
            return;
        Instance = this;
    }

    public List<PlayerController> GetEnemies(PlayerController player)
    {
        List<PlayerController> enemies = new List<PlayerController>();

        foreach(Team t in m_teams)
        {
            if (t.TeamID != player.Team.TeamID)
            {
                enemies.AddRange(t.Players);
            }
        }

        return enemies;
    }

    public void InstantiatePlayers(Transform[] spawnPoints)
    {
        m_teams = new List<Team>
        {
            new Team(Color.red, 0), new Team(Color.blue, 1),
            new Team(Color.green, 2),new Team(Color.yellow, 3)
        };

        for (int i = 0; i < CurrentPlayers.Count; i++)
        {
            string characterName = CurrentPlayers[i];
            PlayableCharacter character = GetPlayableCharacter(characterName);

            GameObject go = Instantiate(character.Prefab, spawnPoints[i].position, spawnPoints[i].rotation) as GameObject;

            PlayerController player = go.GetComponent<PlayerController>();
            m_teams[i].AddPlayer(player);
            player.PlayerIndex = i;
            player.CharacterSide = character.Side;
            player.CameraController = CameraConfig.Instance.InitializePlayer(player, i, CurrentPlayers.Count);
        }
    }

    private PlayableCharacter GetPlayableCharacter(string name)
    {
        foreach (PlayableCharacter character in PlayableCharacters)
        {
            if (character.Name == name)
            {
                return character;
            }
        }
        return null;
    }

    private List<Team> m_teams;
}

public enum CharacterSide
{
    NormalSide,
    DarkSide
}