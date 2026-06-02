using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;

public class BackpackUI : MonoBehaviour
{
    [Header("Backpack things")]
    public ItemHandler[] backpack;
    public FPlayer player;
    public TMPro.TMP_Text total_weight;

    //private Transform playerTransform; // Player transform pos
    //private BoxInventory boxInventory; // Reference to the box's inventory script (for box interactions)
    //public List<ItemHandler> groundItems = new List<ItemHandler>(); // Public list of items on the ground (tags required)
    //private ItemHandler currentGroundItem; // Current item the player is colliding with
    //private bool isTouchingBox; // Flag for touching a box
    [Header("UI things")]
    public Item currentItem = null;
    private void Start()
    {
        StartCoroutine(SetBackpack());
    }

    IEnumerator SetBackpack()
    {
        yield return StaticHolder.waitFrame;
        player.backpack[0] = Math.Clamp(TotalWeight(), (short)0, short.MaxValue);
        player.backpack[^1] = Math.Clamp((short)(StaticHolder.backpackBase * (SaveManager.instance.activeSave.backpackLvl + 1)), (short)0, short.MaxValue);

        //backpack[0].UpdateItem(player.knife);
        backpack[1].UpdateItem(player.gun);
    }

    public void UpdateItems()   
    {
        foreach (var handler in backpack)
            handler.UpdateIcon();
    }

    public short TotalWeight()   
    {
        short total = 0;
        foreach (var handler in backpack)
            if (handler != null && handler.itemSelf != null)
                total += handler.itemSelf.CalculateWeight();
        return total;
    }

    public bool AddItem(Item itemToAdd)
    {
        if (itemToAdd == null) return false;
        short maxWeight = player.backpack[1];
        if (TotalWeight() + itemToAdd.CalculateWeight() < maxWeight) return false;

        // Find the last empty slot (start from the end)
        for (int i = backpack.Length - 1; i >= 0; i--)      
        {
            Debug.Log($"Im searching in {i}");
            if (backpack[i] == null || backpack[i].itemSelf == null || backpack[i].itemSelf.Quantity[0] == 0)
            {
                backpack[i].UpdateItem(itemToAdd);
                UpdateItems();
                player.backpack[0] = (short)TotalWeight();
                return true;
            }
        }

        Debug.LogWarning("Cannot add item: backpack is full!");
        return false;
    }

    // Deletes an item by reference
    public void DeleteItem(Item itemToDelete)       
    {
        for (byte i = 0; i < backpack.Length; i++)
        {
            if (backpack[i] != null && backpack[i].itemSelf == itemToDelete)
            {
                backpack[i].itemSelf = null;
                return;
            }
        }
        Debug.LogWarning("Item not found in backpack!");
    }

    // Deletes an item by index (handles throwing to box or ground)
    //public void DeleteItem(byte index)  
    //{
    //    if (index >= backpack.Length || backpack[index] == null || backpack[index].itemSelf == null)
    //    {
    //        Debug.LogWarning("Invalid index or no item in slot!");
    //        return;
    //    }

    //    Item itemToDelete = backpack[index].itemSelf;

    //    // Throw to box if touching box (assumed triggered by mouse click on item in UI)
    //    if (isTouchingBox && boxInventory != null)
    //    {
    //        boxInventory.AddItem(itemToDelete); // Add to box inventory (TODO script)
    //    }
    //    // Throw to (triggered by "throw" button in inventory UI)
    //    else
    //    {
    //        if (itemPrefabs.ContainsKey(itemToDelete.name))
    //        {
    //            GameObject prefab = itemPrefabs[itemToDelete.name];
    //            Vector3 spawnPosition = playerTransform != null ? playerTransform.position : Vector3.zero;
    //            Instantiate(prefab, spawnPosition, Quaternion.identity);
    //            groundItems.Add(itemToDelete); // Add to ground items list
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"No prefab found for item: {itemToDelete.name}");
    //        }
    //    }

    //    // Now delete thrown item from inventory
    //    backpack[index].itemSelf = null;

    //    // If not the last slot, move the last non-empty item to this position
    //    if (index < backpack.Length - 1)
    //    {
    //        int lastIndex = -1;
    //        for (int i = backpack.Length - 1; i > index; i--)
    //        {
    //            if (backpack[i] != null && backpack[i].itemSelf != null && backpack[i].itemSelf.quantity[0] > 0)
    //            {
    //                lastIndex = i;
    //                break;
    //            }
    //        }
    //        if (lastIndex != -1)
    //        {
    //            backpack[index].itemSelf = backpack[lastIndex].itemSelf;
    //            backpack[lastIndex].itemSelf = null;
    //        }
    //    }

    //    UpdateItems();
    //}

}