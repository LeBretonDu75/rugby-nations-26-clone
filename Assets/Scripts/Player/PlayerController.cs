using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public int teamId;
    public bool isHuman = false;
    public bool hasBall = false;
    public PlayerStats stats;
    public Transform ballSocket;
    
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator anim;
    public float tackleCooldown = 0f;
    public float currentStamina = 100f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = stats.speed;
    }

    void Update()
    {
        if (tackleCooldown > 0) tackleCooldown -= Time.deltaTime;
        UpdateAnimator();
    }

    public void MoveTo(Vector3 pos, bool sprint = false)
    {
        if (tackleCooldown > 0) return;
        float spd = sprint && currentStamina > 0 ? stats.sprintSpeed : stats.speed;
        agent.speed = spd;
        agent.SetDestination(pos);
        if (sprint) currentStamina -= Time.deltaTime * 15f;
        else currentStamina = Mathf.Min(100, currentStamina + Time.deltaTime * 10f);
    }

    public void Stop() { agent.ResetPath(); }

    void UpdateAnimator()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude);
        anim.SetBool("HasBall", hasBall);
        anim.SetBool("Tackled", tackleCooldown > 0);
    }

    public void GetTackled()
    {
        tackleCooldown = 2.5f;
        hasBall = false;
        anim.SetTrigger("Tackle");
        agent.ResetPath();
    }
}