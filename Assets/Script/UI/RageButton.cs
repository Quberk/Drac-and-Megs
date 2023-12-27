using UnityEngine;
using UnityEngine.EventSystems;

public class RageButton : MonoBehaviour, IPointerClickHandler
{
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
        gameObject.SetActive(false);
        gun = FindObjectOfType<Gun>();
        cameraAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        //myGun = gun.gameObject;

        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(hook.Length);
        if (gunTaken == false)
        {
            gun = FindObjectOfType<Gun>();
            myGun = gun.gameObject;
            if (myGun.CompareTag("Hooker"))hook = FindObjectsOfType<Hook>();
            gunTaken = true;
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Memberi Efek pada Camera Scene
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

        int randomNumber = Random.Range(0, (rageWeapon.Length) * 100);
        PlayerStat myPlayer = FindObjectOfType<PlayerStat>();
        myPlayer.rageUsed = true;
        GameObject gunPos = GameObject.Find("Gun");

        //Memakai Rage Gun Random
        for(int i = 0; i < rageWeapon.Length; i++)
        {
            if (randomNumber <= (i + 1) * 100 && alreadyPick == false)
            {
                GameObject weapon = Instantiate(rageWeapon[i], gunPos.transform.position, Quaternion.identity);

                weapon.transform.SetParent(gunPos.transform);
                weapon.transform.localScale = new Vector3(4.448224f, 4.448224f, 4.448224f);

                alreadyPick = true;
            }
        }

        alreadyPick = false;
        gameObject.SetActive(false);

    }
}
