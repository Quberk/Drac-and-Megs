using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public string boolName;
    private Animator myAnim;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    void EventAnimationTrigger()
    {
        myAnim.SetBool(boolName, false);
    }
}
