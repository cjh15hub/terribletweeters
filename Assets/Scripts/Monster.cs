using System.Linq;
using UnityEngine;


[SelectionBase]
public class Monster : Entity
{
    [SerializeField]
    public Sprite deadSprite;

    // component references
    /*private new Rigidbody2D rigidbody;*/
    private SpriteRenderer spriteRenderer;
    private ParticleSystem poofParticleSystem;

    private void Awake()
    {
        /*rigidbody = GetComponent<Rigidbody2D>();*/
        spriteRenderer = GetComponent<SpriteRenderer>();
        var particleSystems = GetComponentsInChildren<ParticleSystem>();
        poofParticleSystem = particleSystems.FirstOrDefault(p => p.gameObject.name == "PoofParticleSystem") ?? particleSystems[0];
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead)
        {
            return;
        }
        else
        {
            base.OnCollisionEnter2D(collision);
        }
        
    }

    public override void DeathRoutine()
    {
        spriteRenderer.sprite = deadSprite;
        poofParticleSystem.Play();
    }
}
