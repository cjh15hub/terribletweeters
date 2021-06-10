using System.Linq;
using UnityEngine;


[SelectionBase]
public class Monster : Entity
{
    [SerializeField]
    public Sprite deadSprite;

    // component references
    private SpriteRenderer spriteRenderer;
    private ParticleSystem poofParticleSystem;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        var particleSystems = GetComponentsInChildren<ParticleSystem>();
        poofParticleSystem = particleSystems.FirstOrDefault(p => p.gameObject.name == "PoofParticleSystem") ?? particleSystems[0];
    }

    public override void DeathRoutine()
    {
        spriteRenderer.sprite = deadSprite;
        poofParticleSystem.Play();
    }
}
