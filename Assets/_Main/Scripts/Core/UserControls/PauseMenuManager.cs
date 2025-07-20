using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    public List<GameObject> menuItems;
    public int currentItemIndex;
    public Image menuTopEffect;

    // Start is called before the first frame update
    void Start()
    {
        currentItemIndex = 0;
        EventSystem.current.SetSelectedGameObject(menuItems[0]);
        UpdateCurrentItem();
    }

    // Update is called once per frame
    void Update()
    {
    
        if(Input.GetKeyDown(KeyCode.W))
        {
            if(currentItemIndex > 0)
            currentItemIndex--; 
            UpdateCurrentItem();
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            if(currentItemIndex < menuItems.Count-1)
            currentItemIndex++;
            UpdateCurrentItem();
        }
    }

    public void UpdateCurrentItem()
    {
        GameObject currentItem = menuItems[currentItemIndex];
        menuTopEffect.material.SetColor(
            "_ColorTop",
             currentItem.GetComponent<Button>().colors.highlightedColor);
             
        EventSystem.current.SetSelectedGameObject(currentItem);
    }
}
