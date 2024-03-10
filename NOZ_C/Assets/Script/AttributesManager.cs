using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    public GameObject FloatingTextPrefab;
    public int health;
    public int attack;

    public void TakeDamage(int amount)
    {
        if (FloatingTextPrefab && health > 0)
        {
            var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
            go.GetComponent<TextMesh>().text = amount.ToString();
        }
        health -= amount;
    }
}