using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShootingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    private Gun gun;
    private GunRage gunRage;
    private PlayerStat playerStat;

    // Start is called before the first frame update
    void Start()
    {
        gun = FindObjectOfType<Gun>();
        playerStat = FindObjectOfType<PlayerStat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStat.rageUsed == true) gunRage = FindObjectOfType<GunRage>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerStat.rageUsed == true) gunRage.isShooting = true;
        else gun.isShooting = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (playerStat.rageUsed == true) gunRage.isShooting = false;
        else gun.isShooting = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (playerStat.rageUsed == true) gunRage.isHooking = true;
        gun.isHooking = true;
    }
}
