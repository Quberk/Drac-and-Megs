using UnityEngine;

public class GunRage : MonoBehaviour
{
    public bool gatlingGun;
    public bool eatingGun;
    public bool rocketGun;

    public GameObject shootingSound;

    [Header("General")]
    public GameObject UIEffect;
    public float fireRate;
    private float fireCounter;
    private Animator myAnim;

    [Header("Golden Gatling")]
    public GameObject projectile;
    public GameObject[] projectilePos;
    public GameObject ammo;
    public float ammoShifting;
    public ParticleSystem ammoShell;
    public GameObject effectPos;
    private int posNum = 0;

    [Header("Rocket Launcher")]
    public float chargingTime;
    public GameObject chargingEffect;
    public GameObject fullChargeEffect;
    public GameObject fullChargeEffect1;
    public GameObject chargingSound;
    public GameObject fullChargeSound;
    private GameObject sound1;
    private GameObject sound2;
    public GameObject shootingPos;
    public float posChange;
    private GameObject effect1;
    private GameObject effect2;
    private GameObject effect3;
    private float chargingCounter = 0f;
    private GameObject myBullet;
    private bool alreadyFull = false;


    [Header("Soul Eater")]
    public float xAxis;
    public float yAxis;
    public float damage;
    public GameObject teleportEffect;
    public GameObject teleportEffect1;
   // private GameObject enemyStat;
    private GameObject[] enemy;
    private Vector3 lastPos;
    [HideInInspector]
    public bool isHooking;
    public GameObject eatingSound;
    private GameObject player;
    private float playerSpeed;
    private float gravity;

    [HideInInspector]
    public bool isShooting;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (gatlingGun == true) { 
            ammoShell.Stop();
        }

        else if (eatingGun == true)
        {
            playerSpeed = player.GetComponent<CharacterMovement>().speed;
            gravity = player.GetComponent<Rigidbody2D>().gravityScale;
            lastPos = player.transform.position;
        }

        fireCounter = fireRate;

        //Membuat Efek UI pada Layar sesuai dengan senjata Rage
        GameObject effectUI = Instantiate(UIEffect, transform.position, Quaternion.identity);
        effectUI.transform.SetParent(GameObject.Find("Canvas").transform);
        effectUI.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

        if (gatlingGun == true) GatlingGun();
        else if (eatingGun == true) EatingGun();
        else RocketGun();
    }

    void GatlingGun()
    {
        //goldenEffect1.Emit(1);
        //goldenEffect2.Emit(1);

        fireCounter += Time.deltaTime;

        if (fireCounter >= fireRate && isShooting == true)
        {
            myAnim.SetTrigger("isShooting");
            if (posNum == projectilePos.Length)
            {
                posNum = 0;
                
                ammo.transform.localPosition = new Vector3(ammo.transform.localPosition.x,
                                                           ammo.transform.localPosition.y + ammoShifting, ammo.transform.localPosition.z);
                
                if (ammo.transform.localPosition.y >= 2.72f)
                    ammo.transform.localPosition = new Vector3(ammo.transform.localPosition.x, 1.22f, ammo.transform.localPosition.z);
            } 

            Instantiate(projectile, projectilePos[posNum].transform.position, Quaternion.identity);
            Instantiate(shootingSound, transform.position, Quaternion.identity);
            ammoShell.Emit(1);
            fireCounter = 0f;
            posNum++;
        }
    }

    void EatingGun()
    {
        //fireCounter += Time.deltaTime;

        if (fireCounter >= fireRate && isHooking == true)
        {
            myAnim.SetBool("isAttacking", false);
            enemy = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemy.Length != 0)
            {
                //lastPos = player.transform.position;
                //Memanipulasi Stat dari Player
                player.GetComponent<CharacterMovement>().speed = 0;
                player.GetComponent<Rigidbody2D>().gravityScale = 0;


                //random = Random.Range(0, enemy.Length - 1);
                player.transform.position = new Vector3(enemy[0].transform.position.x - xAxis,
                                                        enemy[0].transform.position.y - yAxis,
                                                        enemy[0].transform.position.z);
                Eating();
                //Menghentikan Pergerakan Enemy
                if (enemy[0].GetComponent<EnemyMovement>()) enemy[0].GetComponent<EnemyMovement>().moving = false;

                //Instantiate(teleportEffect, lastPos, Quaternion.identity);
                Instantiate(teleportEffect1, player.transform.position, Quaternion.identity);

                myAnim.SetBool("isAttacking", true);
                fireCounter = 0f;
            }
            
            isHooking = false;
        }
        else isHooking = false;
    }

    //Bagian Kedua dari SoulEater
    public void Eating()
    {

        if (enemy[0])
        {
            //Memanipulasi Stat dari Enemy
            if (enemy.Length >= 3f)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (enemy[i].GetComponent<EnemyHealth>()) enemy[i].GetComponent<EnemyHealth>().Damaging(damage);
                    else enemy[i].GetComponent<EnemyFormation>().Damaging(damage);

                    Instantiate(teleportEffect, enemy[i].transform.position, Quaternion.identity);
                }
            }

            else
            {
                if (enemy[0].GetComponent<EnemyHealth>()) enemy[0].GetComponent<EnemyHealth>().Damaging(damage);
                else enemy[0].GetComponent<EnemyFormation>().Damaging(damage);

                Instantiate(teleportEffect, player.transform.position, Quaternion.identity);
            }
            Instantiate(eatingSound, transform.position, Quaternion.identity);
        }
    }

    //Bagian akhir SoulEater
    public void FinishedEating()
    {
        myAnim.SetBool("isAttacking", false);

        //Memanipulasi Stat dari Player
        player.GetComponent<CharacterMovement>().speed = playerSpeed;
        player.GetComponent<Rigidbody2D>().gravityScale = gravity;

        player.transform.position = new Vector3(lastPos.x, player.transform.position.y, player.transform.position.z);
        fireCounter = fireRate;
        isHooking = false;

    }

    void RocketGun()
    {
        fireCounter += Time.deltaTime;
        if (fireCounter >= fireRate)
        {
            //Charge masih 0
            if (isShooting == true && chargingCounter <= 0)
            {
                chargingCounter += Time.deltaTime;
                
                //Efek Charging
                effect1 = Instantiate(chargingEffect, shootingPos.transform.position, Quaternion.identity);
                effect1.transform.SetParent(transform);
                sound1 = Instantiate(chargingSound, transform.position, Quaternion.identity);

            }

            //Sedang Charging
            else if (isShooting == true && chargingCounter != 0)
            {
                chargingCounter += Time.deltaTime;
                myAnim.SetTrigger("isCharging");

                //Jika Charging sudah siap untuk di Tembakkan
                if (chargingCounter >= chargingTime && alreadyFull == false)
                {
                    //Efek Suara
                    sound2 = Instantiate(fullChargeSound, transform.position, Quaternion.identity);
                    Destroy(sound1);

                    effect2 = Instantiate(fullChargeEffect, shootingPos.transform.position, Quaternion.identity);
                    effect2.transform.SetParent(transform);
                    effect3 = Instantiate(fullChargeEffect1, shootingPos.transform.position, Quaternion.identity);
                    effect3.transform.SetParent(transform);
                    alreadyFull = true;
                    Destroy(effect1);
                }

            }

            //Melepaskan Projectile
            else if (isShooting == false && chargingCounter >= chargingTime)
            {
                
                Instantiate(shootingSound, transform.position, Quaternion.identity);
                myBullet = Instantiate(projectile, shootingPos.transform.position, Quaternion.identity);
                myBullet.transform.localScale = new Vector3(0.62985f, 0.62985f, 0.62985f);

                Destroy(effect2);
                Destroy(effect3);
                Destroy(sound2);

                myBullet.transform.SetParent(null);
                myBullet.GetComponent<Bullet>().readyToShoot = true;
                myAnim.SetTrigger("isShooting");
                chargingCounter = 0;

                fireCounter = 0f; //Reset Cooldown

                alreadyFull = false; // Reset Efek Full Charge
            }

            //Charging Dibatalkan
            else if (isShooting == false && chargingCounter > 0)
            {
                //Destroy(myBullet);
                myAnim.SetTrigger("isCancelled");
                chargingCounter = 0;

                //Menghancurkan Efek Charging
                Destroy(effect1);
                Destroy(sound1);

                fireCounter = 0f;//Reset Cooldown

                alreadyFull = false; // Reset Efek Full Charge
            }
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
