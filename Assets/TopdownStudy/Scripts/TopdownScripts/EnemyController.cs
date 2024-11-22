using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : HealthController
{
    [Header("Basic Stats")]
    [SerializeField] float moveSpeed;
    [SerializeField] float damage;
    [SerializeField] float timeToDamage;
    bool isTouching;
    bool canMove;
    PlayerTopdownControl playerRef;

    [Header("Movement variables")]
    Transform destiny;
    public float nextWaypointDistance;
    int currentWayPoint;
    bool reachedEndOfPath;
    [SerializeField] float distanceToPlayer;
    Path path;
    Seeker seeker;

    [SerializeField] Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        playerRef = FindObjectOfType<PlayerTopdownControl>();
    }

    private void Start()
    {
        destiny = playerRef.transform;
        canMove = true;

        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, destiny.position, OnPathComplete);
    }

    void Update()
    {
        if (Vector2.Distance(destiny.position, transform.position) <= distanceToPlayer)
        {
            canMove = false;
            rb.velocity = Vector2.zero;
            if (!isTouching)
            {
                isTouching = true;
                playerRef.GetComponent<HealthController>().TakeDamage(damage);
                StartCoroutine(ReloadDamage());
            }

        }
        else
        {
            if (isTouching)
            {
                isTouching = false;
                StopAllCoroutines();
            }
            canMove = true;
        }
    }

    void FixedUpdate()
    {
        if (path == null || !canMove) return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;

        rb.velocity = direction * moveSpeed;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }
    }

    IEnumerator ReloadDamage()
    {
        yield return new WaitForSeconds(timeToDamage);
        if (isTouching)
        {
            playerRef.GetComponent<HealthController>().TakeDamage(damage);
            StartCoroutine(ReloadDamage());
        }
    }


    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    
}
