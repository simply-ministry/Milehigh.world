using UnityEngine;

/// <summary>
/// Defines the type of an item, which can influence how it's used or displayed.
/// </summary>
public enum ItemType
{
    Generic,
    Quest,
    Consumable,
    Material,
    Equipment
}

/// <summary>
/// A ScriptableObject representing a generic item in the game.
/// Use this as a base to create specific item assets in the Unity Editor.
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Milehigh.World/Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Item Identification")]
    [Tooltip("The name of the item as it will appear in the UI.")]
    public string itemName = "New Item";

    [Tooltip("A unique identifier for this item type. Can be used for database lookups or specific logic.")]
    public string itemID;

    [TextArea(3, 10)]
    [Tooltip("The description of the item shown to the player.")]
    public string description = "Item Description";

    [Tooltip("The icon that represents the item in the UI.")]
    public Sprite icon;

    [Header("Item Properties")]
    [Tooltip("The type of the item, which determines its basic function.")]
    public ItemType itemType = ItemType.Generic;

    [Tooltip("The maximum number of this item that can be held in a single stack.")]
    public int maxStackSize = 99;

    [Tooltip("Is this item unique, meaning the player can only ever have one?")]
    public bool isUnique = false;

    /// <summary>
    /// A virtual method to define what happens when the item is used.
    /// This should be overridden by more specific item types (e.g., a Health Potion).
    /// </summary>
    /// <param name="user">The character who is using the item.</param>
    public virtual void Use(Character user)
    {
        Debug.Log($"Using {itemName} on {user.characterName}. No specific action defined for this item type.");
    }
}