using UnityEngine;

public class ChaosParticle : MonoBehaviour
{
    public bool rage;
    public bool health;

    private Animator healthAnim;
    private Animator rageAnim;

    public float cost;

    public float speed;
    private Rigidbody2D myRigid;

    public float movingTime;
    private float movingCounter = 0;

    public float offset;

    private GameObject player;

    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();

        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        direction = new Vector3(x, y, 0f) * speed * 10f;

        //Mencari semua Status Bar Animator
        //healthAnim = GameObject.Find("Health_Bar").GetComponent<Animator>();
        //rageAnim = GameObject.Find("Rage_Bar").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movingCounter += Time.deltaTime;
        player = GameObject.Find("CenterPoint");
        if (movingCounter >= movingTime) {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
        }

        else myRigid.AddForce(direction);

        //Bergerak mengarah Player
        float targetPosX = player.transform.position.x - transform.position.x;
        float targetPosY = player.transform.position.y - transform.position.y;
        float angle = Mathf.Atan2(targetPosY, targetPosX) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + offset));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Rage
        if (collision.gameObject.CompareTag("Player") && rage == true)
        {
            collision.gameObject.GetComponent<PlayerStat>().RageConsumption(-cost);
            //rageAnim.SetTrigger("GetParticle");
            Destroy(gameObject);
        }
        //Health
        if (collision.gameObject.CompareTag("Player") && health == true)
        {
            collision.gameObject.GetComponent<PlayerStat>().HealthConsumption(-cost, true);
            healthAnim.SetTrigger("GetParticle");
            Destroy(gameObject);
        }
    }
}
