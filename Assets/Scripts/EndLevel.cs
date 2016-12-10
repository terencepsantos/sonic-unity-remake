using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public Animator EndLevelAnimator;

    void Awake()
    {
        gameObject.tag = "EndLevel";
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            EndLevelAnimator.SetTrigger("EndLevel");
        }
    }
}
