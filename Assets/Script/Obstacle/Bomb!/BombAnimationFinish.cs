using UnityEngine;

public class BombAnimationFinish : MonoBehaviour
{
    public GameObject explodingEffect;
    public GameObject explodingEffect1;
    public BombController bomb;

    public GameObject explosionSound;

    private Animator myAnim;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (bomb.deactivate == true)
        {
            myAnim.SetTrigger("Deactivated");
        }
    }

    public void Explode()
    {
        bomb.explode = true;
        GameObject x1 = Instantiate(explodingEffect, transform.position, Quaternion.identity);
        GameObject x2 = Instantiate(explodingEffect1, transform.position, Quaternion.identity);
        Instantiate(explosionSound, transform.position, Quaternion.identity);

        x1.transform.position = bomb.transform.position;
        x2.transform.position = bomb.transform.position;

        Destroy(gameObject);
    }
}
