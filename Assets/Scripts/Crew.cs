using UnityEngine;

public class Crew : MonoBehaviour
{
    public int Balance => _balance;

    private int _balance;

    public bool CanBuy(int cost) => _balance >= cost;

    public void Buy(int cost)
    {
        _balance -= cost;
    }

    public void Earn(int amount)
    {
        _balance += amount;
    }
}
