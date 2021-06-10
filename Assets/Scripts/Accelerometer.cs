using UnityEngine;

public class Accelerometer : MonoBehaviour
{
    public Vector2 velocity
    {
        get; private set;
    }

    public Vector2 impactForce
    {
        get; private set;
    }

    private Rigidbody2D _rigidbody;

    [SerializeField]
    private float impactMultiplier = 1;

    private void Start()
    {
        velocity = Vector2.zero;
        impactForce = Vector2.zero;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        velocity = _rigidbody.velocity;
        impactForce = new Vector2(impactMultiplier * _rigidbody.mass * velocity.x, impactMultiplier * _rigidbody.mass * velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Impact(collision);
    }

    private void Impact(Collision2D collision)
    {
        Accelerometer otherAccel = collision.gameObject.GetComponent<Accelerometer>();
        impactForce = otherAccel.impactForce;
    }
}
