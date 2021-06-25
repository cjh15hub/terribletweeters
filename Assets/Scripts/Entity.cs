using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour, IDamagable
{
    [SerializeField]
    public float initialHealth = 100;
    public float health
    {
        get => _health;
        private set => _health = value;
    }

    [SerializeField] [InspectorReadonly]
    private float _health = 100;

    [SerializeField]
    private float destroyDelay = 1.5f;

    public bool isDead { get => health <= 0 && _dead; }
    private bool _dead = false;

    // component references
    private Accelerometer accelerometer;

    private void Start()
    {
        health = initialHealth;
        accelerometer = GetComponent<Accelerometer>();
        if (accelerometer == null)
        {
            accelerometer = gameObject.AddComponent(typeof(Accelerometer)) as Accelerometer;
        }
    }

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (health <= 0 && !_dead)
        {
            Die();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead)
        {
            return;
        }
        else
        {
            Entity otherEntity = collision.gameObject.GetComponent<Entity>();
            Accelerometer otherAccelerometer = collision.gameObject.GetComponent<Accelerometer>();
            if (otherAccelerometer)
            {
                TakeDamage(otherAccelerometer.impactForce.magnitude);
                if(otherEntity)
                {
                    otherEntity.TakeDamage(accelerometer.impactForce.magnitude);
                }
            }
            TakeDamage(accelerometer.impactForce.magnitude);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void Die()
    {
        if (!_dead)
        {
            health = 0;
            OnDeath();
            _dead = true;
        }
    }

    protected virtual void OnDeath()
    {
        DeactivateAfterDelay(destroyDelay);
    }

    protected void DeactivateAfterDelay(float delay)
    {
        StartCoroutine(Deactivate(delay));
    }
    private IEnumerator Deactivate(float delay)
    {
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
    }
}
