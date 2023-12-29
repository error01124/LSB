using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    private Inventory _inventory;

    public void Drop(InventoryItem item)
    {
        item.Drop(_inventory.transform.position);
    }
}
