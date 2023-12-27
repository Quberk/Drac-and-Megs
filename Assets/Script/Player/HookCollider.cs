using UnityEngine;

public class HookCollider : MonoBehaviour
{
    public Hook hook;
    public GameObject hookEffect;

    [HideInInspector]
    public GameObject enemyTransform;
    [HideInInspector]
    public EnemyHealth enemyHealth;

    [HideInInspector]
    public SnitchHealth snitchHealth;

    /*private PlayerStat playerStat;
    private Gun myGun;*/
    // Start is called before the first frame update

    private void Start()
    {
        /*playerStat = FindObjectOfType<PlayerStat>();
        myGun = FindObjectOfType<Gun>();*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy") && hook.isAttacking == true)
        {
            enemyTransform = collision.gameObject;
            if (enemyTransform.GetComponent<EnemyHealth>()) { 
                enemyHealth = enemyTransform.GetComponent<EnemyHealth>();
                hook.imCatching = true;
                hook.isAttacking = false;
            }
            else enemyTransform.GetComponent<EnemyFormation>().Damaging(1000f);

            if (enemyTransform.GetComponent<EnemyShoot>()) { 
                enemyTransform.GetComponent<EnemyShoot>().allowShoot = false;
                enemyTransform.GetComponent<EnemyShoot>().fireRateCounter = -2f;
            }
            //enemyTransform.GetComponent<Collider2D>().enabled = false;

            Instantiate(hookEffect, transform.position, Quaternion.identity);
        }

        else if (collision.transform.CompareTag("Snitch"))
        {
            enemyTransform = collision.gameObject;
            snitchHealth = enemyTransform.GetComponent<SnitchHealth>();
            SnitchMovement snitchMovement = enemyTransform.GetComponent<SnitchMovement>();
            snitchMovement.waitingCounter = 0f;
            snitchHealth.Damaging(100f);

            Instantiate(hookEffect, transform.position, Quaternion.identity);
        }

        else if (collision.transform.CompareTag("Obstacle"))
        {
            GameObject baby = collision.gameObject;
            BlockadeObstacle blockade = baby.GetComponent<BlockadeObstacle>();
            blockade.Damaging(100f);

            Instantiate(hookEffect, transform.position, Quaternion.identity);
        }
    }
}
