using UnityEngine;

public class Accelerometer : MonoBehaviour
{
    private Vector2 _velocity;
    public Vector2 velocity
    {
        get { return _velocity; }
    }

    private Vector2 _impactForce;
    public Vector2 impactForce
    {
        get { return _impactForce; }
    }


    private Rigidbody2D _rigidbody;

    [SerializeField]
    private float impactMultiplier = 1;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _velocity = _rigidbody.velocity;
        _impactForce.x = impactMultiplier * _rigidbody.mass * _velocity.x;
        _impactForce.y = impactMultiplier * _rigidbody.mass * _velocity.y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Impact(collision);
    }

    private void Impact(Collision2D collision)
    {
        Accelerometer otherAccel = collision.gameObject.GetComponent<Accelerometer>();
        _impactForce = otherAccel.impactForce;
    }
}
