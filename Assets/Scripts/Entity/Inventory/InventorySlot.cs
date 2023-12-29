using System;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public event Action<InventoryItem> ItemChanged;
    public int X => _position.x;
    public int Y => _position.y;
    public Vector2Int Position => _position;
    public InventoryItem Item => _item;

    private Inventory _inventory;
    private Vector2Int _position;
    private InventoryItem _item;

    public void Initialize(Inventory inventory, int positionX, int positionY)
    {
        _inventory = inventory;
        _position = new Vector2Int(positionX, positionY);
    }

    public void OnClicked()
    {
        _inventory.OnSlotClicked(this);
    }

    public bool IsEmpty() => !HasItem();

    public bool HasItem() => _item != null;

    public void RemoveItem()
    {
        SetItem(null);
    }

    public void SetItem(InventoryItem item)
    {
        _item = item;
        ItemChanged?.Invoke(_item);
    }
}