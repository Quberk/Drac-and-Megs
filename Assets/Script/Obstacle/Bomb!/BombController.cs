using UnityEngine;

public class BombController : MonoBehaviour
{
    public float speed;

    public float rightBoundarie;
    public float leftBoundarie;
    public float upBoundarie;
    public float downBoundarie;

    [HideInInspector]
    public bool trigger = false;

    [HideInInspector]
    public bool explode = false;

    [HideInInspector]
    public bool deactivate = false;

    [HideInInspector]
    public bool obstacle = true;

    private bool killing = false;
    private float killingCounter = 0f;

    public Animator myAnim;

    public GameObject effect;

    private Animator cameraAnim;

    // Start is called before the first frame update
    void Start()
    {
        cameraAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();

        GameManaging g = FindObjectOfType<GameManaging>();
        //if (obstacle == true) speed += g.waveNum;

        float xPos = rightBoundarie + 3f;
        float yPos = Random.Range(downBoundarie, upBoundarie);

        transform.position = new Vector3(xPos, yPos, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger == true)
        {
            effect.SetActive(false);
            myAnim.SetTrigger("Explode");
            trigger = false;
        }

        if (explode == true)
        {
            killingCounter += Time.deltaTime;
            if (killingCounter >= 0.1f)
            {
                killing = true;
                explode = false;
                killingCounter = 0f;
            }
        }

        transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);

        if (transform.position.x <= leftBoundarie)
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        if (killing == true)
        {
            killingCounter += Time.deltaTime;
            if (killingCounter >= 0.1f)
            {
                cameraAnim.SetTrigger("hit");
                Destroy(gameObject);
            }      
        }
    }

    public void Deactivate()
    {
        if (obstacle == true) {
            deactivate = true;
            effect.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && killing == true)
        {
            collision.GetComponent<PlayerStat>().HealthConsumption(100f, false);
            cameraAnim.SetTrigger("hit");
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Enemy") && killing == true)
        {
            if (collision.GetComponent<EnemyHealth>()) collision.GetComponent<EnemyHealth>().Damaging(100f);
            else collision.GetComponent<EnemyFormation>().Damaging(100f);

            cameraAnim.SetTrigger("hit");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && killing == true)
        {
            collision.GetComponent<PlayerStat>().HealthConsumption(100f, false);
            cameraAnim.SetTrigger("hit");
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Enemy") && killing == true)
        {
            if (collision.GetComponent<EnemyHealth>()) collision.GetComponent<EnemyHealth>().Damaging(100f);
            else collision.GetComponent<EnemyFormation>().Damaging(100f);

            cameraAnim.SetTrigger("hit");
            Destroy(gameObject);
        }
    }

}
