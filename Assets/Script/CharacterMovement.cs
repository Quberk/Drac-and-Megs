using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public ParticleSystem batEffect1;
    public ParticleSystem batEffect2;
    //public ParticleSystem flyEffect;

    private Rigidbody2D myRigid;
    public float speed;

    private float screenWidth;

    public Animator bodyAnim;

    [SerializeField]
    private float currentYPos;
    private float lastYPos;


    // Start is called before the first frame update
    void Start()
    {
        screenWidth = Screen.width;

        myRigid = GetComponent<Rigidbody2D>();
        batEffect1.Stop();
        batEffect2.Stop();
        //flyEffect.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;

        while (i < Input.touchCount)
        {
            if (Input.GetTouch (i).position.x < screenWidth /2)
            {
                Flying(1.0f);
            }
            i++;
        }

        YPositionCheck();
    }

    public void Flying(float touchInput)
    {
        myRigid.AddForce(new Vector2(0f, touchInput * speed * Time.deltaTime));
        batEffect1.Emit(1);
        batEffect2.Emit(1);
        //flyEffect.Emit(2);
    }

    //Mengecek Posisi Y Player apakah sedang jatuh atau tidak
    void YPositionCheck()
    {
        currentYPos = transform.position.y;

        if (currentYPos < lastYPos)
        {
            bodyAnim.SetBool("isFalling", true);
        }

        else if (currentYPos > lastYPos)
        {
            bodyAnim.SetBool("isFalling", false);
        }

        lastYPos = transform.position.y;

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boundaries"))
        {
            bodyAnim.SetBool("isFalling", false);
        }
    }
}
