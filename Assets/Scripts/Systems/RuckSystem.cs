using UnityEngine;

public class RuckSystem : MonoBehaviour
{
    public bool inRuck = false;
    public float ruckDuration = 3f;
    public int teamACount = 0;
    public int teamBCount = 0;

    public void StartRuck(Vector3 pos, int attackingTeam, int defendingTeam)
    {
        if (inRuck) return;
        inRuck = true;
        GameManager.Instance.currentState = GameManager.GameState.Ruck;
        teamACount = teamBCount = 0;
        Invoke(nameof(ResolveRuck), ruckDuration);
        Debug.Log("RUCK!");
    }

    void ResolveRuck()
    {
        // L'équipe avec le plus de joueurs dans la zone gagne
        int winner = teamACount >= teamBCount ? 0 : 1;
        var team = winner == 0 ? GameManager.Instance.teamA : GameManager.Instance.teamB;
        var scrumHalf = team.players[8]; // 9
        GameManager.Instance.ball.AttachTo(scrumHalf);
        scrumHalf.hasBall = true;

        inRuck = false;
        GameManager.Instance.currentState = GameManager.GameState.Playing;
    }

    void OnTriggerStay(Collider other)
    {
        if (!inRuck) return;
        var p = other.GetComponent<PlayerController>();
        if (p == null) return;
        if (p.teamId == 0) teamACount++;
        else teamBCount++;
    }
}