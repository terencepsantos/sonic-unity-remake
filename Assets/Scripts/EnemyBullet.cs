using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    private float bulletMovingRate = 1.5f;

    private float secondsTilDestroy = 4;

    void Awake()
    {
        gameObject.tag = "EnemyBullet";
    }

    void Start()
    {
        Invoke("DestroyBullet", secondsTilDestroy);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, bulletMovingRate * Time.deltaTime, 0), Space.Self);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

}