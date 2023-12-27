using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    private float healthCounter = 0f;

    public float rageCost;

    public GameObject deathEffect;

    private PlayerStat playerStat;

    public GameObject energyParticle;
    public int energyAmount;

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
            playerStat.RageConsumption(-rageCost);
            for (int i = 0; i < energyAmount; i++)
            {
                Instantiate(energyParticle, transform.position, Quaternion.identity);
            }
            for (int i = 0; i < rageAmount; i++)
            {
                Instantiate(rageParticle, transform.position, Quaternion.identity);
            }
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            myGame.score += score;

            comboManager.ComboAdder(1);
        }
    }

    public void Damaging(float damage)
    {
        healthCounter += damage;
    }
}
