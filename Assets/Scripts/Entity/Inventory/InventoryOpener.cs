using UnityEngine;

public class InventoryOpener : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private KeyCode _openKey;

    private void Update()
    {
        if (Input.GetKeyDown(_openKey))
        {
            Debug.Log("open");
            if (_inventory.IsOpened)
            {
                _inventory.Close();
            }
            else
            {
                _inventory.Open();
            }
        }
    }
}
