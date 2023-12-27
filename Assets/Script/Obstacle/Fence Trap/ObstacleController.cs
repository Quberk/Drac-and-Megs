using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    //Sebagai Penentu apakah Obstacle termasuk desain atau tidak
    public bool obstacleCombo;

    public float upBoundarie;
    public float downBoundarie;
    public float leftBoundarie;
    public float rightBoundarie;

    public float verticalBoundarie;

    public float speedMove;

    public float damage;

    public bool rotatingObs;

    public GameObject laserEffect;
    private Collider2D myCollider;

    public float coolDownTime;
    private float coolDownCounter = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (obstacleCombo == false)
        {
            float yAxis = Random.Range(downBoundarie, upBoundarie);
            float random = Random.Range(90f, 180f);

            if (yAxis < verticalBoundarie || rotatingObs == true) random = 0f;

            transform.position = new Vector3(rightBoundarie + 3f, yAxis, 0f);
            Quaternion rot = Quaternion.Euler(0f, 0f, random);

            transform.rotation = rot;
        }

        myCollider = GetComponent<Collider2D>();

        //Menambah Kecepatan Sesuai dengan Wave
        GameManaging m = FindObjectOfType<GameManaging>();
        //speedMove += m.waveNum;
    }

    // Update is called once per frame
    void Update()
    {
        if (obstacleCombo == false)
        {
            transform.position = new Vector3(transform.position.x - speedMove * Time.deltaTime, transform.position.y, transform.position.z);

            if (transform.position.x < leftBoundarie - 5f) Destroy(gameObject);
        }

        if (rotatingObs == true)
        {
            transform.Rotate (0f, 0f, 50f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().HealthConsumption(damage, false);
        }
    }

    public void Deactivate()
    {

        laserEffect.SetActive(false);
        if (myCollider) myCollider.enabled = false;
        coolDownCounter = 0f;
    }
}
