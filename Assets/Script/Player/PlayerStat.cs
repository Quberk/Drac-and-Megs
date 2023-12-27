using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    private Animator camAnim;

    private Animator hitEffect;

    [Header("Health")]
    public float health;
    [HideInInspector]
    public float healthCounter;
    //public Slider healthBar;
    public SpriteRenderer mySprite;
    public GameObject hitSound;
   // private GameObject healthLowEffect;
   // private float currentHp;
    //private float lastHp;   

    [Header("Rage")]
    public float rage;
    //public GameObject rageButton;
    [HideInInspector]
    public bool rageUsed;
    [HideInInspector]
    public float rageCounter;
    public Slider rageBar;
    public GameObject rageEffect;
    public GameObject rageShieldEffect;
    public float afterRageTime;
    private float afterRageCounter = 0f;

    [Header("Monster Rage")]
    private float monsterRageMeter;
    private bool monsterRageUsed = false;
    [SerializeField]
    private float monsterRageSpeed;


    private bool invincible = false;

    private GameObject rageScreen;

    private GameObject myGun;
    private GameObject theGun;

    [HideInInspector]
    public bool imHit;

    private GameManaging gameManaging;


    // Start is called before the first frame update
    void Start()
    {
        gameManaging = FindObjectOfType<GameManaging>();

        hitEffect = GameObject.Find("Hit_Screen_Effect").GetComponent<Animator>();

        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        //gun = FindObjectOfType<Gun>();

        //Kode di bawah ini coba2
        GameObject sceneController = FindObjectOfType<SceneController>().gameObject;
        myGun = sceneController.GetComponent<SceneController>().theGun;
        theGun = Instantiate(myGun, transform.position, Quaternion.identity);
        theGun.transform.parent = GameObject.Find("Gun").transform;
        theGun.transform.localScale = new Vector3(4.787004f, 4.787004f, 4.787004f);
        theGun.transform.localPosition = new Vector3(0f, 0f, 0f);
        Destroy(sceneController);

        rageScreen = GameObject.Find("Rage_Screen_Effect");
        //healthLowEffect = GameObject.Find("Health_Screen_Effect");
        //myGun = gun.gameObject;

        healthCounter = health;
        //energyCounter = energy;
        rageCounter = 0;

        rageEffect.SetActive(false);
        rageShieldEffect.SetActive(false);
        rageScreen.SetActive(false);

        //healthLowEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (healthCounter <= 0)
        {
            mySprite.color = new Vector4(0f, 0f, 0f, 1f);
        }

        //healthBar.value = healthCounter;
        rageBar.value = rageCounter;


        //GetHit();
        InvincibleTime();

        //Rage Thing
        /*if (rageCounter >= rage)
        {
            rageCounter = rage;
        }*/

        if (rageUsed == true) UsingRage();

        MonsterRage();
        
    }

    public void HealthConsumption(float damage, bool hpItem)
    {
        if (hpItem == true) healthCounter -= damage;
        else if (rageUsed == false && invincible == false)
        {
            healthCounter -= damage; imHit = true;
            camAnim.SetTrigger("hit");
            Instantiate(hitSound, transform.position, Quaternion.identity);
        }
    }


    public void RageConsumption(float rageCost)
    {
        if (rageUsed == false) rageCounter -= rageCost;
    }

    void UsingRage()
    {
        rageCounter -= Time.deltaTime * 5f;
        gameManaging.rageMode = true; //Membuat Enemy berlimpah

        GunRage gunRage = FindObjectOfType<GunRage>();
        rageEffect.SetActive(true);
        rageShieldEffect.SetActive(true);

        rageScreen.SetActive(true);

        if (rageCounter <= 0) {
            //Jika SoulEater Gun selesai dipakai dan Player tidak kembali ke Posisinya kembali
            if (gunRage.eatingGun == true) gunRage.FinishedEating();

            gameManaging.rageMode = false; //Membuat Enemy menjadi normal kembali
            invincible = true;
            rageEffect.SetActive(false);
            rageScreen.SetActive(false);
            rageCounter = 0;
            rageUsed = false;
            theGun.SetActive(true);

            gunRage.Death();
        }

        else if (rageCounter <= 10f)
        {
            gameManaging.rageMode = false; //Membuat Enemy menjadi normal kembali
        }

        //Menonaktifkan Obstacle
        GameObject[] obstacle = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacles in obstacle)
        {
            if (obstacles.GetComponent<ObstacleController>()) obstacles.GetComponent<ObstacleController>().Deactivate();
            else if (obstacles.GetComponent<BombController>()) obstacles.GetComponent<BombController>().Deactivate();
        }
    }

    //Invincible setelah Rage untuk Bebrapa detik
    void InvincibleTime()
    {
        if (invincible == true)
        {
            afterRageCounter += Time.deltaTime;

            if (afterRageCounter >= afterRageTime) { rageShieldEffect.SetActive(false); afterRageCounter = 0f; invincible = false; }
            else rageShieldEffect.SetActive(true);
        }
    }

    //Monster Rage
    void MonsterRage()
    {
        if (monsterRageUsed == true)
        {
            monsterRageMeter -= Time.deltaTime;
        }

        if (monsterRageMeter <= 0)
        {
            monsterRageMeter = 0;
            monsterRageUsed = false;
        }
    }

    public void MonterRageConsumption(float rages)
    {
        monsterRageMeter += rages;
    }


}
