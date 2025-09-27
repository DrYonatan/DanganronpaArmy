using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletCollision : MonoBehaviour
{
    bool didHit = false;
    
    private void OnCollisionEnter(Collision collision)
    {
        if(!didHit)
        {
            if (CheckHitLocation(collision))
            {
                if(GameLoop.instance.CheckEvidence())
                    GameLoop.instance.Hit(collision.contacts[0].point);
                else
                {
                    StartCoroutine(
                        GameLoop.instance.gameObject.GetComponent<TextShatterEffect>().Deflect(
                            gameObject.GetComponent<TextMeshPro>(),
                            collision.contacts[0].point));
                }
            }

            else
            {
                StartCoroutine(
                    GameLoop.instance.gameObject.GetComponent<TextShatterEffect>().Deflect(
                        gameObject.GetComponent<TextMeshPro>(),
                        collision.contacts[0].point));
            }

        }
        
    }

    private bool CheckHitLocation(Collision collision)
    {
        Collider other = collision.collider;

        if (other.CompareTag("OrangeHitBox"))
        {
            didHit = true;
           return true;
        }
        else if (other.CompareTag("WhiteHitBox"))
        {
           // Handle white (default) hit
           didHit = true;
           return false;
        }
        else 
        return false;
    }
    
}
