using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnergyRechargeButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Slider energyRechargeSlider;
    public float rechargeTime;

    [HideInInspector]
    public float rechargeCounter = 0f;

    private PlayerStat playerStat;

    private bool recharging;

    private Animator anim;

    private Animator camAnim;

    private GameObject rechargeEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        energyRechargeSlider.gameObject.SetActive(false);
        rechargeEffect = GameObject.Find("Energy_Recharge_Effect");
        rechargeEffect.SetActive(false);

        playerStat = FindObjectOfType<PlayerStat>();
        playerStat.energyButton = gameObject.GetComponent<EnergyRechargeButton>();

        anim = GetComponent<Animator>();
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        energyRechargeSlider.value = rechargeCounter;

        if (rechargeCounter >= rechargeTime)
        { 
            playerStat.energyCounter = playerStat.energy;
            rechargeCounter = 0f;
            energyRechargeSlider.value = 0f;
            energyRechargeSlider.gameObject.SetActive(false);
            rechargeEffect.SetActive(false);
            gameObject.SetActive(false);
            camAnim.SetTrigger("usingRage");
        }


        if (recharging == true) rechargeCounter += Time.deltaTime;

        //Jika Player Terkena Hit maka akan tercancel
        if (playerStat.imHit == true)
        {
            rechargeCounter = 0f;
            playerStat.imHit = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rechargeCounter += Time.deltaTime;
        energyRechargeSlider.gameObject.SetActive(true);
        recharging = true;
        rechargeEffect.SetActive(true);

        anim.SetBool("pressed", true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        energyRechargeSlider.gameObject.SetActive(false);
        rechargeCounter = 0f;
        energyRechargeSlider.value = rechargeCounter;
        recharging = false;
        rechargeEffect.SetActive(false);

        anim.SetBool("pressed", false);
    }
}
