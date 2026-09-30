using UnityEngine;

public class HumanInput : MonoBehaviour
{
    public PlayerController currentControlled;
    public BallController ball;
    public float passRange = 20f;

    void Update()
    {
        if (currentControlled == null || GameManager.Instance.currentState != GameManager.GameState.Playing) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space);

        Vector3 move = new Vector3(h, 0, v).normalized;
        if (move.magnitude > 0.1f)
        {
            currentControlled.MoveTo(currentControlled.transform.position + move * 10f, sprint);
            currentControlled.transform.rotation = Quaternion.Slerp(currentControlled.transform.rotation, Quaternion.LookRotation(move), 10f * Time.deltaTime);
        }
        else currentControlled.Stop();

        if (Input.GetKeyDown(KeyCode.A)) TryPass(false);
        if (Input.GetKeyDown(KeyCode.W)) TryPass(true);
        if (Input.GetKeyDown(KeyCode.E)) TryKick();
        if (Input.GetKeyDown(KeyCode.Tab)) SwitchPlayer();
    }

    void TryPass(bool longPass)
    {
        if (!currentControlled.hasBall) return;
        var team = currentControlled.teamId == 0 ? GameManager.Instance.teamA : GameManager.Instance.teamB;
        var target = team.GetBestSupportPlayer(currentControlled, longPass);
        if (target != null)
        {
            ball.PassTo(target.transform.position + Vector3.up, longPass ? 22f : 16f);
            currentControlled.hasBall = false;
            // Auto-switch
            currentControlled = target;
            currentControlled.isHuman = true;
        }
    }

    void TryKick()
    {
        if (!currentControlled.hasBall) return;
        Vector3 dir = currentControlled.transform.forward;
        ball.Kick(dir, 25f);
    }

    void SwitchPlayer()
    {
        var team = GameManager.Instance.teamA;
        PlayerController closest = null;
        float dist = Mathf.Infinity;
        foreach (var p in team.players)
        {
            if (p.hasBall) continue;
            float d = Vector3.Distance(p.transform.position, ball.transform.position);
            if (d < dist) { dist = d; closest = p; }
        }
        if (closest != null)
        {
            currentControlled.isHuman = false;
            currentControlled = closest;
            currentControlled.isHuman = true;
        }
    }
}