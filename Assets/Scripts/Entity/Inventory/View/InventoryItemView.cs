using UnityEngine;

public class InventoryItemView : MonoBehaviour
{
    [SerializeField] private InventoryItem _item;
    [SerializeField] private GameObject _content;
    [SerializeField] private Sprite _focusedSlotContainerSprite;
    [SerializeField] private Sprite _unfocusedSlotContainerSprite;

    private GameObject _container;
    private SpriteRenderer _containerSpriteRenderer;

    private void Awake()
    {
        _container = gameObject;
        _containerSpriteRenderer = _container.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Unfocus();
    }

    public void Focus()
    {
        _containerSpriteRenderer.sprite = _focusedSlotContainerSprite;
    }

    public void Unfocus()
    {
        _containerSpriteRenderer.sprite = _unfocusedSlotContainerSprite;
    }
}
