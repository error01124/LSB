using UnityEngine;

public class AllWindows : MonoBehaviour
{
    public IViewWindow MainMenu => _mainMenu;
    public IViewWindow HostMenu => _hostMenu;
    public IViewWindow JoinCrewMenu => _joinCrewMenu;

    [SerializeField] private ViewWindow _hostMenu;
    [SerializeField] private ViewWindow _mainMenu;
    [SerializeField] private ViewWindow _joinCrewMenu;
}
