using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttribute : MonoBehaviour
{
    public AttributesManager atm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && PlayerCombat.isAttacking)
        {
            other.GetComponent<AttributesManager>().TakeDamage(atm.attack);
        }    
    }
}
