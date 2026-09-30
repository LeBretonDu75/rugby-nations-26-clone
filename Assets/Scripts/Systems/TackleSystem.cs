using UnityEngine;

public class TackleSystem : MonoBehaviour
{
    public float tackleDistance = 2f;

    void Update()
    {
        var ball = GameManager.Instance.ball;
        if (ball.carrier == null) return;

        var carrier = ball.carrier;
        var enemyTeam = carrier.teamId == 0 ? GameManager.Instance.teamB : GameManager.Instance.teamA;

        foreach (var enemy in enemyTeam.players)
        {
            if (enemy.tackleCooldown > 0) continue;
            if (Vector3.Distance(enemy.transform.position, carrier.transform.position) < tackleDistance)
            {
                ExecuteTackle(enemy, carrier);
                break;
            }
        }
    }

    void ExecuteTackle(PlayerController tackler, PlayerController carrier)
    {
        carrier.GetTackled();
        GameManager.Instance.ball.Detach();
        GameManager.Instance.ball.rb.velocity = (Random.insideUnitSphere + Vector3.up) * 3f;

        // Lancer Ruck
        FindObjectOfType<RuckSystem>().StartRuck(carrier.transform.position, carrier.teamId, tackler.teamId);
    }
}