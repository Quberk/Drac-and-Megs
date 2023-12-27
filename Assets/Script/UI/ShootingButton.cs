using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShootingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private Gun gun;
    private GunRage gunRage;
    private PlayerStat playerStat;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        gun = FindObjectOfType<Gun>();
        playerStat = FindObjectOfType<PlayerStat>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStat.rageUsed == true) { gunRage = FindObjectOfType<GunRage>();
            gun.isShooting = false;
            gun.isHooking = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerStat.rageUsed == true) gunRage.isShooting = true;
        else gun.isShooting = true;

        anim.SetBool("pressed", true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (playerStat.rageUsed == true) gunRage.isShooting = false;
        else gun.isShooting = false;

        anim.SetBool("pressed", false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (playerStat.rageUsed == true) gunRage.isHooking = true;
        gun.isHooking = true;
    }
}
