using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthController>()?.TakeDamage(damage);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<HealthController>()?.TakeDamage(damage);
        }
    }
}
