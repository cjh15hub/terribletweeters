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
    public float newtonHealth = 40;

    // component references
    private new Rigidbody2D rigidbody;
    private Accelerometer accelerometer;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem poofParticleSystem;

    public bool isDead
    {
        get { return _isDead; }
        private set { _isDead = value; }
    }
    private bool _isDead;


    private Vector2 velocity;

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
        isDead = false;
    }

    private void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(ShouldDieFromCollision(collision))
        {
            Die();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (ShouldDieFromCollision(collision))
        {
            Die();
        }
    }

    private bool ShouldDieFromCollision(Collision2D collision)
    {
        if (isDead) return false;

        // if collision is with a bird, always consider it a kill
        Bird bird = collision.gameObject.GetComponent<Bird>();
        if (bird)
        {
            newtonHealth = 0;
            return true;
        }

        // monsters are squishy and won't hurt each other
        Monster anotherMonster = collision.gameObject.GetComponent<Monster>();
        if (anotherMonster) return false;

        // if something falls onto the monster
        //if (collision.contacts[0].normal.y < -0.5)
        //{
        //    // return true;
        //}
        //else 
        if(collision.gameObject.tag == "BluntObject")
        {
            const float almostZero = 0.0001f;
            var otherAccelerometer = collision.gameObject.GetComponent<Accelerometer>();

            var relativeVelocity = accelerometer.velocity - otherAccelerometer.velocity;

            if (accelerometer.velocity.magnitude > almostZero && otherAccelerometer.velocity.magnitude > almostZero)
            {
                // both objects moving
                newtonHealth -= CalculateDamage((rigidbody.mass + collision.rigidbody.mass), relativeVelocity, ouchForce);
            }
            else if(accelerometer.velocity.magnitude > almostZero)
            {
                // monster is moving
                newtonHealth -= CalculateDamage(rigidbody.mass, relativeVelocity, ouchForce);
            }
            else if(otherAccelerometer.velocity.magnitude > almostZero)
            {
                // other is moving
                newtonHealth -= CalculateDamage(collision.rigidbody.mass, relativeVelocity, ouchForce);
            }

        }

        return newtonHealth <= 0;
    }

    private float CalculateDamage(float mass, Vector2 relativeVelocity, float minForce)
    {
        var exertedForce = 0.5f * mass * Mathf.Pow(relativeVelocity.magnitude, 2);

        if (exertedForce > minForce) return exertedForce;
        else return 0;
    }

    private void Die()
    {
        isDead = true;
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
