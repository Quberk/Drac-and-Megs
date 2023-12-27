using UnityEngine;

public class Gun : MonoBehaviour
{
    public float energyCost;
    private PlayerStat playerStat;

    [Header("General")]
    public GameObject shootingSound;

    [Header("Gun Type")]
    public bool automaticGun;
    public bool chargingGun;
    public bool hookerGun;
    public bool shotGun;

    [HideInInspector]
    public bool isShooting;
    public GameObject fireEffect;

    [Header("Firing")]
    public GameObject bullet;
    public GameObject shootingPos;

    public float fireRateTime;
    private float fireRateCounter = 0f;

    [Header("Charging Gun Stat")]
    public float sizeGrowth;
    public float chargingTime;
    public GameObject fullChargeEffect;
    public GameObject fullChargeEffect1;
    public GameObject fullChargeEffect2;
    public GameObject fullChargeEffect3;
    public GameObject chargingEffect;
    public GameObject chargingSound;
    public GameObject fullChargeSound;
    private GameObject sound1;
    private GameObject sound2;
    private GameObject effect1;
    private float chargingCounter = 0f;
    private GameObject myBullet;
    private bool alreadyFull = false;
    private GameObject effectFull;
    private GameObject effectFull1;

    [Header("Shot Gun")]
    [SerializeField]
    private GameObject blastInShortRange, blastPos;

    [Header("Hooker Gun")]
    public GameObject[] hook;
    [HideInInspector]
    public bool isHooking;
    private bool alreadyHooking;
    private int numberHook = 0;

    private Animator myAnim;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        playerStat = FindObjectOfType<PlayerStat>();

    }

    // Update is called once per frame
    void Update()
    {
        if (automaticGun == true)
        {
            AutomaticGun();
        }

        else if (chargingGun == true)
        {
            ChargerGun();
        }

        else if (shotGun == true)
        {
            ShotGun();
        }

        else
        {
            HookerGun();
        }

    }

    //All the Shot Takes
    GameObject Shoot()
    {
        GameObject bullets = Instantiate(bullet, shootingPos.transform.position, Quaternion.identity);
        Instantiate(shootingSound, transform.position, Quaternion.identity);

        return bullets;
    }

    //Senjata Atotomatis
    void AutomaticGun()
    {
        fireRateCounter += Time.deltaTime;

        if (isShooting == true && fireRateCounter >= fireRateTime)
        {

            fireRateCounter = 0f;
            myAnim.SetTrigger("isShooting");
            Invoke("Shoot", 0.1f);

            //Effect Firing
            Instantiate(fireEffect, shootingPos.transform.position, Quaternion.identity);

        }
    }

    //Senjata Charging
    void ChargerGun()
    {
        fireRateCounter += Time.deltaTime;
        if (fireRateCounter >= fireRateTime)
        {
            //Charge masih 0
            if (isShooting == true && chargingCounter <= 0)
            {
                //Efek Suara
                sound1 = Instantiate(chargingSound, transform.position, Quaternion.identity);

                myBullet = Instantiate(bullet, shootingPos.transform.position, Quaternion.identity);

                //Bullet harus terus mengikuti Pos ketika di instantiate
                myBullet.transform.position = shootingPos.transform.position;
                chargingCounter += Time.deltaTime;

                //Efek Charging
                effect1 = Instantiate(chargingEffect, shootingPos.transform.position, Quaternion.identity);
                effect1.transform.SetParent(transform);

            }

            //Sedang Charging
            else if (isShooting == true && chargingCounter != 0)
            {
                chargingCounter += Time.deltaTime;
                myAnim.SetTrigger("isCharging");

                //Bullet harus terus mengikuti Pos ketika di instantiate
                myBullet.transform.position = shootingPos.transform.position;

                //Memperbesar ukuran Bullet sampai pada maks
                if (chargingCounter <= chargingTime)
                {
                    myBullet.transform.localScale = new Vector3(myBullet.transform.localScale.x + sizeGrowth * Time.deltaTime,
                                                                myBullet.transform.localScale.y + sizeGrowth * Time.deltaTime,
                                                                0f);
                }

                //Jika Charging sudah siap untuk di Tembakkan
                else if (alreadyFull == false)
                {
                    GameObject chargeFullEfect = Instantiate(fullChargeEffect, shootingPos.transform.position, Quaternion.identity);
                    chargeFullEfect.transform.SetParent(shootingPos.transform);
                    GameObject chargeFullEffect1 = Instantiate(fullChargeEffect1, shootingPos.transform.position, Quaternion.identity);
                    chargeFullEffect1.transform.SetParent(shootingPos.transform);

                    effectFull = Instantiate(fullChargeEffect2, shootingPos.transform.position, Quaternion.identity);
                    effectFull.transform.SetParent(shootingPos.transform);
                    effectFull1 = Instantiate(fullChargeEffect3, shootingPos.transform.position, Quaternion.identity);
                    effectFull1.transform.SetParent(shootingPos.transform);

                    //Efek Suara
                    sound2 = Instantiate(fullChargeSound, transform.position, Quaternion.identity);
                    Destroy(sound1);

                    alreadyFull = true;

                    Destroy(effect1);
                }

            }

            //Melepaskan Projectile
            else if (isShooting == false && chargingCounter >= chargingTime)
            {

                myBullet.GetComponent<Bullet>().readyToShoot = true;
                myAnim.SetTrigger("isShooting");
                chargingCounter = 0;

                //Menghancurkan Efek Charging
                //Destroy(effect1);
                Instantiate(fireEffect, shootingPos.transform.position, Quaternion.identity);
                Instantiate(shootingSound, transform.position, Quaternion.identity);

                fireRateCounter = 0f; //Reset Cooldown

                Destroy(effectFull);
                Destroy(effectFull1);
                Destroy(sound2);

                alreadyFull = false; // Reset Efek Full Charge
            }

            else if (isShooting == false && chargingCounter > 0)
            {
                Destroy(myBullet);
                myAnim.SetTrigger("isCancelled");
                chargingCounter = 0;

                //Menghancurkan Efek Charging
                Destroy(effect1);
                Destroy(sound1);

                fireRateCounter = 0f;//Reset Cooldown

                alreadyFull = false; // Reset Efek Full Charge
            }
        }
    }

    //Senjata Shotgun
    void ShotGun()
    {
        fireRateCounter += Time.deltaTime;

        if (isHooking == true && fireRateCounter >= fireRateTime)
        {
            fireRateCounter = 0f;

            for (int i = 0; i  < 5; i++)
            {
                GameObject bulleth = Shoot();
                bulleth.GetComponent<Bullet>().numberOfBulletShotGun = i;
            }
            myAnim.SetTrigger("isShooting");

            //Effect Firing
            Instantiate(fireEffect, shootingPos.transform.position, Quaternion.identity);
            Instantiate(blastInShortRange, blastPos.transform.position, Quaternion.identity);
        }

        isHooking = false;
    }

    //Senjata Hooker
    void HookerGun()
    {
        fireRateCounter += Time.deltaTime;

        if (isHooking == true)
        {
            if (fireRateCounter >= fireRateTime)
            {
                numberHook += 1;
                alreadyHooking = false;
                //Coba2
                for (int i = 0; i < hook.Length; i++)
                {
                    Hook myHook = hook[i].GetComponent<Hook>();
                    if (myHook.ready == true && alreadyHooking == false)
                    {
                        fireRateCounter = 0f;
                        myHook.imHooking = true;
                        alreadyHooking = true;
                    }
                }

                if (numberHook == hook.Length) { isHooking = false; numberHook = 0; }
            }

            //isHooking = false;
        }

    }
}
