using UnityEngine;

public class BlockadeBeast : MonoBehaviour
{
    [Header("Blockade")]
    public GameObject[] blockade;
    public GameObject blockadePos;

    [Header("Movement")]
    public float rightBoundarie;
    public float leftBoundarie;
    public float yAxis;
    public float speed;

    private bool alreadySpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(rightBoundarie, yAxis, 0f);

        int randomNumber = Random.Range(0, blockade.Length * 100);
        for (int j = 1; j <= blockade.Length; j++)
        {
            if (randomNumber <= j * 100f && alreadySpawn == false)
            {
                GameObject block = Instantiate(blockade[j - 1], blockadePos.transform.position, Quaternion.identity);
                block.transform.SetParent(transform);
                alreadySpawn = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-speed * Time.deltaTime, 0f, 0f);
        if (transform.position.x < leftBoundarie)
        {
            Destroy(gameObject);
        }
    }
}
