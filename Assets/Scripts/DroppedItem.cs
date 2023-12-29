using UnityEngine;

public class DroppedItem : Interactable
{
    public InventoryItem IneventoryItem => _inventoryItem;

    [SerializeField] private InventoryItem _inventoryItem;

    public override void Interact(Player player)
    {
        Pickup(player);
    }

    private void Pickup(Player player)
    {
        if (player.Inventory.TryAddItem(_inventoryItem))
        {
            gameObject.SetActive(false);
        }
    }

    public void Put(Vector3 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
    }
}
