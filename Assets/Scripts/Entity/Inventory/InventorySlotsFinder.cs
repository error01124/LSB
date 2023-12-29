using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySlotsFinder : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    private InventorySlot[,] _slots;
    private int _width;
    private int _height;

    private void Awake()
    {
        _width = _inventory.Width;
        _height = _inventory.Height;
    }

    private void OnEnable()
    {
        _inventory.SlotsInstanciated += OnSlotsInstantiated;
    }

    private void OnDisable()
    {
        _inventory.SlotsInstanciated -= OnSlotsInstantiated;
    }

    private void OnSlotsInstantiated(InventorySlot[,] slots)
    {
        _slots = slots;
    }

    public InventorySlot FindFreeSlotToPutItem(InventoryItem item)
    {
        bool isItemFits;

        for (int slotX = 0; slotX < _width - item.Width; slotX++)
        {
            for (int slotY = 0; slotY < _height - item.Height; slotY++)
            {
                var slot = _slots[slotX, slotY];
                isItemFits = IsSlotFitsToKeepItem(slot, item);

                if (isItemFits)
                    return slot;
            }
        }

        return null;
    }

    public bool IsSlotFitsToKeepItem(InventorySlot slot, InventoryItem item)
    {
        var startSlot = slot;
        var endSlot = FindEndSlotForItem(startSlot, item);
        return FindSlotsInArea(startSlot, endSlot).All(x => x == null);
    }

    public InventorySlot FindEndSlotForItem(InventorySlot start, InventoryItem item)
    {
        return _slots[start.X + item.Width, start.Y + item.Height];
    }

    public IEnumerable<InventorySlot> FindSlotsInArea(InventorySlot start, InventorySlot end)
    {
        for (int x = start.X; x <= end.X; x++)
        {
            for (int y = start.Y; y <= end.Y; y++)
            {
                yield return _slots[x, y];
            }
        }
    }
}
