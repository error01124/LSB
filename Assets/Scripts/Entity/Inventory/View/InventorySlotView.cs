using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventorySlotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private InventorySlot _slot;
    [SerializeField] private Sprite _focusedEmptySlotContainerSprite;
    [SerializeField] private Sprite _unfocusedEmptySlotContainerSprite;

    private GameObject _container;
    private Image _containerImage;
    private Button _button;

    private void Awake()
    {
        _container = gameObject;
        _containerImage = _container.GetComponent<Image>();
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        Show();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClicked);
        _slot.ItemChanged += OnItemChanged;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClicked);
        _slot.ItemChanged -= OnItemChanged;
    }

    private void OnClicked()
    {
        _slot.OnClicked();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_slot.IsEmpty())
        {
            Focus();
        }
        else
        {
            FocusItem();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_slot.IsEmpty())
        {
            Unfocus();
        }
        else
        {
            UnfocusItem();
        }
    }

    private void Focus()
    {
        _containerImage.sprite = _focusedEmptySlotContainerSprite;
    }

    private void Unfocus()
    {
        _containerImage.sprite = _unfocusedEmptySlotContainerSprite;
    }

    private void OnItemChanged(InventoryItem item)
    {
        if (_slot.IsEmpty())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void FocusItem()
    {
        var item = _slot.Item;
        item.gameObject.GetComponent<InventoryItemView>().Focus();
    }

    private void UnfocusItem()
    {
        var item = _slot.Item;
        item.gameObject.GetComponent<InventoryItemView>().Unfocus();
    }

    private void Hide()
    {
        _containerImage.sprite = null;
    }

    private void Show()
    {
        Unfocus();
    }
}
