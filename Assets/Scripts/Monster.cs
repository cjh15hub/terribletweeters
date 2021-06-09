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
    public double ouchForce = 4f;

    [SerializeField]
    public double newtonHealth = 40;

    // component references
    private new Rigidbody2D rigidbody;
    private Accelerometer accelerometer;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem poofParticleSystem;

    public bool isDead
    {
        get { return newtonHealth <= 0; }
    }


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
        
    }

    private void FixedUpdate()
    {
        if (newtonHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TakeDamage(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //TakeDamage(collision);
    }

    private void TakeDamage(Collision2D collision)
    {

        // monsters are squishy and won't hurt each other
        if (isDead || collision.gameObject.GetComponent<Monster>())
        {
            return;
        }

        // if collision is with a bird, always consider it a kill
        Bird bird = collision.gameObject.GetComponent<Bird>();
        if (bird)
        {
            newtonHealth = 0;
        }
        else if (collision.gameObject.tag == "BluntObject")
        {
            Accelerometer otherAccelerometer = collision.gameObject.GetComponent<Accelerometer>();

            if (otherAccelerometer.impactForce.magnitude >= ouchForce)
            {

                newtonHealth -= otherAccelerometer.impactForce.magnitude;
            }
        }
    }

    private void Die()
    {
        newtonHealth = 0;
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
