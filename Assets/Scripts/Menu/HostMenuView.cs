using UnityEngine;
using UnityEngine.UI;

public class HostMenuView : ViewWindow
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _backButton;

    private void Start()
    {
        AddHook(_backButton, _allWindows.HostMenu);
    }
}
