using UnityEngine;

public class ParalayangEating : MonoBehaviour
{
    [HideInInspector]
    public bool finishedAttacking = true;
    [SerializeField]
    private Animator eatingAnim;

    [HideInInspector]
    public Vector3 enemyPosition;
    public GameObject[] enemyPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && finishedAttacking == true)
        {
            collision.gameObject.transform.position = enemyPosition;
            finishedAttacking = false;
            eatingAnim.SetTrigger("isAttacking");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.transform.position = enemyPosition;
            if (finishedAttacking == true) {
                if (collision.gameObject.GetComponent<EnemyHealth>()) collision.gameObject.GetComponent<EnemyHealth>().Damaging(500f);
                else collision.gameObject.GetComponent<EnemyFormation>().Damaging(500f);
            } 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject != null)
        {
            collision.gameObject.transform.position = enemyPosition;
        }
    }

}
