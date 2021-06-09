using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerometer : MonoBehaviour
{
    public Vector2 velocity
    {
        get { return _velocity; }
        private set { _velocity = value; }
    }
    private Vector2 _velocity;


    private new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        this.velocity = rigidbody.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BluntObject")
        {
            var otherVelocity = collision.gameObject.GetComponent<Accelerometer>()?.velocity;
            this.velocity += otherVelocity ?? Vector2.zero;
        }
    }
}
