using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesCombo : MonoBehaviour
{
    public float leftBoundarie;
    public float rightBoundarie;

    public float speedMove;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(rightBoundarie + 5f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x - speedMove * Time.deltaTime, transform.position.y, transform.position.z);

        if (transform.position.x < leftBoundarie - 5f) Destroy(gameObject);
    }
}
