using UnityEngine;

public class SnitchMovement : MonoBehaviour
{

    public float leftBoundaries;
    public float rightBoundaries;

    public float waitingTime;
    [HideInInspector]
    public float waitingCounter;

    

    public float lifeTime;
    private float lifeCounter = 0f;
    private bool dying = false;

    [Header("Movement Pattern")]
    public float xSpeed;
    private float ySpeed;
    public float yMax;
    private bool goDown;
    private bool goUp;
    private float firstYpos;

    [HideInInspector]
    public bool moving;
    void Start()
    {
        //Menambah Kecepatan Sesuai dengan Wave
        GameManaging m = FindObjectOfType<GameManaging>();
        xSpeed += m.waveNum * 0.5f;

        float xAxis = Random.Range(rightBoundaries, rightBoundaries + 5f);
        float yAxis = 0f;

        Vector3 pos = new Vector3(xAxis, yAxis, transform.position.z);
        transform.position = pos;

        goDown = true;

        firstYpos = transform.position.y;

        waitingCounter = waitingTime;

        ySpeed = 0f;
    }

    // Update is called once per frame
    void Update()
    {


        if (transform.position.y >= firstYpos + yMax)
        {
            goDown = true;
            goUp = false;
            ySpeed = 0f;
        }

        else if (transform.position.y <= firstYpos - yMax)
        {
            goUp = true;
            goDown = false;
            ySpeed = 0f;
        }

        if (goDown == true)
        {
            if (transform.position.y <= (firstYpos - yMax) / 2) ySpeed -= Time.deltaTime * 5f;
            else ySpeed += Time.deltaTime * 10f;
            transform.position = new Vector3(transform.position.x - xSpeed * Time.deltaTime, transform.position.y - ySpeed * Time.deltaTime, transform.position.z);
            

        }

        else if (goUp == true)
        {
            if (transform.position.y >= (firstYpos + yMax) / 2) ySpeed -= Time.deltaTime * 5f;
            else ySpeed += Time.deltaTime * 10f;
            transform.position = new Vector3(transform.position.x - xSpeed * Time.deltaTime, transform.position.y + ySpeed * Time.deltaTime, transform.position.z);
        }

        if (transform.position.x <= leftBoundaries)
        {
            Destroy(gameObject);
        }

        if (transform.position.x <= rightBoundaries - 2f) transform.tag = "Snitch";

    }

}
