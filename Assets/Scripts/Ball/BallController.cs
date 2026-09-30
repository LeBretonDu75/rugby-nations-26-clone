using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    public PlayerController carrier;
    public Rigidbody rb;
    public bool isLoose = true;

    void Awake() { rb = GetComponent<Rigidbody>(); }

    public void AttachTo(PlayerController player)
    {
        carrier = player;
        isLoose = false;
        rb.isKinematic = true;
        transform.SetParent(player.ballSocket);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Detach()
    {
        if (carrier != null) carrier.hasBall = false;
        carrier = null;
        isLoose = true;
        transform.SetParent(null);
        rb.isKinematic = false;
    }

    public void PassTo(Vector3 target, float speed = 18f)
    {
        Detach();
        Vector3 dir = (target - transform.position).normalized;
        dir.y = 0.2f;
        rb.velocity = dir * speed;
    }

    public void Kick(Vector3 direction, float power)
    {
        Detach();
        rb.velocity = direction.normalized * power + Vector3.up * 5f;
        rb.AddTorque(Random.insideUnitSphere * 10f);
    }

    public void ResetBall()
    {
        Detach();
        transform.position = Vector3.zero + Vector3.up;
        rb.velocity = Vector3.zero;
    }
}