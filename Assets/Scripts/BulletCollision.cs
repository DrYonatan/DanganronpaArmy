using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(CheckHitLocation(collision))
        GameLoop.instance.Hit(collision.contacts[0].point);
    }

    private bool CheckHitLocation(Collision collision)
    {
        Collider other = collision.collider;

        if (other.CompareTag("OrangeHitBox"))
        {
           return true;
        }
        else
        {
           // Handle white (default) hit
           return false;
        }
    }
    
}
