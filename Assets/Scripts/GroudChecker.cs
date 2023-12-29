using System;
using UnityEngine;

public class GroudChecker : MonoBehaviour
{
    public event Action<bool> GroundedChanged;

    private void OnTriggerEnter(Collider other)
    {
        GroundedChanged?.Invoke(true);
    }

    private void OnTriggerExit(Collider other)
    {
        GroundedChanged?.Invoke(false);
    }
}
