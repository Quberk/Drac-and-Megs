using UnityEngine;

public class LayerCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Physics2D.IgnoreLayerCollision(10, 10);
        Physics2D.IgnoreLayerCollision(10, 8);
        Physics2D.IgnoreLayerCollision(10, 11);
        Physics2D.IgnoreLayerCollision(10, 12);

        //Particle Stat
        Physics2D.IgnoreLayerCollision(13, 0);
        Physics2D.IgnoreLayerCollision(13, 8);
        Physics2D.IgnoreLayerCollision(13, 9);
        Physics2D.IgnoreLayerCollision(13, 10);
        Physics2D.IgnoreLayerCollision(13, 11);

        Physics2D.IgnoreLayerCollision(12, 0);
        Physics2D.IgnoreLayerCollision(12, 9);

        Physics2D.IgnoreLayerCollision(9, 16);
        Physics2D.IgnoreLayerCollision(9, 9);
        //Obstacle
        // Physics2D.IgnoreLayerCollision(15, 10);
    }
}
