using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Court;
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
                if(GameLoop.instance.CheckEvidence(collision.collider.GetComponentInParent<FloatingText>().correctEvidence))
                    GameLoop.instance.Hit(collision.contacts[0].point);
                else
                {
                    StartCoroutine(
                        GameLoop.instance.gameObject.GetComponent<TextShatterEffect>().Deflect(
                            gameObject.GetComponent<TextMeshPro>(),
                            collision.contacts[0].point));
                    GameLoop.instance.WrongHit();
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
        if (other.CompareTag("WhiteHitBox"))
        {
           didHit = true;
        }
        return false;
    }
    
}
