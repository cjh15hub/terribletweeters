using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[SelectionBase]
public class Monster : MonoBehaviour
{
    [SerializeField]
    public Sprite deadSprite;

    [SerializeField]
    public float ouchForce = 19.5f;

    [SerializeField]
    public float startingHealth = 40;

    [SerializeField] [InspectorReadonly]
    private float _newtonHealth = 0;


    // component references
    private new Rigidbody2D rigidbody;
    private Accelerometer accelerometer;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem poofParticleSystem;

    public bool isDead { get => _newtonHealth <= 0; }
    private readonly float almostZero = 0.0001f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        accelerometer = GetComponent<Accelerometer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        var particleSystems = GetComponentsInChildren<ParticleSystem>();
        poofParticleSystem = particleSystems.FirstOrDefault(p => p.gameObject.name == "PoofParticleSystem") ?? particleSystems[0];
    }

    private void Start()
    {
        _newtonHealth = startingHealth;
    }

    private void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionHandler(collision);
    }

    private void CollisionHandler(Collision2D collision)
    {
        if (isDead) return;

        // if collision is with a bird, always consider it a kill
        Bird bird = collision.gameObject.GetComponent<Bird>();
        if (bird)
        {
            Die();
        }

        // monsters are squishy and won't hurt each other
        Monster anotherMonster = collision.gameObject.GetComponent<Monster>();
        if (anotherMonster) return;

        // if something falls onto the monster
        //if (collision.contacts[0].normal.y < -0.5)

        float impactDamage = 0;

        if(collision.gameObject.tag == "BluntObject")
        {
            var otherAccelerometer = collision.gameObject.GetComponent<Accelerometer>();

            var relativeVelocity = accelerometer.velocity - otherAccelerometer.velocity;

            if (accelerometer.velocity.magnitude > almostZero && otherAccelerometer.velocity.magnitude > almostZero)
            {
                // both objects moving
                impactDamage = CalculateImpactDamage((rigidbody.mass + collision.rigidbody.mass), relativeVelocity, ouchForce);
            }
            else if(accelerometer.velocity.magnitude > almostZero)
            {
                // monster is moving
                impactDamage = CalculateImpactDamage(rigidbody.mass, relativeVelocity, ouchForce);
            }
            else if(otherAccelerometer.velocity.magnitude > almostZero)
            {
                // other is moving
                impactDamage = CalculateImpactDamage(collision.rigidbody.mass, relativeVelocity, ouchForce);
            }
        }

        if (impactDamage > 0) ApplyDamage(impactDamage);
    }

    private float CalculateImpactDamage(float mass, Vector2 relativeVelocity, float minForce)
    {
        var exertedForce = 0.5f * mass * Mathf.Pow(relativeVelocity.magnitude, 2);

        if (exertedForce > minForce) return exertedForce;
        else return 0;
    }

    private void ApplyDamage(float newtonDamage)
    {
        _newtonHealth -= newtonDamage;
        if (_newtonHealth <= 0) Die();
    }

    private void Die()
    {
        // prevent permanent negative numbers
        _newtonHealth = 0;

        spriteRenderer.sprite = deadSprite;
        poofParticleSystem.Play();

        StartCoroutine(DeactivateAfterDelay());
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);
    }

}
