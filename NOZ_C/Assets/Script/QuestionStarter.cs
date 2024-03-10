using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionStarter : MonoBehaviour
{
    public GameObject uiCanvas; // Tham chiếu đến canvas UI
    private bool isInside = false; // Biến để xác định xem nhân vật có ở bên trong cube không

    void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu nhân vật đi vào trong cube
        if (other.CompareTag("Player"))
        {
            isInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Kiểm tra nếu nhân vật rời khỏi cube
        if (other.CompareTag("Player"))
        {
            isInside = false;
        }
    }

    void Update()
    {
        // Kiểm tra nếu nhân vật ở bên trong cube và người chơi nhấn phím F
        if (isInside && Input.GetKeyDown(KeyCode.F))
        {
            // Hiển thị canvas UI
            uiCanvas.SetActive(true);
        }
    }
}
