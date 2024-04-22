using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))] //makes sure it's not null
public class RigidbodySleep : MonoBehaviour
{
    private int sleepCountdown = 4;
    private Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>(); //only do it once when it wakes up
    }

    void FixedUpdate()
    {
        if(sleepCountdown > 0){
            rigid.Sleep();
            sleepCountdown--;
        }
    }

}
