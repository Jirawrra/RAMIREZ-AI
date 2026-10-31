using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{

    [Header("References")]
    public Transform player;


    [Header("Waypoints (Patrol)")]
    public Transform[] waypoints;

    [Header("Detection")]
    public float chaseRadius = 10f;
    public float attackRadius = 1.5f;

    [Header("Field of View")]
    [Range(0f, 1f)]
    public float dotThreshold = 0.5f;    // 1.0 = dead ahead, 0.0 = 90°, -1.0 = 360°
    public LayerMask wallLayer;          // Assign your wall/tilemap layer


    [Header("Speeds")]
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 5.5f;

    [Header("Visuals")]
    public Renderer bodyRenderer;
    public Color patrolColor = Color.red;
    public Color chaseColor = Color.yellow;

    // ──────────────────────────────────────────────
    //  State Machine
    // ──────────────────────────────────────────────
    public enum EnemyState { Patrol, Chase }
    public EnemyState currentState { get; private set; } = EnemyState.Patrol;

    // ──────────────────────────────────────────────
    //  Private
    // ──────────────────────────────────────────────
    NavMeshAgent agent;
    int waypointIndex = 0;
    Vector2 facingDir = Vector2.right;   // Tracks which way the enemy is facing


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        EnterState(EnemyState.Patrol);
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol: UpdatePatrol(); break;
            case EnemyState.Chase: UpdateChase(); break;
        }

        // Update facing direction from movement
        Vector2 vel = new Vector2(agent.velocity.x, agent.velocity.y);
        if (vel.magnitude > 0.01f)
            facingDir = vel.normalized;
    }


    void UpdatePatrol()
    {
        if (player != null && CanSeePlayer())
        {
            EnterState(EnemyState.Chase);
            return;
        }

        if (waypoints == null || waypoints.Length == 0) return;

        agent.SetDestination(waypoints[waypointIndex].position);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
    }


    void UpdateChase()
    {
        // Return to patrol if player is out of range AND no longer visible
        if (player != null && !CanSeePlayer() && DistanceToPlayer() > chaseRadius * 1.2f)
        {
            EnterState(EnemyState.Patrol);
            return;
        }

        if (player != null)
            agent.SetDestination(player.position);

        if (player != null && DistanceToPlayer() <= attackRadius)
        {
            Debug.Log("Player caught by enemy!");
            // player.GetComponent<PlayerHealth>()?.TakeDamage(1);
        }
    }

    void EnterState(EnemyState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case EnemyState.Patrol:
                agent.speed = patrolSpeed;
                SetColor(patrolColor);
                break;

            case EnemyState.Chase:
                agent.speed = chaseSpeed;
                SetColor(chaseColor);
                break;
        }
    }

    // ──────────────────────────────────────────────
    //  Line of Sight
    // ──────────────────────────────────────────────

    bool CanSeePlayer()
    {
        if (player == null) return false;

        float dist = DistanceToPlayer();
        if (dist > chaseRadius) return false;

        // --- Dot product FOV check ---
        Vector2 toPlayer = ((Vector2)player.position - (Vector2)transform.position).normalized;
        float dot = Vector2.Dot(facingDir, toPlayer);
        if (dot < dotThreshold) return false;

        // --- Wall raycast check ---
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            toPlayer,
            dist,
            wallLayer
        );

        // No wall hit = player is visible
        return hit.collider == null;
    }

    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    float DistanceToPlayer()
    {
        if (player == null) return Mathf.Infinity;
        return Vector2.Distance(transform.position, player.position);
    }

    void SetColor(Color c)
    {
        if (bodyRenderer != null)
            bodyRenderer.material.color = c;
    }

    void OnDrawGizmosSelected()
    {
        // Chase radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        // Attack radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        // FOV cone — visible in editor and at runtime
        {
            float angle = Mathf.Acos(Mathf.Clamp(dotThreshold, -1f, 1f)) * Mathf.Rad2Deg;
            // In editor use transform.right as default facing; in play use live facingDir
            Vector3 forward = Application.isPlaying ? (Vector3)facingDir : transform.right;
            Vector3 leftEdge = Quaternion.Euler(0, 0, angle) * forward * chaseRadius;
            Vector3 rightEdge = Quaternion.Euler(0, 0, -angle) * forward * chaseRadius;
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, leftEdge);
            Gizmos.DrawRay(transform.position, rightEdge);
            // Arc between the two edges
            int segments = 20;
            Vector3 prev = transform.position + leftEdge;
            for (int i = 1; i <= segments; i++)
            {
                float t = (float)i / segments;
                float a = Mathf.Lerp(angle, -angle, t);
                Vector3 next = transform.position + Quaternion.Euler(0, 0, a) * forward * chaseRadius;
                Gizmos.DrawLine(prev, next);
                prev = next;
            }
        }
    }
}