using UnityEngine;

public class BombDetector : MonoBehaviour
{
    public BombController myBomb;
    public GameObject beepSound;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (myBomb.obstacle == false) GetComponent<BoxCollider2D>().size = new Vector2(45f, GetComponent<BoxCollider2D>().size.y); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && myBomb.deactivate == false)
        {
            myBomb.trigger = true;
            Instantiate(beepSound, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
