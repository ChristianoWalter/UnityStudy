using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : HealthController
{
    [Header("Basic Stats")]
    [SerializeField] float moveSpeed;
    [SerializeField] float timeToDamage;
    bool isOnRange;
    bool canMove;
    PlayerTopdownControl playerRef;

    [Header("Combat variables")]
    [SerializeField] float damage;
    [SerializeField] float rangedDistance;
    [SerializeField] float lifeToChange;
    [SerializeField] GameObject projectile;
    bool isRangedAttack;

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
            if (!isOnRange)
            {
                isOnRange = true;
                if (isRangedAttack)
                {
                    StartCoroutine(ReloadDamage());
                }
                else
                {
                    playerRef.GetComponent<HealthController>().TakeDamage(damage);
                    StartCoroutine(ReloadDamage());
                }
            }

        }
        else
        {
            if (isOnRange)
            {
                isOnRange = false;
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
        if (isOnRange)
        {
            if (isRangedAttack)
            {
                Vector2 direction = ((Vector2)playerRef.transform.position - rb.position).normalized;
                GameObject _projectile = Instantiate(projectile, transform.position, Quaternion.identity);
                _projectile.GetComponent<ProjectileScript>().direction = direction;
            }
            else
            {
                playerRef.GetComponent<HealthController>().TakeDamage(damage);
            }
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

    protected override void DamageEffect()
    {
        if (currentHealth <= lifeToChange)
        {
            isRangedAttack = true;
            distanceToPlayer = rangedDistance;
        }
    }
}
