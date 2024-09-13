using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPictureScript : MonoBehaviour
{
    bool isMoving;
    // Start is called before the first frame update
    void Start()
    {

        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveImage());
        }

    }

    private IEnumerator MoveImage()
    {
        Debug.Log("Activated now!");
        isMoving = true;
        float elapsedTime = 0;
        float duration = 8f;
        float elapsedTime2 = 0;
        

        GameObject image = GameObject.Find($"VN controller/Root/Canvas - Overlay/MovingImage");
   
        Vector3 left = new Vector3(-440, 0, 0);
        Vector3 startPos = image.transform.position;
        Vector3 leftTargetPos = startPos + left; // Adjust this vector to change the direction
        while (elapsedTime < duration)
        {
            if (elapsedTime < 3f)
            {
                image.transform.position = Vector3.Lerp(startPos, leftTargetPos, elapsedTime / 3f);
            }

            else if (elapsedTime > 4f)
            {
                image.transform.position = Vector3.Lerp(leftTargetPos, startPos, elapsedTime2 / 3f);
                elapsedTime2 += Time.deltaTime;
            }


            elapsedTime += Time.deltaTime;
            yield return null;

        }
        image.transform.position = startPos;
        isMoving = false;
    }

}
