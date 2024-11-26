using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [Header("Projectile base stats")]
    [SerializeField] float speed;
    [SerializeField] float damage;
    bool isMoving = true;

    [HideInInspector] public Vector2 direction;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject impactEffect;
    [SerializeField] Animator anim;

    private void Start()
    {
        Vector2 _localScale = transform.localScale;
        _localScale.x *= direction.x;
        transform.localScale = _localScale;
    }

    void Update()
    {
        //adicionando velocidade ao projétil
        if (isMoving) rb.velocity = direction * speed;
        else rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy") return;

        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<HealthController>().TakeDamage(damage);
        }

        //efeito do impacto do tiro
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
