using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameLoop.instance.Hit();
        GameLoop.instance.GetComponent<TextShatterEffect>().Explosion(other.ClosestPoint(transform.position));
    }

    private bool CheckHitLocation(Collision collision)
    {
        TextMeshPro textMeshPro = GameLoop.instance.currentText.GetComponent<TextMeshPro>();
        ContactPoint contact = collision.GetContact(0);
        Vector3 hitPoint = contact.point;

        bool res = false;

        // Convert world point to local point of the TextMeshPro object
        Vector3 localHit = textMeshPro.transform.InverseTransformPoint(hitPoint);

        TMP_TextInfo textInfo = textMeshPro.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
                continue;

            // Each character is a quad: bottom left and top right
            Vector3 bottomLeft = charInfo.bottomLeft;
            Vector3 topRight = charInfo.topRight;

            if (localHit.x >= bottomLeft.x && localHit.x <= topRight.x &&
                localHit.y >= bottomLeft.y && localHit.y <= topRight.y)
            {
                if(i >= GameLoop.instance.correctCharacterIndexBegin && i <= GameLoop.instance.correctCharacterIndexEnd)
                {
                    res = true;
                }
                break;
            }
        }

        return res;
    }

    private Vector3 GetFrontTipPosition()
    {
        MeshFilter meshFilter = gameObject.GetComponentInChildren<MeshFilter>();
        if(meshFilter == null) return transform.position;
        
        Bounds bounds = meshFilter.mesh.bounds;
        
        Vector3 localFront = bounds.center + new Vector3(0, 0, bounds.extents.z);
        
        return meshFilter.transform.TransformPoint(localFront);
    }
}
