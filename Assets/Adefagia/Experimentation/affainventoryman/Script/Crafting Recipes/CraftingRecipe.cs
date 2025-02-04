using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemAmount
{
    public Item Item;
    [Range(1, 10)]
    public int Amount;
}

[CreateAssetMenu]

public class CraftingRecipe : ScriptableObject
{
    public List<ItemAmount> Materials;
    public List<ItemAmount> Results;

    public bool CanCraft(IItemContainer itemContainer)
    {
        return HasMaterials(itemContainer) && HasSpace(itemContainer);   
    }

    private bool HasMaterials(IItemContainer itemContainer)
    {
        foreach (ItemAmount itemAmount in Materials)
        {
            if (itemContainer.ItemCount(itemAmount.Item.ID) < itemAmount.Amount)
            {
                Debug.LogWarning("You dont have the required Materials!");
                return false;
            }
        }
        return true;
    }

    private bool HasSpace(IItemContainer itemContainer)
    {
        foreach (ItemAmount itemAmount in Results)
        {
            if (!itemContainer.CanAddItem(itemAmount.Item, itemAmount.Amount))
            {
                Debug.LogWarning("Your Inventory Full!");
                return false;
            }
        }
        return true;
    }

    public void Craft(IItemContainer itemContainer)
    {
        if (CanCraft(itemContainer))
        {
            RemoveMaterials(itemContainer);
            AddResults(itemContainer);
        }
    }

    private void RemoveMaterials(IItemContainer itemContainer)
    {
        foreach (ItemAmount itemAmount in Materials)
        {
            for (int i = 0; i < itemAmount.Amount; i++)
            {
                Item oldItem = itemContainer.RemoveItem(itemAmount.Item.ID);
                oldItem.Destroy();
            }
        }
    }

    private void AddResults(IItemContainer itemContainer)
    {
        foreach (ItemAmount itemAmount in Results)
        {
            for (int i = 0; i < itemAmount.Amount; i++)
            {
                itemContainer.AddItem(itemAmount.Item.GetCopy());
            }
        }
    }
}
