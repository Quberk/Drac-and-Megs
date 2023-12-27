using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public ParticleSystem batEffect1;
    public ParticleSystem batEffect2;
    public GameObject batSound;
    private float batSoundCounter = 0;

    private bool tapFlying ;
    private bool downFlying ;
    private bool upsideDownFlying = true;

    [Header("Tap Flying")]
    [SerializeField]
    private ParticleSystem flyingFx;
    [SerializeField]
    private ParticleSystem fartFx;
    [SerializeField]
    private float tapCoolDownTime;
    private float tapCoolDownCounter = 0f;
    [HideInInspector]
    public bool clickedBtn = false;

    [Header("Upside Down Flying")]
    [SerializeField]
    private float upsideDownCoolDownTime;
    private float upsideDownCoolDownCounter = 0f;
    [SerializeField]
    private Animator baloonAnim;
    [SerializeField]
    private ParticleSystem fireFx;

    [Header("Down Flying")]
    [SerializeField]
    private Animator wingAnim;
    [SerializeField]
    private ParticleSystem[] upWindFx;
    [SerializeField]
    private ParticleSystem[] downWindFx;
    private bool flyingDown = false;


    //public ParticleSystem flyEffect;

    private Rigidbody2D myRigid;
    public float speed;

    private float screenWidth;

    public Animator bodyAnim;

    [SerializeField]
    private float currentYPos;
    private float lastYPos;

    private bool touchingBoundaries;


    // Start is called before the first frame update
    void Start()
    {
        screenWidth = Screen.width;

        myRigid = GetComponent<Rigidbody2D>();
        batEffect1.Stop();
        batEffect2.Stop();
        //flyEffect.Stop();

        if (downFlying == true)
        {
            for (int j = 0; j < upWindFx.Length; j++)
            {
                downWindFx[j].Stop();
            }

            for (int j = 0; j < upWindFx.Length; j++)
            {
                upWindFx[j].Stop();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (tapFlying == true)
        {
            TapFlying();
            return;
        }
        if (downFlying == true)
        {
            DownFlying();
            return;
        }
        if (upsideDownFlying == true)
        {
            UpsideDownFlying();
            return;
        }

        NormalFlying();
    }

    public void Flying(float touchInput)
    {
        myRigid.AddForce(new Vector2(0f, touchInput * speed * Time.deltaTime));
        
        //batEffect2.Emit(1);
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

    void NormalFlying()
    {
        clickedBtn = false;
        myRigid.gravityScale = Mathf.Abs(myRigid.gravityScale);
        int i = 0;

        while (i < Input.touchCount)
        {
            if (Input.GetTouch(i).position.x < screenWidth / 2)
            {
                Flying(1.0f);
                batEffect1.Emit(1);
                batSoundCounter += Time.deltaTime;
                if (batSoundCounter >= 0.3f)
                {
                    Instantiate(batSound, transform.position, Quaternion.identity);
                    batSoundCounter = 0f;
                }

            }

            i++;
        }

        if (touchingBoundaries == true)
        {
            batEffect1.Emit(1);
            //batEffect2.Emit(1);
        }

        YPositionCheck();
    }

    void TapFlying()
    {
        myRigid.gravityScale = Mathf.Abs(myRigid.gravityScale);

        tapCoolDownCounter += Time.deltaTime;
        if (clickedBtn == true && tapCoolDownCounter >= tapCoolDownTime)
        {
            flyingFx.Emit(30);
            fartFx.Emit(10);
            myRigid.AddForce(new Vector2(0f, 1000f * Time.deltaTime), ForceMode2D.Impulse);
            tapCoolDownCounter = 0f;
            clickedBtn = false;
        }

        clickedBtn = false;

    }

    void DownFlying()
    {
        clickedBtn = false;
        if (myRigid.gravityScale > 0)myRigid.gravityScale *= -1f;

        int i = 0;

        wingAnim.SetBool("kebawah", false);
        flyingDown = false;

        while (i < Input.touchCount)
        {
            if (Input.GetTouch(i).position.x < screenWidth / 2)
            {
                Flying(-1.0f);
                flyingDown = true;
                wingAnim.SetBool("kebawah", true);

            }
            i++;
        }

        if (flyingDown == false)
        {
            for (int j = 0; j < upWindFx.Length; j++)
            {
                downWindFx[j].Stop();
            }

            for (int j = 0; j < upWindFx.Length; j++)
            {
                upWindFx[j].Play();
            }
        }

        else
        {
            for (int j = 0; j < upWindFx.Length; j++)
            {
                downWindFx[j].Play();
            }

            for (int j = 0; j < upWindFx.Length; j++)
            {
                upWindFx[j].Stop();
            }
        }

    }

    void UpsideDownFlying()
    {
        upsideDownCoolDownCounter += Time.deltaTime;
        if (clickedBtn == true && upsideDownCoolDownCounter >= upsideDownCoolDownTime)
        {
            myRigid.gravityScale *= -1f;
            upsideDownCoolDownCounter = 0f;
            clickedBtn = false;
        }

        if (myRigid.gravityScale <= 0)
        {
            baloonAnim.SetBool("isFlying", true);
            fireFx.Play();
        }

        else {
            baloonAnim.SetBool("isFlying", false);
            fireFx.Stop();
        }
        

        clickedBtn = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boundaries"))
        {
            bodyAnim.SetBool("isFalling", false);
            touchingBoundaries = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boundaries"))
        {
            touchingBoundaries = false;
        }
    }
}
