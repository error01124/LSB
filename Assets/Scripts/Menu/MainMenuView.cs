using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : ViewWindow
{
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _joinCrewButton;
    [SerializeField] private Button _leaveButton;
    [SerializeField] private HostMenuView _hostMenuView;
    [SerializeField] private JoinCrewMenuView _joinCrewMenuView;

    private void Start()
    {
        AddHook(_hostButton, _allWindows.HostMenu);
        AddHook(_joinCrewButton, _allWindows.JoinCrewMenu);
    }
}
