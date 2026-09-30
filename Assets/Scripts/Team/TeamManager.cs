using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public int teamId;
    public List<PlayerController> players = new List<PlayerController>();
    public int score = 0;
    public Transform formationCenter;

    public void AddScore(int pts) { score += pts; UIManager.Instance.UpdateScore(); }

    public void ResetFormation()
    {
        for (int i = 0; i < players.Count; i++)
        {
            Vector3 pos = formationCenter.position + new Vector3(Random.Range(-10,10), 0, Random.Range(-20,20));
            players[i].agent.Warp(pos);
            players[i].tackleCooldown = 0;
        }
    }

    public PlayerController GetBestSupportPlayer(PlayerController carrier, bool longPass)
    {
        PlayerController best = null;
        float bestScore = -1;
        foreach (var p in players)
        {
            if (p == carrier || p.tackleCooldown > 0) continue;
            float dist = Vector3.Distance(carrier.transform.position, p.transform.position);
            if (longPass && dist < 15f) continue;
            if (!longPass && dist > 15f) continue;
            // Le joueur doit être derrière ou à côté (pas d'en-avant)
            if (teamId == 0 && p.transform.position.z > carrier.transform.position.z + 2f) continue;
            if (teamId == 1 && p.transform.position.z < carrier.transform.position.z - 2f) continue;

            float score = 100 - dist;
            if (score > bestScore) { bestScore = score; best = p; }
        }
        return best;
    }

    public PlayerController GetBallCarrier()
    {
        return players.Find(p => p.hasBall);
    }
}