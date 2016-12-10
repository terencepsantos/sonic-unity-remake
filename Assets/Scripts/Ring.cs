using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour 
{
    public SpriteRenderer RingSpriteRenderer;
    public ParticleSystem RingParticleSystem;
    public Collider2D RingCollider;
    public Collider2D RingBounceCollider;
    public Rigidbody2D RingRigidbody2D;
    public Animator RingAnimator;

    private bool isBlinking = false;
    private bool isCollected = false;


    void Awake()
    {
        gameObject.tag = "Ring";
        RingCollider.enabled = false;
        RingBounceCollider.enabled = false;
        Invoke("TurnOnColliders", 1.0f);
    }


    private void TurnOnColliders()
    {
        if (!isCollected)
        {
            RingBounceCollider.enabled = true;
            RingCollider.enabled = true;
        }
    }


    public void ScatteredRings(float[] randomForces)
    {
        gameObject.tag = "RingRespawn";

        RingRigidbody2D.isKinematic = false;
        RingRigidbody2D.AddForce(new Vector2(randomForces[0] * randomForces[2], randomForces[1]), ForceMode2D.Impulse);

        RingAnimator.ResetTrigger("FadeOut");
        RingAnimator.SetTrigger("FadeOut");

        StartCoroutine(RingBlink());
    }


    private void EnemyRing()
    {
        TurnOnColliders();
    }


    //Called by Animation Event
    public void DestroyRing()
    {
        Destroy(gameObject);
    }


    private IEnumerator RingBlink()
    {
        isBlinking = true;

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
            isCollected = true;

            if (isBlinking)
            {
                Destroy(gameObject);
            }
            else
            {
                RingSpriteRenderer.enabled = false;
                RingCollider.enabled = false;
                RingBounceCollider.enabled = false;
                RingParticleSystem.Emit(1);
                Destroy(gameObject, RingParticleSystem.duration + 0.1f);
            }
        }
    }
}
