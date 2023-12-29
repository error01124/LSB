using Mirror;
using UnityEngine;

public class Entity : NetworkBehaviour
{
    public float Health => _healthScale.Points;

    [SerializeField] protected EntityScale _healthScale;

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Health == 0)
        {
            Kill();
        }
    }

    public void Damage(float amount)
    {
        _healthScale.Decrease(amount);
    }

    public virtual void Kill()
    {
        _healthScale.Set(0);
        Destroy(gameObject);
    }
}
