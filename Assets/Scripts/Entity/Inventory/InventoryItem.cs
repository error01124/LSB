using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public DroppedItem DroppedItem => _droppedItem;
    public InventorySlot ParentSlot => _parentSlot;
    public int Width => _size.x;
    public int Height => _size.y;
    public Vector2Int Size => _size;
    public bool IsDragging => _isDragging;

    [SerializeField] private DroppedItem _droppedItem;
    [SerializeField] private int _width = 1;
    [SerializeField] private int _height = 1;

    private InventorySlot _parentSlot;
    private Vector2Int _size;
    private bool _isDragging = false;
    private int _rotation = 0;

    private void Awake()
    {
        _size = new Vector2Int(_width, _height);    
    }

    public void DragToPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    public void Rotate()
    {
        _rotation += 90;
        _rotation = _rotation % 360;
        _size = new Vector2Int(_size.y, _size.x);
        transform.rotation = Quaternion.Euler(_rotation, 0, 0);
    }

    public void StartDrag()
    {
        _isDragging = true;
    }

    public void StopDrag()
    {
        _isDragging = false;
    }

    public void SetParentSlot(InventorySlot slot)
    {
        _parentSlot = slot;
        transform.SetParent(slot.transform);
    }

    public void Drop(Vector3 position)
    {
        _droppedItem.Put(position);
    }
}
