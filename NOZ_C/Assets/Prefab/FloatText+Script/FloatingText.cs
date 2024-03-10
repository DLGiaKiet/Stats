using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float DestroyTime = 3f;
    public Vector3 Offset = new Vector3(0, 0.25f, 0);
    public Vector3 Random_posi_Text = new Vector3(0.25f, 0, 0);
    void Start()
    {
        Destroy(gameObject, DestroyTime);
        transform.localPosition += Offset;
        transform.localEulerAngles += new Vector3(0, -90, 0);
        transform.localPosition += new Vector3(Random.Range(-Random_posi_Text.x, Random_posi_Text.x),
            Random.Range(-Random_posi_Text.y, Random_posi_Text.y), 
            Random.Range(-Random_posi_Text.z, Random_posi_Text.z));
    }
}
