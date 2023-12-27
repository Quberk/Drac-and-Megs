using UnityEngine;

public class FireBalloonEnemyDetector : MonoBehaviour
{
    [SerializeField]
    private Animator fireBaloonAnim;
    [SerializeField]
    private ParticleSystem[] fireAttackFx;
    private int enemyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < fireAttackFx.Length; i++)
        {
            fireAttackFx[i].Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCount == 0)
        {
            for (int i = 0; i < fireAttackFx.Length; i++)
            {
                fireAttackFx[i].Stop();
                fireBaloonAnim.SetBool("isAttacking", false);
            }
        }

        else
        {
            for (int i = 0; i < fireAttackFx.Length; i++)
            {
                fireAttackFx[i].Play();
                fireBaloonAnim.SetBool("isAttacking", true);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyCount--;
        }
    }
}
