using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleManager : MonoBehaviour
{
    public int speed = 30;
    public Canvas canvas;

    public static ReticleManager instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        int actualSpeed = speed;
        if(WorldManager.instance != null)
        if(WorldManager.instance.currentRoom.currentInteractable != null)
        actualSpeed *= 4;

        transform.Rotate(0, 0,  actualSpeed * Time.deltaTime);
        
    }

    public void ReticleAsCursor()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out pos
        );
        gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
    }
}
