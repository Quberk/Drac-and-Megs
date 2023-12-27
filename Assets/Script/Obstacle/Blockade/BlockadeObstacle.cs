using UnityEngine;

public class BlockadeObstacle : MonoBehaviour
{
    public float damage;

    [Header("Destroyable Blockade")]
    public bool destroyable = false;
    public float health;
    public GameObject destroyedEffect;
    private float healthCounter = 0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (healthCounter >= health && destroyable == true)
        {
            Instantiate(destroyedEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().HealthConsumption(damage, false);
        }
    }

    public void Damaging(float damage)
    {
        healthCounter += damage;
    }
}
