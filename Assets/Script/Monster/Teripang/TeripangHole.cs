using UnityEngine;

public class TeripangHole : MonoBehaviour
{
    [SerializeField]
    private GameObject centerOfHole;
    [SerializeField]
    private float speedToHole;
    [SerializeField]
    private ParticleSystem suckingFx;

    private int amountOfEnemyInsideAttackRange;

    // Start is called before the first frame update
    void Start()
    {
        suckingFx.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (amountOfEnemyInsideAttackRange <= 0)
        {
            amountOfEnemyInsideAttackRange = 0;
            suckingFx.Stop();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            suckingFx.Play();
            if (collision.GetComponent<EnemyHealth>()) collision.gameObject.transform.position = 
                                                    Vector3.MoveTowards(collision.gameObject.transform.position,
                                                                          centerOfHole.transform.position,
                                                                          speedToHole * Time.deltaTime);
            else collision.GetComponent<EnemyFormation>().Damaging(500f);

            if (collision.gameObject.transform.position.x <= centerOfHole.transform.position.x && collision.GetComponent<EnemyHealth>()) {
               collision.GetComponent<EnemyHealth>().Damaging(100f);
            } 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            amountOfEnemyInsideAttackRange += 1;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            amountOfEnemyInsideAttackRange -= 1;
        }
    }
}
