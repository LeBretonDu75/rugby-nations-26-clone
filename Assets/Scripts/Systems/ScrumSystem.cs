using UnityEngine;

public class ScrumSystem : MonoBehaviour
{
    public void TriggerScrum(Vector3 pos)
    {
        GameManager.Instance.currentState = GameManager.GameState.Scrum;
        Debug.Log("MELEE");
        // Mini-jeu de mêlée : appuis rapides
        Invoke(nameof(WinScrum), 2.5f);
    }

    void WinScrum()
    {
        int winner = Random.Range(0,2); // Basé sur stats pack
        var team = winner == 0 ? GameManager.Instance.teamA : GameManager.Instance.teamB;
        GameManager.Instance.ball.AttachTo(team.players[8]);
        team.players[8].hasBall = true;
        GameManager.Instance.currentState = GameManager.GameState.Playing;
    }
}