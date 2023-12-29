using Mirror;
using UnityEngine;

public class PlayerInteraction : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _distance = 2;
    [SerializeField] private KeyCode _interactionKey = KeyCode.E;

    private Player _player;
    private Interactable _previousInteractable;

    private void Start()
    {
        _player = GetComponent<Player>();   
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        var objectInView = FindObjectInView();
        var interactable = objectInView?.GetComponent<Interactable>();

        if (_previousInteractable != null)
        {
            if (_previousInteractable.IsFocused)
                _previousInteractable.Defocus();
        }

        if (interactable != null)
        {
            interactable.Focus();

            if (Input.GetKeyDown(_interactionKey))
                Interact(interactable);

            _previousInteractable = interactable;
        }
    }

    private GameObject FindObjectInView()
    {
        RaycastHit raycastHit;
        Vector3 origin = _camera.position;
        Vector3 direction = _camera.TransformDirection(Vector3.forward);

        if (Physics.Raycast(origin, direction, out raycastHit, _distance))
            return raycastHit.collider.gameObject;

        return null;
    }

    private void Interact(Interactable interactable)
    {
        interactable.Interact(_player);
    }
}
