using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] float distanceToPlayer;

    protected override void Awake()
    {
        base.Awake();
        playerRef = FindObjectOfType<PlayerTopdownControl>();
    }

    private void Start()
    {
        destiny = playerRef.transform;
        canMove = true;
    }

    void Update()
    {
        if (Vector2.Distance(destiny.position, transform.position) <= distanceToPlayer)
        {
            canMove = false;
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

        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, destiny.position, moveSpeed * Time.deltaTime);

            Vector2 direction = transform.position - destiny.position;
            direction.Normalize();
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

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthController>()?.TakeDamage(damage);
            isTouching = true;
            canMove = false;
            StartCoroutine(ReloadDamage());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopAllCoroutines();
            canMove = true;
            isTouching = false;
        }
    }*/
}
