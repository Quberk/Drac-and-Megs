using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float leftBoundaries;
    public float rightBoundaries;
    public float upBoundaries;
    public float bottomBoundaries;

    public float waitingTime;
    private float waitingCounter = 0f;

    [HideInInspector]
    public bool allowMove;
    public float speed;
    private Vector3 target;
    [HideInInspector]
    public bool moving;
    // Start is called before the first frame update
    void Start()
    {
        allowMove = true;

        float y = Random.Range(2, 3);

        int rand = Random.Range(1, 2);
        float xAxis = Random.Range(leftBoundaries, rightBoundaries);
        float yAxis = 0f;

        switch (rand)
        {
            case 1:
                yAxis = upBoundaries;
                break;
            case 2:
                yAxis = bottomBoundaries;
                break;
        }

        Vector3 pos = new Vector3(xAxis, yAxis + y, transform.position.z);
        transform.position = pos;

        //Mengganti angka dari OrderInLayer secara Random
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

        int angkaRandom = Random.Range(0, 100);
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].sortingOrder *= angkaRandom;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (allowMove == true)
        {
            waitingCounter += Time.deltaTime;
            if (waitingCounter >= waitingTime)
            {
                moving = true;
                float xAxis = Random.Range(leftBoundaries, rightBoundaries);
                float yAxis = Random.Range(bottomBoundaries, upBoundaries);

                speed = Random.Range(speed, speed + 1f);

                target = new Vector3(xAxis, yAxis, 0f);
            }

            if (moving == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                waitingCounter = 0f;
                if (transform.position == target)
                {
                    moving = false;
                }
            }

            //Mengganti Tag Enemy jika telah masuk Layar
            if (transform.position.y >= bottomBoundaries && transform.position.y <= upBoundaries) transform.gameObject.tag = "Enemy";
            else if (transform.position.y <= bottomBoundaries || transform.position.y >= upBoundaries || transform.position.x >= rightBoundaries || transform.position.x <= leftBoundaries)
            {
                waitingCounter = waitingTime;
                moving = false;
            }
        }
    }
}
