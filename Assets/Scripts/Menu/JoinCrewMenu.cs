using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinCrewMenu : MonoBehaviour
{
    public event Action<ConnectionResult> ConnectionResultReceived;

    public void OnIdReceived(string text)
    {
        ConnectionResultReceived?.Invoke(ConnectionResult.Success);
    }

    public enum ConnectionResult
    {
        Success,
        Failure
    }
}
