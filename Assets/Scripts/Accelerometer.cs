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

    private Vector2 _impactForce;
    public Vector2 impactForce
    {
        get { return _impactForce; }
        set { _impactForce = value; }
    }

    public const float almostZero = 0.0001f;


    private new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        this.velocity = rigidbody.velocity;
        _impactForce = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.tag == "BluntObject")
        {
            var otherVelocity = collision.gameObject.GetComponent<Accelerometer>()?.velocity;
            this.velocity += otherVelocity ?? Vector2.zero;
        }*/

        if (collision.gameObject.tag == "BluntObject")
        {
            Accelerometer otherAccelerometer = collision.gameObject.GetComponent<Accelerometer>();
            //Vector2 relativeVelocity = velocity - otherAccelerometer.velocity;

            if (velocity.magnitude > almostZero && otherAccelerometer.velocity.magnitude > almostZero)
            {
                // both objects moving
                _impactForce = CalculateExertedForce((rigidbody.mass + collision.rigidbody.mass), otherAccelerometer);
            }
            else if (velocity.magnitude > almostZero)
            {
                // self is moving
                _impactForce =  CalculateExertedForce(rigidbody.mass, otherAccelerometer);
                otherAccelerometer.impactForce = _impactForce; 
            }
            else if (otherAccelerometer.velocity.magnitude > almostZero)
            {
                // other is moving
                _impactForce = CalculateExertedForce(collision.rigidbody.mass, otherAccelerometer) + otherAccelerometer.impactForce;
            }

        }
    }

    private Vector2 CalculateExertedForce(float mass, Accelerometer otherAccelerometer)
    {
        Vector2 relativeVelocity = velocity - otherAccelerometer.velocity;
        float angle = Vector2.Angle(rigidbody.position, otherAccelerometer.rigidbody.position);
        float x = mass * relativeVelocity.x * Mathf.Sin(angle);
        float y = mass * relativeVelocity.y * Mathf.Cos(angle);
        return new Vector2(x, y);
    }
}
