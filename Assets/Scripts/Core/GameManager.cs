using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { KickOff, Playing, Ruck, Scrum, TryScored, Paused }
    
    public GameState currentState = GameState.Playing;
    public TeamManager teamA;
    public TeamManager teamB;
    public BallController ball;
    public FieldManager field;
    public float matchTime = 300f;
    private float currentTime;

    void Awake() { Instance = this; currentTime = matchTime; }

    void Update()
    {
        if (currentState != GameState.Playing) return;
        currentTime -= Time.deltaTime;
        if (currentTime <= 0) EndHalf();

        int tryResult = field.CheckTry(ball.transform.position);
        if (tryResult != -1) ScoreTry(tryResult);
    }

    void ScoreTry(int teamId)
    {
        currentState = GameState.TryScored;
        if (teamId == 0) teamA.AddScore(5);
        else teamB.AddScore(5);
        Debug.Log($"ESSAI EQUIPE {teamId}!");
        Invoke(nameof(ResetAfterTry), 3f);
    }

    void ResetAfterTry()
    {
        currentState = GameState.Playing;
        ball.ResetBall();
        teamA.ResetFormation();
        teamB.ResetFormation();
    }

    void EndHalf() { Debug.Log("Mi-temps"); currentTime = matchTime; }
}