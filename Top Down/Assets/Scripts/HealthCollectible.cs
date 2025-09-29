using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthCollectible : MonoBehaviour
{


    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();


        if (controller != null && controller.currentHealth < controller.maxHealth)
        {
            controller.ChangeHealth(controller.currentHealth += 20);
            Destroy(gameObject);
        }
    }
}
