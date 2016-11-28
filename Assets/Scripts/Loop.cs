using UnityEngine;
using System.Collections;

public class Loop : MonoBehaviour 
{
    public Collider2D[] CollisionFlips;

    void OnTriggerEnter2D(Collider2D coll)
    {
        foreach (Collider2D cflip in CollisionFlips)
        {
            cflip.enabled = !cflip.enabled;
        }
    }
}
