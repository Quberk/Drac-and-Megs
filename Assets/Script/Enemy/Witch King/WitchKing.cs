using UnityEngine;

public class WitchKing : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private int waveNum;
    private int attackCount;

    private bool dead = false;

    [SerializeField]
    private Animator myAnim;

    [Header("Position")]
    private Vector2 destination;
    private int stayingCount = 0;
    [SerializeField]
    private float xFirstPos, yPos1, yPos2;
    [SerializeField]
    private float xSecondPos, xSecondPos1;
    [SerializeField]
    private float xFinalPos;
    [SerializeField]
    private float attackDurationTime, stayDurationTime;
    [SerializeField]
    private float[] stopTime;
    private float stayDurationCounter = 0f;

    [Header("Attack")]
    [SerializeField]
    private WitchSafeRange witchSafeRange;
    [SerializeField]
    private ParticleSystem[] attackEffects;
    [SerializeField]
    private Animator shieldWarningFx;
    private bool alreadyAttacking = false;

    [Header("Get Attacked")]
    [SerializeField]
    private GameObject money;
    [SerializeField]
    private ParticleSystem jeweleryHitFx;

    // Start is called before the first frame update
    void Start()
    {

        float xPos = xFirstPos;
        float yPos = Random.Range(yPos1, yPos2);

        transform.position = new Vector2(xPos, yPos);

        stayingCount++;
        destination = GetLocation(stayingCount);

        for (int i = 0; i < attackEffects.Length; i++)
        {
            attackEffects[i].Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if ((Vector2.Distance(transform.position, destination)) <= 0.2f)
        {
            transform.position = destination;
            if (stayingCount == 3) AttackingZone();
        }
        else { transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime); alreadyAttacking = false; }


        if (attackCount <= waveNum)
        {
            stayDurationCounter += Time.deltaTime;
            if (stayDurationCounter >= (stayDurationTime))
            {
                stayingCount++;
                stayDurationCounter = 0f;

                //Resetting The Staying Count
                if (stayingCount > 3) { stayingCount = 1; attackCount++; FinishAttacking(); }

                switch (stayingCount)
                {
                    case 1:
                        stayDurationTime = stopTime[0];
                        break;
                    case 2:
                        stayDurationTime = stopTime[1];
                        break;
                    case 3:
                        stayDurationTime = stopTime[2];
                        break;
                }

                destination = GetLocation(stayingCount);
            }
        }

        else if (dead == false){ 
            destination = new Vector2(xFinalPos * 2f, Random.Range(yPos1, yPos2));
            dead = true;
        }

        //Jika sudah selesai
        if (transform.position.x >= xFinalPos * 2f) Dead();
    }

    void AttackingZone()
    {
        if (alreadyAttacking == false)
        {
            for (int i = 0; i < attackEffects.Length; i++)
            {
                attackEffects[i].Play();
            }

            shieldWarningFx.SetTrigger("Attacking");
            alreadyAttacking = true;
        }

    }

    public void setWaveNum(int wave)
    {
        waveNum = wave;
    }

    public void IsAttacking()
    {
        witchSafeRange.setAttackingStatus(true);
    }

    public void FinishAttacking()
    {
        witchSafeRange.setAttackingStatus(false);
    }

    Vector2 GetLocation(int stayCount)
    {
        Vector2 dest = new Vector2(0f, 0f);


        //Jika Stay belum mencapat 2 maka Witch akan mencari Tempat stay sementara
        if (stayCount <= 2)
        {

            dest = new Vector2(Random.Range(xSecondPos, xSecondPos1), Random.Range(yPos1, yPos2));
        }

        else
        {
            dest = new Vector2(xFinalPos, Random.Range(yPos1, yPos2));
        }

        return dest;
    }

    public void Damaging(float damage)
    {
        int damageInt = (int)damage;
        int moneyAmount = damageInt / 100;

        for (int i = 0; i < moneyAmount; i++)
        {
            Instantiate(money, transform.position, Quaternion.identity);
        }

        jeweleryHitFx.Play();
        myAnim.SetTrigger("getHit");
    }

    void Dead()
    {
        Destroy(gameObject);
    }
}
