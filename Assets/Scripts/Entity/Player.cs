using UnityEngine;

public class Player : Entity
{
    public Inventory Inventory => _inventory;
    public float Stamina => _staminaScale.Points;

    [SerializeField] private Inventory _inventory;
    [SerializeField] private EntityScale _staminaScale;

    private void Update()
    {
        if (Stamina == 0)
        {
            Kill();
        }
    }

    public void ExpendStamina(float amount)
    {
        _staminaScale.Decrease(amount);
    }

    public override void Kill()
    {
        _staminaScale.Set(0);
        base.Kill();
        //_inventory.DropAll();
    }
}
