using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Button _dropAreaButton;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _slotGroup;
    [SerializeField] private GameObject _slot;

    private GameObject[,] _slots;

    private void Awake()
    {
        InstantiateSlots();
    }

    private void Start()
    {
        if (!_inventory.IsOpened)
        {
            OnClosed();
        }
    }

    private void OnEnable()
    {
        _inventory.Opened += OnOpened;
        _inventory.Closed += OnClosed;
        _dropAreaButton.onClick.AddListener(OnDropAreaClicked);
    }

    private void OnDisable()
    {
        _inventory.Opened -= OnOpened;
        _inventory.Closed -= OnClosed;
        _dropAreaButton.onClick.RemoveListener(OnDropAreaClicked);
    }

    private void InstantiateSlots()
    {
        int width = _inventory.Width;
        int height = _inventory.Height;
        _slots = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var slotCopied = Instantiate(_slot, _slotGroup.transform);
                _slots[x, y] = slotCopied;
            }
        }

        _inventory.OnSlotsInstantiated(_slots);
    }

    private void OnOpened()
    {
        Debug.Log("open 3");
        _inventoryPanel.SetActive(true);
    }

    private void OnClosed()
    {
        _inventoryPanel.SetActive(false);
    }

    private void OnDropAreaClicked()
    {
        _inventory.OnDropAreaClicked();
    }
}
