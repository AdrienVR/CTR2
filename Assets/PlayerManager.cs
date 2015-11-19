
using System;
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

    [Serializable]
    public class PlayableCharacter
    {
        public string Name;
        public CharacterSide Side;
        public GameObject Prefab;
    }

    public PlayableCharacter[] PlayableCharacters;

    public List<string> CurrentPlayers;

    public void InstantiatePlayers()
    {
        foreach(string characterName in CurrentPlayers)
        {
            PlayableCharacter character = GetPlayableCharacter(characterName);
            GameObject go = Instantiate(character.Prefab);
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
}

public enum CharacterSide
{
    NormalSide,
    DarkSide
}