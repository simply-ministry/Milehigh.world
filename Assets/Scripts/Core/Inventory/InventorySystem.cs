// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A component that manages a collection of items for a character.
/// It handles adding, removing, and querying items in the inventory.
/// </summary>
public class InventorySystem : MonoBehaviour
{
    [Header("Inventory Settings")]
    [Tooltip("The maximum number of unique item stacks this inventory can hold.")]
    public int inventorySlots = 20;

    // The core data structure for the inventory. The Item is the template, the int is the quantity.
    private Dictionary<Item, int> items = new Dictionary<Item, int>();

    // Public property to allow other systems to safely view the inventory contents.
    public IReadOnlyDictionary<Item, int> Items => items;

    /// <summary>
    /// Adds a specified quantity of an item to the inventory.
    /// </summary>
    /// <param name="itemToAdd">The item asset to add.</param>
    /// <param name="quantity">The number of items to add.</param>
    /// <returns>True if the item was successfully added, false otherwise.</returns>
    public bool AddItem(Item itemToAdd, int quantity = 1)
    {
        if (itemToAdd == null || quantity <= 0)
        {
            Debug.LogWarning("[InventorySystem] Tried to add an invalid item or quantity.");
            return false;
        }

        // If the item is already in the inventory, try to add to the stack.
        if (items.ContainsKey(itemToAdd))
        {
            int newQuantity = items[itemToAdd] + quantity;
            // Respect the item's max stack size. For simplicity, we won't add if it overflows.
            // A more complex system might create a new stack.
            if (newQuantity <= itemToAdd.maxStackSize)
            {
                items[itemToAdd] = newQuantity;
                Debug.Log($"[InventorySystem] Added {quantity} {itemToAdd.itemName}(s). New total: {newQuantity}.");
                return true;
            }
            else
            {
                Debug.LogWarning($"[InventorySystem] Failed to add {itemToAdd.itemName}. Stack overflow.");
                return false;
            }
        }
        // If it's a new item, check if there's a free slot.
        else
        {
            if (items.Count < inventorySlots)
            {
                // Ensure the quantity being added doesn't exceed the stack size for a new entry.
                if (quantity <= itemToAdd.maxStackSize)
                {
                    items.Add(itemToAdd, quantity);
                    Debug.Log($"[InventorySystem] Added new item: {quantity} {itemToAdd.itemName}(s).");
                    return true;
                }
                else
                {
                    Debug.LogWarning($"[InventorySystem] Failed to add new item {itemToAdd.itemName}. Quantity exceeds max stack size.");
                    return false;
                }
            }
            else
            {
                Debug.LogWarning($"[InventorySystem] Failed to add {itemToAdd.itemName}. Inventory is full.");
                return false;
            }
        }
    }

    /// <summary>
    /// Removes a specified quantity of an item from the inventory.
    /// </summary>
    /// <param name="itemToRemove">The item asset to remove.</param>
    /// <param name="quantity">The number of items to remove.</param>
    public void RemoveItem(Item itemToRemove, int quantity = 1)
    {
        if (itemToRemove == null || quantity <= 0)
        {
            Debug.LogWarning("[InventorySystem] Tried to remove an invalid item or quantity.");
            return;
        }

        if (items.ContainsKey(itemToRemove))
        {
            int newQuantity = items[itemToRemove] - quantity;
            if (newQuantity > 0)
            {
                items[itemToRemove] = newQuantity;
                Debug.Log($"[InventorySystem] Removed {quantity} {itemToRemove.itemName}(s). {newQuantity} remaining.");
            }
            else
            {
                items.Remove(itemToRemove);
                Debug.Log($"[InventorySystem] Removed all {itemToRemove.itemName}(s) from inventory.");
            }
        }
        else
        {
            Debug.LogWarning($"[InventorySystem] Tried to remove {itemToRemove.itemName}, but it was not in the inventory.");
        }
    }

    /// <summary>
    /// Checks if the inventory contains a specific item.
    /// </summary>
    /// <param name="item">The item to check for.</param>
    /// <returns>True if the item exists in the inventory, false otherwise.</returns>
    public bool HasItem(Item item)
    {
        return items.ContainsKey(item);
    }

    /// <summary>
    /// Gets the quantity of a specific item in the inventory.
    /// </summary>
    /// <param name="item">The item to query.</param>
    /// <returns>The quantity of the item, or 0 if the item is not found.</returns>
    public int GetItemQuantity(Item item)
    {
        return items.TryGetValue(item, out int quantity) ? quantity : 0;
    }
}