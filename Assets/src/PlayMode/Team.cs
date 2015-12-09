using UnityEngine;
using System.Collections.Generic;

public class Team
{
    public Team(Color color, int teamID)
    {
        TeamColor = color;
        TeamID = teamID;

        Players = new List<PlayerController>();
    }

    public Color TeamColor;

    public void AddPlayer(PlayerController player)
    {
        player.Team = this;
        Players.Add(player);
    }

    public int TeamID;

    public List<PlayerController> Players;
}
