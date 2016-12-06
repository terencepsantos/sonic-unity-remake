using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour 
{
    public SpriteRenderer RingSpriteRenderer;
    public ParticleSystem RingParticleSystem;
    public Collider2D RingCollider;
    public Rigidbody2D RingRigidbody2D;


    void Awake()
    {
        gameObject.tag = "Ring";
    }


    public void InstantiateScatteredRings(float[] randomForces)
    {
        RingRigidbody2D.isKinematic = false;
        RingRigidbody2D.AddForce(new Vector2(randomForces[0] * randomForces[2], randomForces[1] * randomForces[2]));
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
