using UnityEngine;

public class ShootingWizard : MonoBehaviour
{
    public float speed;
    public float startYPos;
    public float distance;

    [HideInInspector]
    public int myNum;

    public float leftBoundarie;
    public float rightBoundarie;

    public GameObject puke;
    public GameObject warningEffect;
    public GameObject warningSound;
    public GameObject burstSound;

    public float lifeTime;
    [HideInInspector]
    public float lifeCounter = 0f;

    public float pukeTime;
    private float pukeCounter = 0f;

    private bool die = false;

    private Collider2D myColl;

    private bool alreadyInsWarning = false;
    private bool alreadyInsBurst = false;


    // Start is called before the first frame update
    void Start()
    {
        myColl = GetComponent<Collider2D>();
        myColl.enabled = false;

        puke.SetActive(false);
        warningEffect.SetActive(false);

        float xPos = leftBoundarie + 2f;
        float yPos = startYPos - (distance * (myNum - 1f));

        transform.position = new Vector3(xPos, yPos, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Sudah siap mati
        if (transform.position.x >= rightBoundarie + 8f && die == true)
        {
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            Destroy(gameObject);
        }
        //Sedang menyerang dengan Puke
        else if (transform.position.x >= rightBoundarie && die == false)
        {

            lifeCounter += Time.deltaTime;
            pukeCounter += Time.deltaTime;

            if (lifeCounter >= lifeTime) { 
                die = true;

                //Mengaktifkan Efek Puke
                myColl.enabled = false;
                puke.SetActive(false);
            }

            else if (pukeCounter >= pukeTime + 0.2f)
            {
                myColl.enabled = true;
                if (alreadyInsWarning == false)
                {
                    Instantiate(burstSound, transform.position, Quaternion.identity);
                    alreadyInsWarning = true;
                }
            }

            else if (pukeCounter >= pukeTime)
            {
                puke.SetActive(true);
                warningEffect.SetActive(false);
            }

            else
            {
                warningEffect.SetActive(true);
                if (alreadyInsBurst == false)
                {
                    alreadyInsBurst = true;
                    Instantiate(warningSound, transform.position, Quaternion.identity);
                }
                
            }
        }
        //Bergerak ke Kanan
        else
        {
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().HealthConsumption(100f, false);
        }
    }
}
