using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    private float healthCounter = 0f;

    public float lifeTime = 10f;
    private float lifeCounter = 0f;

    public GameObject deathEffect;
    public GameObject deadSound;

    private PlayerStat playerStat;

    public GameObject rageParticle;
    public int rageAmount;

    private GameManaging myGame;

    private ComboManager comboManager;

    public float score;

   

    // Start is called before the first frame update
    void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();
        myGame = FindObjectOfType<GameManaging>();
        comboManager = FindObjectOfType<ComboManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (healthCounter >= health)
        {
            for (int i = 0; i < rageAmount; i++)
            {
                Instantiate(rageParticle, transform.position, Quaternion.identity);
            }
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Instantiate(deadSound, transform.position, Quaternion.identity);
            Destroy(gameObject);
            //myGame.score += score;

            comboManager.ComboAdder(1);
        }

        lifeCounter += Time.deltaTime;
        if (lifeCounter >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void Damaging(float damage)
    {
        healthCounter += damage;
    }

    public void Die()
    {
        for (int i = 0; i < rageAmount; i++)
        {
            Instantiate(rageParticle, transform.position, Quaternion.identity);
        }
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Instantiate(deadSound, transform.position, Quaternion.identity);
        Destroy(gameObject);

        comboManager.ComboAdder(1);
    }
}
