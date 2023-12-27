using UnityEngine;

public class SelfDestruction : MonoBehaviour
{
    public float lifeTime;
    private float lifeCounter;
    // Start is called before the first frame update
    void Start()
    {
        lifeCounter = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        lifeCounter -= Time.deltaTime;

        if (lifeCounter <= 0)
        {
            Destroy(gameObject);
        }
    }
}
