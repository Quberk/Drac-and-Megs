using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private PlayerStat player;
    private Gun myGun;

    public float damage;
    public GameObject soundEffect;
    [HideInInspector]
    public bool ready = true;
    [HideInInspector]
    public bool imHooking;

    public float posChange;
    public GameObject hookingPos;
    public GameObject enemyPos;
    [SerializeField]
    private Vector2 firstHookPos;

    [HideInInspector]
    public bool imCatching = false;
    [HideInInspector]
    public bool isAttacking = false;
    private bool isCatching = false;

    private bool isCoolDown = false;
    public float coolDownTime;
    private float coolDownCounter = 0;

    public ParticleSystem effect;

    public float distRay;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
        myGun = FindObjectOfType<Gun>();
        firstHookPos = hookingPos.transform.localPosition;

        //effect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(hookingPos.transform.position, hookingPos.transform.position + transform.right * distRay, Color.red);
        if (imHooking == true)
        {

            ready = false;
            anim.SetTrigger("isAttacking");
            Instantiate(soundEffect, transform.position, Quaternion.identity);

            imHooking = false;
            
            //isAttacking = true;
        }

        if (isCoolDown == true)
        {
            PostAttack();
        }
    }

    //Ketika sedang Idle
    public void Idle()
    {
        hookingPos.transform.localPosition = firstHookPos;
        isAttacking = false;
        isCatching = false;
        ready = true;
    }

    //Perubahan Posisi COllder sesuai Animasi dan Juga Hook Pos
    public void ColliderMove()
    {
        /* if (isAttacking == true && imCatching == true)
         {
             anim.SetTrigger("isWaiting");
             hookingPos.transform.position = enemyPos.transform.position;
             isCatching = true;
             isAttacking = false;
             imCatching = false;
             //Hapus Jika Tak Bekerja
             HookCollider hookColl = hookingPos.GetComponent<HookCollider>();
             hookColl.enemyTransform.GetComponent<Collider2D>().enabled = false;
             hookColl.enemyTransform.GetComponent<EnemyMovement>().allowMove = false;
         }*/

        hookingPos.transform.position = new Vector2(hookingPos.transform.position.x + posChange, hookingPos.transform.position.y);
        effect.Emit(3);

    }

    //Setelah selesai menyelesaikan attack animation tetapi tidak mendapatkan Enemy
    public void FinishedAttacking()
    {
        if (isCatching == false) 
        { 
        anim.SetTrigger("isHooking");
        isAttacking = true;
        }

        effect.Emit(3);
    }

    //Fungsi ketika menarik Musuh
    public void Hooking()
    {
        //<------------------Jika tidak Ingin lagi memberikan kemampuan untuk menghook dengan mudah maka disable code di bawah ini saja------->

        if (imCatching == true)
        {
            //anim.SetTrigger("isWaiting");
            //hookingPos.transform.position = enemyPos.transform.position;
            isCatching = true;
            //isAttacking = false;
            //imCatching = false;
            //Hapus Jika Tak Bekerja
            HookCollider hookColl = hookingPos.GetComponent<HookCollider>();
            if (hookColl.enemyTransform) hookColl.enemyTransform.transform.position = hookingPos.transform.position;
            //hookColl.enemyTransform.GetComponent<Collider2D>().enabled = false;
            hookColl.enemyTransform.GetComponent<EnemyMovement>().allowMove = false;

            //Coba"
            hookingPos.transform.position = new Vector2(hookingPos.transform.position.x - posChange, hookingPos.transform.position.y);
        }

        else hookingPos.transform.position = new Vector2(hookingPos.transform.position.x - posChange, hookingPos.transform.position.y);

        effect.Emit(3);
    }
    
    //Fungsi ketika selesai menarik Hook
    public void FinishedHooking()
    {
        if (isCatching == true)
        {
            anim.SetTrigger("isWaiting");
            hookingPos.transform.position = enemyPos.transform.position;
            isCatching = true;
            isAttacking = false;
            imCatching = false;
        }

        else
        {
            anim.SetTrigger("backToState");
            ready = true;
        }

    }

    //Fungsi CoolDown Hook ketika sudah menyerang
    public void CoolDownHooking()
    {
        isCoolDown = true;
    }

    //Fungsi yang dipalnggil untuk memberi Damage selama CoolDown
    void PostAttack()
    {
        if (isCoolDown == true)
        {
            coolDownCounter += Time.deltaTime;
            HookCollider hookColl = hookingPos.GetComponent<HookCollider>();

            if (coolDownCounter >= coolDownTime)
            {
                RestartState();
            }

            else if (hookColl.enemyTransform != null)
            {
                hookColl.enemyTransform.transform.position = hookingPos.transform.position;
                hookColl.enemyTransform.GetComponent<Collider2D>().enabled = false;
                hookColl.enemyTransform.GetComponent<EnemyMovement>().allowMove = false;
            }

            
        }

    }

    public void Damaging()
    {
        HookCollider hookColl = hookingPos.GetComponent<HookCollider>();
        hookColl.enemyHealth.Damaging(damage);
    }
    
    //Fungsi untuk mereset Semua Stat dari Hook
    public void RestartState()
    {
        HookCollider hookColl = hookingPos.GetComponent<HookCollider>();

        anim.SetTrigger("backToState");
        hookingPos.transform.localPosition = firstHookPos;

        if (hookColl.enemyTransform != null)
        {
            if (hookColl.enemyTransform.GetComponent<EnemyShoot>())
            {
                hookColl.enemyTransform.GetComponent<EnemyShoot>().fireRateCounter = -5f;
                hookColl.enemyTransform.GetComponent<EnemyShoot>().allowShoot = true;
            }

            if (hookColl.enemyTransform.GetComponent<EnemyMovement>())
            {
                hookColl.enemyTransform.GetComponent<Collider2D>().enabled = true;
                EnemyMovement mahmud = hookColl.enemyTransform.GetComponent<EnemyMovement>();
                mahmud.allowMove = true;
                mahmud.waitingCounter = mahmud.waitingTime;
                hookColl.enemyTransform = null;
            }
        }

        coolDownCounter = 0;
        ready = true;
        isCatching = false;
        isCoolDown = false;
    }
}
