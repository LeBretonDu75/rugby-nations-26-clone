using UnityEngine;

public class AIController : MonoBehaviour
{
    public TeamManager team;
    public TeamManager enemyTeam;
    public BallController ball;

    void Update()
    {
        if (GameManager.Instance.currentState != GameManager.GameState.Playing) return;
        if (team.players.Find(p => p.isHuman) != null && team.teamId == 0) return; // Equipe humaine

        var carrier = team.GetBallCarrier();
        if (carrier != null)
        {
            // Attaquant porteur : fonce vers l'en-but
            float targetZ = team.teamId == 0 ? 60f : -60f;
            Vector3 target = new Vector3(Random.Range(-15f, 15f), 0, targetZ);
            carrier.MoveTo(target, true);

            // Passe si en danger
            foreach(var e in enemyTeam.players)
            {
                if (Vector3.Distance(e.transform.position, carrier.transform.position) < 6f)
                {
                    var support = team.GetBestSupportPlayer(carrier, false);
                    if (support != null) ball.PassTo(support.transform.position, 16f);
                    break;
                }
            }
        }
        else
        {
            // Défense / soutien
            foreach (var p in team.players)
            {
                if (p.hasBall) continue;
                if (ball.isLoose) p.MoveTo(ball.transform.position);
                else if (team.GetBallCarrier() != null)
                    p.MoveTo(team.GetBallCarrier().transform.position + Random.insideUnitSphere * 10f);
            }
        }
    }
}