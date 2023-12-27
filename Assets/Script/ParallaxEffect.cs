using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private Vector2 startPos;
    public float parallexEffect;
    public float boundariesPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (transform.position.x <= boundariesPos + 3.25f) transform.position = startPos;

        transform.Translate(new Vector2(-parallexEffect, 0f) * Time.deltaTime);
    }
}
