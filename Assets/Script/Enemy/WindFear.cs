using UnityEngine;

public class WindFear : MonoBehaviour
{
    public float speed;
    public float leftBoundarie;

    public GameObject blindEffect;

    private GameObject playerPos;

    public float xAxes;
    public float yAxes;

    public GameObject effect;

    public GameObject deathEffect;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player");

        transform.position = new Vector3(leftBoundarie + 2f, playerPos.transform.position.y, playerPos.transform.position.z);

        effect.transform.parent = null;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, playerPos.transform.position.y + 1.5f, transform.position.z);
        effect.transform.position = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManaging gameManager = FindObjectOfType<GameManaging>();
            gameManager.bombSpamReady = true;

            GameObject blind = Instantiate(blindEffect, playerPos.transform.position, Quaternion.identity);
            blind.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
            blind.transform.localPosition = new Vector3(xAxes, yAxes, 0f);
            blind.transform.localScale = new Vector3(8.841208f, 8.841208f, 0f);
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
    }
}
