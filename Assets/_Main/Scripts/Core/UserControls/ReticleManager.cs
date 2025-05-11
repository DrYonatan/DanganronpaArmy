using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleManager : MonoBehaviour
{
    public int speed = 30;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int actualSpeed = speed;
        if(WorldManager.instance.currentRoom.currentInteractable != null)
        actualSpeed *= 4;

        transform.Rotate(0, 0,  actualSpeed * Time.deltaTime);
        
    }
}
