using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTopdownControl : HealthController
{
    [SerializeField] float speed;
    Vector2 direction;
    [SerializeField] Rigidbody2D rb;

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
    }

    public void MovementAction(InputAction.CallbackContext value)
    {
        direction = value.ReadValue<Vector2>();
    }
}
