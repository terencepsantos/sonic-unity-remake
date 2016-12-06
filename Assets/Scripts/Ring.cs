using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour 
{
    public SpriteRenderer RingSpriteRenderer;
    public ParticleSystem RingParticleSystem;
    public Collider2D RingCollider;


    void Awake()
    {
        gameObject.tag = "Ring";
    }


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            RingSpriteRenderer.enabled = false;
            RingCollider.enabled = false;
            RingParticleSystem.Emit(1);
            Destroy(gameObject, RingParticleSystem.duration + 0.1f);
        }
    }
}
