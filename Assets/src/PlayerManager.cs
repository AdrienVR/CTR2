//#define PLAYERMANAGER_DEBUG

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
        [HideInInspector]
        public string Name;
        public GameObject Prefab;
        public CharacterSide Side;
#if PLAYERMANAGER_DEBUG
        public Mesh CharacterMesh;
        public Material CharacterMaterial;
        public Material KartMaterial;
        public Vector3 CharacterRotation;
        public Vector3 CharacterScale;
#else
        [HideInInspector]
        public Mesh CharacterMesh;
        [HideInInspector]
        public Material CharacterMaterial;
        [HideInInspector]
        public Material KartMaterial;
        [HideInInspector]
        public Vector3 CharacterRotation;
        [HideInInspector]
        public Vector3 CharacterScale;
#endif

        public void OnValidate()
        {
            Name = Prefab.name;
            CharacterMesh = Prefab.GetComponentInChildren<MeshFilter>().sharedMesh;
            CharacterMaterial = Prefab.GetComponentInChildren<MeshRenderer>().sharedMaterial;
            CharacterRotation = Prefab.GetComponentInChildren<MeshFilter>().transform.localRotation.eulerAngles;
            CharacterScale = Prefab.GetComponentInChildren<MeshFilter>().transform.localScale;
            KartMaterial = Prefab.GetComponentsInChildren<MeshRenderer>()[1].sharedMaterial;
        }
    }

    public GameObject KartPlayerPrefab;
    public MeshRenderer[] PrefabRenderers;

    public bool DestroyPrefabInstances;

    public PlayableCharacter[] PlayableCharacters;

    public int TotalPlayers = 2;

    public List<string> CurrentPlayers = new List<string>(4);

    void Awake()
    {
        if (!enabled)
            return;
        Instance = this;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (GameObjectUtils.IsPrefabAsset(gameObject))
            return;
        PrefabRenderers = KartPlayerPrefab.GetComponentsInChildren<MeshRenderer>();
        foreach(PlayableCharacter character in PlayableCharacters)
        {
            character.OnValidate();
        }
    }

    [ContextMenu("Update Character Prefabs")]
    void UpdateCharacterPrefabs()
    {
        OnValidate();
        foreach (PlayableCharacter character in PlayableCharacters)
        {
            GameObject go = Instantiate(KartPlayerPrefab) as GameObject;

            go.name = character.Name;
            go.GetComponentInChildren<MeshFilter>().sharedMesh = character.CharacterMesh;
            go.GetComponentInChildren<MeshRenderer>().sharedMaterial = character.CharacterMaterial;
            go.GetComponentInChildren<MeshFilter>().transform.localRotation = Quaternion.Euler(character.CharacterRotation);
            go.GetComponentInChildren<MeshFilter>().transform.localScale = character.CharacterScale;
            go.GetComponentsInChildren<MeshRenderer>()[1].sharedMaterial = character.KartMaterial;
            
            GameObject prefab = UnityEditor.PrefabUtility.CreatePrefab("Assets/Prefabs/Characters/" + go.name + ".prefab", go);
            UnityEditor.PrefabUtility.ConnectGameObjectToPrefab(go, prefab);
            character.Prefab = prefab;
            if (DestroyPrefabInstances)
                DestroyImmediate(GameObject.Find(character.Name));
        }
        OnValidate();
    }
#endif

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

    private PlayableCharacter GetPlayableCharacterFromPrefab(string name)
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