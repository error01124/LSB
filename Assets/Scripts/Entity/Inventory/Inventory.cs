using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action Opened;
    public event Action Closed;
    public event Action<InventorySlot[,]> SlotsInstanciated;
    public bool IsOpened => _isOpened;
    public int Height => _height;
    public int Width => _width;

    [SerializeField] private InventorySlotsFinder _slotsFinder;
    [SerializeField] private InventoryItemDragger _itemDragger;
    [SerializeField] private ItemDropper _itemDropper;
    [SerializeField] private int _height = 4;
    [SerializeField] private int _width = 4;

    private bool _isOpened = false;
    private InventorySlot[,] _slots;

    public void OnSlotsInstantiated(GameObject[,] slotsObjects)
    {
        _slots = new InventorySlot[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var slot = slotsObjects[x, y].GetComponent<InventorySlot>();
                slot.Initialize(this, x, y);
                _slots[x, y] = slot;
            }
        }

        SlotsInstanciated?.Invoke(_slots);
    }

    public void Open()
    {
        _isOpened = true;
        Debug.Log("open 2");
        Opened?.Invoke();
    }

    public void Close()
    {
        _isOpened = false;
        Closed?.Invoke();
    }

    public bool TryAddItem(InventoryItem item)
    {
        var slot = _slotsFinder.FindFreeSlotToPutItem(item);

        if (!IsSlotExist(slot))
        {
            item.Rotate();
            slot = _slotsFinder.FindFreeSlotToPutItem(item);
        }

        if (IsSlotExist(slot))
        {
            SetItem(item, slot);
            return true;
        }

        return false;
    }

    private bool IsSlotExist(InventorySlot slot)
    {
        return slot != null;
    }

    private void SetItem(InventoryItem item, InventorySlot slot)
    {
        var startSlot = slot;
        var endSlot = _slotsFinder.FindEndSlotForItem(startSlot, item);
        var slots = _slotsFinder.FindSlotsInArea(startSlot, endSlot);
        item.SetParentSlot(slot);

        foreach (var itemSlot in slots)
        {
            itemSlot.SetItem(item);
        }
    }

    private void RemoveItemFromSlot(InventorySlot slot)
    {
        SetItem(null, slot);
    }

    public void DropItemFromSlot(InventorySlot slot)
    {
        _itemDropper.Drop(slot.Item);
        RemoveItemFromSlot(slot);
    }

    public void DropAll()
    {
        for (int slotX = 0; slotX < Width; slotX++)
        {
            for (int slotY = 0; slotY < Height; slotY++)
            {
                var slot = _slots[slotX, slotY];

                if (slot.HasItem())
                {
                    DropItemFromSlot(slot);
                }
            }
        }
    }

    public void OnSlotClicked(InventorySlot slot)
    {
        if (_itemDragger.IsAnyItemDrag())
        {
            if (!slot.HasItem())
            {
                _itemDragger.PutItem(slot);
            }
        }
        else
        {
            if (slot.HasItem())
            {
                _itemDragger.TakeItem(slot);
            }
        }
    }

    public void OnDropAreaClicked()
    {
        if (!_itemDragger.IsAnyItemDrag())
        {
            _itemDragger.DropItem();
        }
    }
}

