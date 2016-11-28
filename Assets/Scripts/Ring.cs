using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour 
{
    private SpriteRenderer ringSpriteRenderer;
    private ParticleSystem ringParticleSystem;
    private Collider2D ringCollider;


    void Awake()
    {
        gameObject.tag = "Ring";

        ringSpriteRenderer.GetComponent<SpriteRenderer>();
        ringParticleSystem.GetComponent<ParticleSystem>();
        ringCollider.GetComponent<Collider2D>();
    }


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            ringSpriteRenderer.enabled = false;
            ringParticleSystem.Emit(1);
            Destroy(gameObject, ringParticleSystem.duration + 0.1f);
        }
    }
}
