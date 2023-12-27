using UnityEngine;

public class SnitchHealth : MonoBehaviour
{
    public float health;
    private float healthCounter = 0f;

    public GameObject hpParticle;
    public int hpAmount;

    public GameObject dieSound;

    private Gun gun;
    private GameObject myGun;
    public GameObject[] rageWeapon;

    private Hook[] hook;

    private Animator cameraAnim;

    private bool alreadyPick = false;

    private bool gunTaken = false;

    // Start is called before the first frame update
    void Start()
    {
       // gameObject.SetActive(false);
        gun = FindObjectOfType<Gun>();
        myGun = gun.gameObject;
        if (myGun.CompareTag("Hooker")) hook = FindObjectsOfType<Hook>();
        gunTaken = true;
        cameraAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (healthCounter >= health)
        {

            for (int i = 0; i < hpAmount; i++)
            {
                Instantiate(hpParticle, transform.position, Quaternion.identity);
                Instantiate(dieSound, transform.position, Quaternion.identity);
            }

            cameraAnim.SetTrigger("usingRage");

            //Untuk Hook tidak Langsung di disaktifkan
            if (myGun.CompareTag("Hooker"))
            {
                for (int i = 0; i < hook.Length; i++)
                {
                    hook[i].RestartState();
                    myGun.SetActive(false);
                }
            }
            //Untuk Senjata Lain tidak Diaktifkan ketika memencet Tombol Rage
            else myGun.SetActive(false);

            //Mempersiapkan Rage Gun
            int randomNumber = Random.Range(0, (rageWeapon.Length) * 100);
            PlayerStat myPlayer = FindObjectOfType<PlayerStat>();
            myPlayer.rageUsed = true;
            myPlayer.rageCounter = myPlayer.rage;
            GameObject gunPos = GameObject.Find("Gun");

            //Memakai Rage Gun Random
            for (int i = 0; i < rageWeapon.Length; i++)
            {
                if (randomNumber <= (i + 1) * 100 && alreadyPick == false)
                {
                    GameObject weapon = Instantiate(rageWeapon[i], gunPos.transform.position, Quaternion.identity);

                    weapon.transform.SetParent(gunPos.transform);
                    weapon.transform.localScale = new Vector3(4.448224f, 4.448224f, 4.448224f);

                    alreadyPick = true;
                }
            }
            Destroy(gameObject);
        }


    }

    public void Damaging(float damage)
    {
        healthCounter += damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) Damaging(100f);
    }
}
