using UnityEngine;

public class EnemyFormation : MonoBehaviour
{
    [Header("Cart")]
    public bool isCart;
    public GameObject brokenEffect;
    public float leftBoundaries;
    public float health = 100f;
    private float healthCounter = 0f;
    [HideInInspector]
    public float speed = 3f;
    private GameObject[] passanger = new GameObject[10];

    [Header("Special Formation")]
    public bool specialFormation = false;
    public bool theFirst = false;
    public float xPosToDie;
    private float copySpeed;

    private bool alreadySpawned = false;

    public GameObject[] EnemyPos;

    public float rightBoundarie;
    public float upBoundarie;
    public float downBoundarie;

    public GameObject[] enemy;

    private bool enemySpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        float y = Random.Range(downBoundarie, upBoundarie);

        if (isCart == true) {
            GameManaging m = FindObjectOfType<GameManaging>(); 
            //speed += m.waveNum * 0.5f;

            PlayerStat player = FindObjectOfType<PlayerStat>();
            //if (player.rageUsed == true) speed = 7f;
            transform.position = new Vector3(rightBoundarie, y, 0f);
        }
        else if (specialFormation == true)
        {
            GameManaging m = FindObjectOfType<GameManaging>();
            m.specialFormation = true;

            transform.SetParent(null);

            if (theFirst == true) transform.position = new Vector3(rightBoundarie, transform.position.y, 0f);
        }

        else transform.position = new Vector3(rightBoundarie, y, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (alreadySpawned == false)
        {
            for (int i = 0; i < EnemyPos.Length; i++)
            {
                enemySpawned = false;
                int b = Random.Range(0, enemy.Length * 100);
                for (int j = 1; j <= enemy.Length; j++)
                {
                    if (b <= j * 100f && enemySpawned == false)
                    {
                        passanger[i] = Instantiate(enemy[j - 1], EnemyPos[i].transform.position, Quaternion.Euler(0f, 180f, 0f));
                        PlayerStat player = FindObjectOfType<PlayerStat>();
                        //if (player.rageUsed == true) passanger[i].GetComponent<EnemyMovement>().speed = 8f;

                        //Apabila merupakan kendaraan massal maka semua enemy akan dijadikan sebagai Child, dan juga menonaktifkan pergerakan dan juga Collider
                        if (isCart == true)
                        {
                            passanger[i].transform.SetParent(transform);
                            passanger[i].GetComponent<Collider2D>().enabled = false;
                            passanger[i].GetComponent<EnemyMovement>().enabled = false;
                        }

                        enemySpawned = true;
                    }
                }
                alreadySpawned = true;
                
            }
        }

        //Membuat semua penumpang tetap tidak bertag Enemy
        if (isCart == true)
        {
            for (int k = 0; k < EnemyPos.Length; k++)
            {
                passanger[k].tag = "Untagged";
            }
        }

        //Jika bukan termasuk Kendaraan Massal maka akan segera hancur
        if (specialFormation == true && theFirst == true)
        {
            if (passanger[0]) copySpeed = passanger[0].GetComponent<EnemyMovement>().speed;
            transform.position = new Vector3(transform.position.x - copySpeed * Time.deltaTime, 0f,0f);
            if (transform.position.x <= xPosToDie)
            {
                GameManaging m = FindObjectOfType<GameManaging>();
                m.specialFormation = false;
                Destroy(gameObject);
            }
        }

        else if (isCart == false) Destroy(gameObject);

        else {
            Moving();
            Die();
        }
    }

    //Semua fungsi dibawah merupakan bagian dari fungsi khusus untuk Cart

    //Hanya untuk kendaraan massal
    public void Damaging(float damage)
    {
        healthCounter += damage;
    }

    //Fungsi untuk pergerakan Cart
    void Moving()
    {
        transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);

        if (transform.position.x <= leftBoundaries - 5f)
        {
            Destroy(gameObject);
        }
    }

    public void Die()
    {
        if (isCart == true && healthCounter >= health)
        {
            Destroy(gameObject);
            Instantiate(brokenEffect, transform.position, Quaternion.identity);
            for (int i = 0; i < passanger.Length; i++)
            {
                passanger[i].GetComponent<EnemyHealth>().Die();
            }
        }
    }

}
