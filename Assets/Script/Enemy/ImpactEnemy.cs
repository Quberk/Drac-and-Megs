using UnityEngine;

public class ImpactEnemy : MonoBehaviour
{
    public float damage;
    public float speed;

    [Header("Boundaries")]
    public float leftBoundaries;
    public float rightBoundaries;
    public float upBoundaries;
    public float downBoundaries;

    public float waitingTime;
    private float waitingCounter = 0f;

    public GameObject dangerSignal;
    public GameObject dangerSound;

    public GameObject dieEffect;
    public GameObject particleEffect;
    public GameObject wineEffect;

    [HideInInspector]
    public bool punishment;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (punishment == false)
        {
            float randX = Random.Range(rightBoundaries, rightBoundaries + 5f);
            //float randY = Random.Range(downBoundaries, upBoundaries);

            Vector3 pos = new Vector3(randX, player.transform.position.y + 1.5f, transform.position.z);
            transform.position = pos;

            Instantiate(dangerSignal, new Vector3(leftBoundaries + 8f, player.transform.position.y + 1.5f, 0f), Quaternion.identity);
        }
        else Instantiate(dangerSignal, new Vector3(leftBoundaries + 8f, transform.position.y, 0f), Quaternion.identity);

        Instantiate(dangerSound, transform.position, Quaternion.identity);
        particleEffect.transform.parent = null;
        wineEffect.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        waitingCounter += Time.deltaTime;

        if (waitingCounter >= waitingTime) Impacting();

        if (transform.position.x < leftBoundaries * 10f) Destroy(gameObject);

        particleEffect.transform.position = transform.position;
        wineEffect.transform.position = transform.position;
    }

    //Fungsi Menabrak
    void Impacting()
    {
        transform.Translate(-speed * Time.deltaTime, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().HealthConsumption(damage, false);
            Instantiate(dieEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
