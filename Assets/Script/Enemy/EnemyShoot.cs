using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject projectile;
    public float coolDownTime;
    private float coolDownCounter;

    public int projectileNumber;
    private int projectileCounter = 0;

    private int projectilePosNum = 0;

    public GameObject[] projectilePos;
    private GameObject bulletPos;

    public float fireRate;
    [HideInInspector]
    public float fireRateCounter = 0;

    [HideInInspector]
    public bool allowShoot = true;

    //Wand Animator
    public Animator wandAnim;

    public float shootingAnimTime;



    // Start is called before the first frame update
    void Start()
    {
        PosShooting();
        int rand = Random.Range(0, 20);
        coolDownCounter = coolDownTime + rand;
        //StartCoroutine(InvokeMethod(Shoot, projBtwTime));
    }

    // Update is called once per frame
    void Update()
    {
        coolDownCounter += Time.deltaTime;
        if (coolDownCounter >= coolDownTime)
        {
            fireRateCounter += Time.deltaTime;

            if (fireRateCounter >= fireRate && allowShoot == true)
            {
                Shoot();
                fireRateCounter = 0f;
            }

            //Trigger Animation Menembak
            else if (fireRateCounter >= (fireRate - shootingAnimTime) && allowShoot == true) wandAnim.SetBool("isAttacking", true);
        }
    }

    //Fungsi untuk menentukan tempat ditembaknya projectile
    private void PosShooting()
    {
        projectilePosNum++;

        if (projectilePosNum > projectilePos.Length)
        {
            projectilePosNum = 1;
            coolDownCounter = 0;
        }

        bulletPos = projectilePos[projectilePosNum - 1];
    }

    //Fungsi untuk menmebak
    void Shoot()
    {
        
        Instantiate(projectile, bulletPos.transform.position, Quaternion.identity);

        projectileCounter++;

        if (projectileCounter == projectileNumber)
        {
            projectileCounter = 0;
            PosShooting();
        }

    }
}
