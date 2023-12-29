using UnityEngine;

public class InventoryItemDragger : MonoBehaviour
{
    [SerializeField] private InventorySlotsFinder _slotFinder;
    [SerializeField] private KeyCode _rotateKey;

    private InventoryItem _dragItem;

    private void Update()
    {
        if (IsAnyItemDrag())
        {
            DoDrag();

            if (Input.GetKeyDown(_rotateKey))
            {
                _dragItem.Rotate();
            }
        }
    }

    public bool IsAnyItemDrag()
    {
        return _dragItem != null;
    }

    private void DoDrag()
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _dragItem.DragToPosition(position);
    }

    public bool CanTakeItem(InventorySlot slot)
    {
        return slot.HasItem();
    }

    public void TakeItem(InventorySlot slot)
    {
        _dragItem = slot.Item;
        var startSlot = _dragItem.ParentSlot;
        var endSlot = _slotFinder.FindEndSlotForItem(startSlot, _dragItem);
        var slots = _slotFinder.FindSlotsInArea(startSlot, endSlot);

        foreach (var itemSlot in slots)
        {
            itemSlot.RemoveItem();
        }

        _dragItem.SetParentSlot(null);
        _dragItem.StartDrag();
    }

    public void PutItem(InventorySlot slot)
    {
        _dragItem.StopDrag();
        var startSlot = slot;
        var endSlot = _slotFinder.FindEndSlotForItem(startSlot, _dragItem);
        var slots = _slotFinder.FindSlotsInArea(startSlot, endSlot);

        foreach (var itemSlot in slots)
        {
            itemSlot.SetItem(_dragItem);
        }

        _dragItem.SetParentSlot(startSlot);
        _dragItem = null;
    }

    public void DropItem()
    {
        _dragItem.Remove();
    }
}
