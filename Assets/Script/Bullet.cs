using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Type")]
    public bool automaticGun;
    public bool chargingGun;

    [Header("Charging Gun Stat")]
    [HideInInspector]
    public bool readyToShoot;
    public GameObject effect;

    public float speed;
    private Rigidbody2D myRIgid;
    public float lifeTime;
    private float lifeCounter;


    // Start is called before the first frame update
    void Start()
    {
        lifeCounter = lifeTime;

        myRIgid = GetComponent<Rigidbody2D>();
        if (automaticGun == true) myRIgid.AddForce(new Vector2(speed * Time.deltaTime, 0f));

        effect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (automaticGun == true)
        {
            AutomaticBullet();
        }

        else
        {
            ChargingBullet();
        }
        
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
}
