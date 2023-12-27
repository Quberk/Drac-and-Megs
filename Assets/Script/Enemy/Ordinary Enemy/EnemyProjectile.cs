using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody2D myRigid;
    public float speed;

    public float damage;

    public GameObject hitEffect;

    public float lifeTime;
    private float lifeCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
        myRigid.AddForce(-transform.right * speed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        lifeCounter += Time.deltaTime;
        myRigid.AddForce(-transform.right * speed * Time.deltaTime);

        if (lifeCounter >= lifeTime) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerStat>().HealthConsumption(damage, false);
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
