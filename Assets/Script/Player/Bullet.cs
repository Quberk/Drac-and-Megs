using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    [Header("Raycast")]
    public float rad;

    [Header("Bullet Type")]
    public bool automaticGun;
    public bool chargingGun;
    public bool rocketLauncher;
    public bool shotGun;

    public GameObject hitEffect;

    [Header("Charging Gun Stat")]
    public GameObject effect;
    public GameObject blastArea;
    [HideInInspector]
    public bool readyToShoot;

    [Header("Rocket")]
    public GameObject effectRocket;
    [HideInInspector]
    public GameObject target;

    [Header("Shot Gun")]
    public int numberOfBulletShotGun = 0;

    [Header("Default Stat")]
    public float speed;
    public GameObject blastSound;
    public float lifeTime;
    private float lifeCounter;
    private Rigidbody2D myRIgid;

    // Start is called before the first frame update
    void Start()
    {
        lifeCounter = lifeTime;
        myRIgid = GetComponent<Rigidbody2D>();
        if (automaticGun == true) myRIgid.AddForce(new Vector2(speed * Time.deltaTime, 0f));
        else if (rocketLauncher == true)
        {
            effectRocket.transform.SetParent(null);
            effect.SetActive(true);
        }
        else if (chargingGun == true) effect.SetActive(false);
        else if (shotGun == true)
        {
            float angle = 9f;

            angle -= 4.5f * numberOfBulletShotGun;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (automaticGun == true)
        {
            AutomaticBullet();
            //Raycasting();
        }

        else if (chargingGun == true)
        {
            ChargingBullet();
        }

        else if (shotGun == true)
        {
            ShotGunBullet();
        }

        else
        {
            RocketProjectile();
        }

        //Raycasting();

    }

    private void FixedUpdate()
    {
        Physics2D.IgnoreLayerCollision(9, 8);
    }

    void AutomaticBullet()
    {
        lifeCounter -= Time.deltaTime;

        if (lifeCounter <= 0)
        {
            Destroy(gameObject);
        }

        myRIgid.AddForce(new Vector2(speed * Time.deltaTime, 0f));
    }

    void ShotGunBullet()
    {
        lifeCounter -= Time.deltaTime;

        if (lifeCounter <= 0)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void ChargingBullet()
    {
        if (readyToShoot == true)
        {
            effect.SetActive(true);

            lifeCounter -= Time.deltaTime;

            if (lifeCounter <= 0)
            {
                Destroy(gameObject);
            }

            myRIgid.AddForce(new Vector2(speed * Time.deltaTime, 0f));
        }
    }

    //Raycast yang mendeteksi Enemy
    void Raycasting()
    {
        RaycastHit2D myRayCast = Physics2D.Raycast(transform.position, transform.right, rad);
        //Raycast Automatic Gun hanya Automatic Gun yang dapat menghancurkan Projectile Musuh
        if (myRayCast.collider != null)
        {
            if (myRayCast.collider.CompareTag("Enemy"))
            {
                myRayCast.collider.GetComponent<EnemyHealth>().Damaging(damage);
                Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            
            else if (myRayCast.collider.CompareTag("Bullet"))
            {
                Destroy(myRayCast.collider.gameObject);
                Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            else if (myRayCast.collider.CompareTag("Snitch"))
            {
                myRayCast.collider.GetComponent<SnitchHealth>().Damaging(damage);
                Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            else if (myRayCast.collider.CompareTag("Shield"))
            {
                myRayCast.collider.GetComponent<EnemyHealth>().Damaging(damage);
                Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

        }
    }

    void RocketProjectile()
    {
        lifeCounter -= Time.deltaTime;

        if (lifeCounter <= 0)
        {
            Destroy(gameObject);
            Destroy(effectRocket);
        }

        if (target)
        {
            Vector3 targetPos = target.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            //Merotasikan Ke arah Target
            Vector3 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        else Destroy(gameObject);

        
        effectRocket.transform.position = transform.position;

        readyToShoot = true;
    }

    //Untuk Charging Gun dan Rocket Gun
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Charging Gun atau Rudal Besar 
        if (chargingGun == true && readyToShoot == true)
        {

            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Shield") || collision.gameObject.CompareTag("Obstacle") 
                || collision.gameObject.CompareTag("WitchKing"))
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
                Instantiate(blastArea, transform.position, Quaternion.identity);
                Instantiate(blastSound, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

        }

        //Automatic Gun
        else if (automaticGun == true || shotGun == true)
        {

            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Shield"))
            {
                if (collision.gameObject.GetComponent<EnemyHealth>()) collision.gameObject.GetComponent<EnemyHealth>().Damaging(damage);
                else collision.gameObject.GetComponent<EnemyFormation>().Damaging(damage);
                Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            else if (collision.gameObject.CompareTag("Obstacle"))
            {
                collision.gameObject.GetComponent<BlockadeObstacle>().Damaging(damage);
                Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            else if (collision.gameObject.CompareTag("WitchKing"))
            {
                collision.gameObject.GetComponent<WitchKing>().Damaging(damage);
                Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

        }
    }

    //Untuk Rudal kecil
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rocketLauncher == true && readyToShoot == true)
        {
            if (collision.gameObject.CompareTag("Enemy") && collision.gameObject == target)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
                Instantiate(blastSound, transform.position, Quaternion.identity);
                if (collision.gameObject.GetComponent<EnemyHealth>()) collision.gameObject.GetComponent<EnemyHealth>().Damaging(damage);
                else collision.gameObject.GetComponent<EnemyFormation>().Damaging(damage);

                Destroy(effectRocket);
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Charging Gun atau Rocket
        if (chargingGun == true && readyToShoot == true)
        {

            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Shield") || collision.gameObject.CompareTag("WitchKing") || collision.gameObject.CompareTag("Obstacle"))
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
                Instantiate(blastArea, transform.position, Quaternion.identity);
                Instantiate(blastSound, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

}
