using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Bullet"))
        {
            other.gameObject.SetActive(false);    
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
