using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TriggerDodge : MonoBehaviour
{
    private Animator ani;

    private void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        ani = temp.GetComponentInChildren<Animator>();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Enemy" && ani.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
        {
            other.isTrigger = true;
        } 
        else if(other.tag == "Enemy" && !ani.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
        {
            other.isTrigger = false;
        }    
    }
}
