using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [HideInInspector]
    public bool ready = true;
    [HideInInspector]
    public bool imHooking;

    public float collChange;
    private Collider2D myColl;
    private Vector2 collStartPos;

    public GameObject hookingPos;

    private bool isAttacking = false;
    private bool isCatching = false;

    public float hookingJumpFrame;
    private int frameHook = 0;

    private bool coolDownReady;
    public float coolDownTime;
    private float coolDownCounter = 0;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        myColl = GetComponent<Collider2D>();

        collStartPos = myColl.offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (imHooking == true)
        {
            ready = false;
            anim.SetTrigger("isAttacking");

            imHooking = false;
            isAttacking = true;
        }

    }

    //Perubahan Posisi COllder sesuai Animasi dan Juga Hook Pos
    public void ColliderMove()
    {
        myColl.offset = new Vector2(myColl.offset.x + collChange, myColl.offset.y);
        hookingPos.transform.position = new Vector2(transform.position.x + collChange, transform.position.y);
        frameHook += 1;
    }
    
    //Fungsi ketika selesai menarik Hook
    public void FinishedHooking()
    {
        if (isCatching == true) anim.SetTrigger("isWaiting");
        else
        {
            anim.SetTrigger("backToState");
            ready = true;
        }
    }

    //Fungsi CoolDown Hook ketika sudah menyerang
    public void CoolDownHooking()
    {
        coolDownCounter += Time.deltaTime;

        if (coolDownCounter >= coolDownTime)
        {
            anim.SetTrigger("backToState");
            coolDownCounter = 0;
            ready = true;
            isCatching = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttacking == true && collision.transform.CompareTag("Enemy"))
        {
            anim.SetTrigger("isHooking");
            anim.Play("Hooking", 0, hookingJumpFrame * frameHook);
            myColl.offset = collStartPos;
            isCatching = true;
            isAttacking = false;
        }

        else if (isAttacking == true && collision.transform.CompareTag("Boundaries"))
        {
            anim.SetTrigger("isHooking");
            myColl.offset = collStartPos;
            isAttacking = false;
        }
    }
}
