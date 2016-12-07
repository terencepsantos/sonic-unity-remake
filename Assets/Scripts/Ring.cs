﻿using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour 
{
    public SpriteRenderer RingSpriteRenderer;
    public ParticleSystem RingParticleSystem;
    public Collider2D RingCollider;
    public Collider2D RingBounceCollider;
    public Rigidbody2D RingRigidbody2D;
    public Animator RingAnimator;


    void Awake()
    {
        gameObject.tag = "Ring";
        RingCollider.enabled = false;
        RingBounceCollider.enabled = false;
        Invoke("TurnOnColliders", 0.5f);
    }


    private void TurnOnColliders()
    {
        RingBounceCollider.enabled = true;
        RingCollider.enabled = true;
    }


    public void MoveScatteredRings(float[] randomForces)
    {
        RingRigidbody2D.isKinematic = false;
        RingRigidbody2D.AddForce(new Vector2(randomForces[0] * randomForces[2], randomForces[1]), ForceMode2D.Impulse);

        RingAnimator.ResetTrigger("FadeOut");
        RingAnimator.SetTrigger("FadeOut");

        StartCoroutine(RingBlink());
    }


    public void DestroyRing()
    {
        Destroy(gameObject);
    }


    private IEnumerator RingBlink()
    {
        while (RingSpriteRenderer.color.a > 0)
        {
            RingSpriteRenderer.enabled = !RingSpriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
        }
    }


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            Debug.Log("Ma oe");
            RingSpriteRenderer.enabled = false;
            RingCollider.enabled = false;
            RingParticleSystem.Emit(1);
            Destroy(gameObject, RingParticleSystem.duration + 0.1f);
        }
    }
}
