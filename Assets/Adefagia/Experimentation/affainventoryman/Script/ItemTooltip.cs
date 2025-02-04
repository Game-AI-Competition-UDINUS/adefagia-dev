using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] Text ItemNameText;
    [SerializeField] Text ItemTypeText;
    [SerializeField] Text ItemDescriptionText;

    private void Awake()
	{
		gameObject.SetActive(false);
	}

    public void ShowTooltip(Item item)
    {
        ItemNameText.text = item.ItemName;
        ItemTypeText.text = item.GetItemType();
        ItemDescriptionText.text = item.GetDescription();

        gameObject.SetActive(true);
        RectTransform tipWindow = gameObject.GetComponent<RectTransform>();

        float horizontal = 0;
        float vertical = 0;
        float offsetX = 10; 
        float offsetY = 10;
        Vector2 mousePos = Input.mousePosition;
        
        // Camera.main.pixelWidth == 1920
        // Camera.main.pixelHeight == 1080

        if(mousePos.x + tipWindow.sizeDelta.x < 1920) horizontal = 1; else horizontal = 0;  //check your horizontal mouse position
        if(tipWindow.sizeDelta.y + mousePos.y > 1080) vertical = 1; else vertical = 0; //check your vertical mouse position
        if(horizontal == 1)
            offsetX = tipWindow.sizeDelta.x + 10; else offsetX = 10; //if overdraw change side. Could also be changed to pixel values if they are known to align the tooltip to the side without jumping from one side to the other
        if(vertical == 1)
            offsetY = -tipWindow.sizeDelta.y - 10; else offsetY = 10;
        
        transform.position =  new Vector2(mousePos.x + offsetX, mousePos.y + offsetY); //Change tooltip position according to your mouseposition and overdraw/correction values

    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
