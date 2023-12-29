using Mirror;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class Interactable : NetworkBehaviour
{
    public bool IsFocused => _isFocused;

    protected Outline _outline;
    protected bool _isFocused;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    private void Update()
    {

    }

    public abstract void Interact(Player player);

    public void Focus()
    {
        _isFocused = true;
        _outline.enabled = true;
    }

    public void Defocus() 
    {
        _isFocused = false;
        _outline.enabled = false; 
    }
}
