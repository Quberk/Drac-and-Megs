using UnityEngine;

public class Blast : MonoBehaviour
{
    public float damage;

    [Header("Rocket Launcher")]
    public bool rocketLauncher;

    public GameObject rocketSpawner;

    public float lifeTime = 0.1f;
    private float lifeCounter = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeCounter += Time.deltaTime;

        if (lifeCounter >= lifeTime) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shield"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().Damaging(damage);
            //spawnProjectile = true;
            Destroy(gameObject);

            //Jika Peluru Rocket maka memanggil Rocket Spawner
            if (rocketLauncher == true) { Instantiate(rocketSpawner, transform.position, Quaternion.identity);
                rocketLauncher = false;
            }
        }

        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<EnemyHealth>()) collision.gameObject.GetComponent<EnemyHealth>().Damaging(damage);
            else collision.gameObject.GetComponent<EnemyFormation>().Damaging(damage);
            //spawnProjectile = true;

            Destroy(gameObject);

            //Jika Peluru Rocket maka memanggil Rocket Spawner
            if (rocketLauncher == true)
            {
                Instantiate(rocketSpawner, transform.position, Quaternion.identity);
                rocketLauncher = false;
            }
        }

        else if (collision.gameObject.CompareTag("Snitch"))
        {
            collision.gameObject.GetComponent<SnitchHealth>().Damaging(damage);
            //spawnProjectile = true;

            Destroy(gameObject);

            //Jika Peluru Rocket maka memanggil Rocket Spawner
            if (rocketLauncher == true)
            {
                Instantiate(rocketSpawner, transform.position, Quaternion.identity);
                rocketLauncher = false;
            }
        }

        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            collision.gameObject.GetComponent<BlockadeObstacle>().Damaging(damage);
            Destroy(gameObject);
            if (rocketLauncher == true)
            {
                Instantiate(rocketSpawner, transform.position, Quaternion.identity);
                rocketLauncher = false;
            }
        }

        else if (collision.gameObject.CompareTag("WitchKing"))
        {
            collision.gameObject.GetComponent<WitchKing>().Damaging(damage);
            Destroy(gameObject);
        }

    }
}
