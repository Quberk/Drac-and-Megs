using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public float leftBoundaries;
    public float rightBoundaries;
    public float upBoundaries;
    public float bottomBoundaries;

    public float waitingTime;
    [HideInInspector]
    public float waitingCounter = 0f;

    [HideInInspector]
    public bool allowMove;
    public float speed;
    private Vector3 target;
    [HideInInspector]
    public bool moving;
    // Start is called before the first frame update
    void Start()
    {
        //Menambah Kecepatan Sesuai dengan Wave
        GameManaging m = FindObjectOfType<GameManaging>();
        //speed += m.waveNum * 0.5f;

        //PlayerStat player = FindObjectOfType<PlayerStat>();
        //if (player.rageUsed == true) speed = 8f;

        allowMove = true;

        //Mengganti angka dari OrderInLayer secara Random
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

        int angkaRandom = Random.Range(0, 100);
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].sortingOrder *= angkaRandom;
        }

        waitingCounter = waitingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (allowMove == true)
        {
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);

            if (transform.position.x <= leftBoundaries - 5f)
            {
                Destroy(gameObject);
            }

            //Sistem Lama
            /* 
            waitingCounter += Time.deltaTime;
            if (waitingCounter >= waitingTime && readyToDie == false)
            {
                moving = true;
                float xAxis = Random.Range(leftBoundaries + 8f, rightBoundaries);
                float yAxis = Random.Range(bottomBoundaries, upBoundaries);

                speed = Random.Range(speed, speed + 1f);

                target = new Vector3(xAxis, yAxis, 0f);
            }

            if (moving == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                waitingCounter = 0f;

                if (transform.position == target && readyToDie == true)
                {
                    Destroy(gameObject);
                    moving = false;
                }

                if (transform.position == target)
                {
                    moving = false;
                }
            }

            //Durasi Waktu hidup
            lifeCounter += Time.deltaTime;
            if (lifeCounter >= lifeTime)
            {
                readyToDie = true;
                moving = true;
                lifeCounter = 0f;

                float y = Random.Range(0, 100);
                float xAxis = 0f;
                if (y <= 50)
                {
                    xAxis = leftBoundaries - 8f;
                }

                else
                {
                    xAxis = rightBoundaries + 8f;
                }

                speed += 8f;
                float yAxis = Random.Range(bottomBoundaries, upBoundaries);

                target = new Vector3(xAxis, yAxis, transform.position.z);
            }*/

            //Mengganti Tag Enemy jika telah masuk Layar
            if (transform.position.x >= leftBoundaries && transform.position.x <= rightBoundaries) transform.gameObject.tag = "Enemy";
            else transform.gameObject.tag = "Untagged";
        }
    }

}
