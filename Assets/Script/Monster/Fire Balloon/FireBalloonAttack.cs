using UnityEngine;

public class FireBalloonAttack : MonoBehaviour
{
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<EnemyHealth>()) collision.gameObject.GetComponent<EnemyHealth>().Damaging(200f);
            else collision.gameObject.GetComponent<EnemyFormation>().Damaging(200f);
        }
    }
}
