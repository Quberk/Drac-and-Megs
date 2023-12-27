using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RageButton : MonoBehaviour, IPointerClickHandler
{
    private Gun gun;
    private GameObject myGun;
    public GameObject rageWeapon;

    private Hook[] hook;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        gun = FindObjectOfType<Gun>();
        myGun = gun.gameObject;

        hook = FindObjectsOfType<Hook>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(hook.Length);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
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

        PlayerStat myPlayer = FindObjectOfType<PlayerStat>();
        myPlayer.rageUsed = true;
        GameObject gunPos = GameObject.Find("Gun");
        GameObject weapon = Instantiate(rageWeapon, gunPos.transform.position, Quaternion.identity);
        weapon.transform.SetParent(gunPos.transform);
        weapon.transform.localScale = new Vector3(4.448224f, 4.448224f, 4.448224f);

        gameObject.SetActive(false);

    }
}
