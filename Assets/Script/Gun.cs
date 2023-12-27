using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Type")]
    public bool automaticGun;
    public bool chargingGun;
    public bool hookerGun;

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
    public GameObject chargingEffect;
    private GameObject effect1;
    private float chargingCounter = 0f;
    private GameObject myBullet;
    private bool alreadyFull = false;

    [Header("Hooker Gun")]
    public GameObject[] hook;
    [HideInInspector]
    public bool isHooking;
    private int hookActiveCounter = 0;
    private string hookNumber;
    private bool alreadyHooking;

    private Animator myAnim;
         

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
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

        else
        {
            HookerGun();
        }
    }

    //All the Shot Takes
    void Shoot()
    {
        Instantiate(bullet, shootingPos.transform.position, Quaternion.identity);
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
            if (isShooting == true && chargingCounter <= 0)
            {
                myBullet = Instantiate(bullet, shootingPos.transform.position, Quaternion.identity);

                //Bullet harus terus mengikuti Pos ketika di instantiate
                myBullet.transform.position = shootingPos.transform.position;
                chargingCounter += Time.deltaTime;

                //Efek Charging
                effect1 = Instantiate(chargingEffect, shootingPos.transform.position, Quaternion.identity);
                effect1.transform.SetParent(transform);
            }

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
                    alreadyFull = true;
                }

            }

            else if (isShooting == false && chargingCounter >= chargingTime)
            {
                myBullet.GetComponent<Bullet>().readyToShoot = true;
                myAnim.SetTrigger("isShooting");
                chargingCounter = 0;

                //Menghancurkan Efek Charging
                Destroy(effect1);
                Instantiate(fireEffect, shootingPos.transform.position, Quaternion.identity);

                fireRateCounter = 0f; //Reset Cooldown

                alreadyFull = false; // Reset Efek Full Charge
            }

            else if (isShooting == false && chargingCounter > 0)
            {
                Destroy(myBullet);
                myAnim.SetTrigger("isCancelled");
                chargingCounter = 0;

                //Menghancurkan Efek Charging
                Destroy(effect1);

                fireRateCounter = 0f;//Reset Cooldown

                alreadyFull = false; // Reset Efek Full Charge
            }
        }
    }

    //Senjata Hooker
    void HookerGun()
    {
        if (isHooking == true)
        {
            /*hookActiveCounter += 1;
            if (hookActiveCounter > hook.Length) hookActiveCounter = 0;
            GameObject hooker = GameObject.Find("Hook" + hookActiveCounter);
            Hook hookerScript = hooker.GetComponent<Hook>();
            if (hookerScript.ready == true) hookerScript.imHooking = true;
            isHooking = false;
            */

            alreadyHooking = false;
            //Coba2
            for (int i = 0; i < hook.Length; i++)
            {
                Hook myHook = hook[i].GetComponent<Hook>();
                if (myHook.ready == true && alreadyHooking == false)
                {
                    myHook.imHooking = true;
                    alreadyHooking = true;
                }                         
            }

            isHooking = false;
        }
    }
}
