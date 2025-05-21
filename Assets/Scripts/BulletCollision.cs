using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameLoop.instance.Hit(collision.contacts[0].point);
    }

    private bool CheckHitLocation(Collision collision)
    {
        TextMeshPro textMeshPro = GameLoop.instance.currentTexts[0].GetComponent<TextMeshPro>();
        textMeshPro.ForceMeshUpdate();
        
        ContactPoint contact = collision.GetContact(0);
        Vector3 hitPoint = contact.point;
        
        bool res = false;
        
        TMP_TextInfo textInfo = textMeshPro.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
                continue;

            // Each character is a quad: bottom left and top right
            Vector3 bottomLeft = textMeshPro.transform.TransformPoint(charInfo.bottomLeft);
            Vector3 topRight = textMeshPro.transform.TransformPoint(charInfo.topRight);
            
            if (hitPoint.x >= bottomLeft.x && hitPoint.x <= topRight.x &&
                hitPoint.y >= bottomLeft.y && hitPoint.y <= topRight.y)
            {
                res = true;
                if(i >= GameLoop.instance.correctCharacterIndexBegin && i <= GameLoop.instance.correctCharacterIndexEnd)
                {
                    res = true;
                }
                break;
            }
        }

        return res;
    }
    
}
