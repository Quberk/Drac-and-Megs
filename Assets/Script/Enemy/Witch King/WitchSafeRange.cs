using UnityEngine;

public class WitchSafeRange : MonoBehaviour
{

    private bool attacking = false;
    private bool playerInsideSaveZone = true;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking == true && playerInsideSaveZone == false) player.GetComponent<PlayerStat>().HealthConsumption(100f, false);
    }

    public void setAttackingStatus(bool attack)
    {
        attacking = attack;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInsideSaveZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInsideSaveZone = false;
        }
    }
}
