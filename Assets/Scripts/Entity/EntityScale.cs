using UnityEngine;

public class EntityScale : MonoBehaviour
{
    public float Points => _points;

    [SerializeField] private float _maxPoints = 100;
    [SerializeField] private float _minPoints = 0;
    [SerializeField] private float _startPoints = 100;
    [SerializeField] private bool _canRegen = true;
    [SerializeField] private float _delayToRegeneration = 3;
    [SerializeField] private float _regenerationRate = 5;

    private float _points;
    private float _timeWithoutDecreasing = 0;

    private void Start()
    {
        _points = _startPoints;
        Limit();
    }

    private void Update()
    {
        _timeWithoutDecreasing += Time.deltaTime;

        if (CanRegen())
        {
            Regen();
        }
    }

    public bool CanRegen() => _canRegen && _points < _maxPoints && _timeWithoutDecreasing >= _delayToRegeneration;

    public void Regen()
    {
        _points += _regenerationRate;
        Limit();
    }

    public void Set(float amount)
    {
        _points = amount;
        Limit();
    }

    public void Decrease(float amount)
    {
        _timeWithoutDecreasing = 0;
        _points -= amount;
        Limit();
    }

    private void Limit()
    {
        _points = Mathf.Clamp(_points, _minPoints, _maxPoints);
    }
}
