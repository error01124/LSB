using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewWindow : MonoBehaviour, IViewWindow
{
    [SerializeField] protected AllWindows _allWindows;

    protected List<WindowNavigationHook> _navigationHooks = new List<WindowNavigationHook>();

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    protected void AddHook(Button button, IViewWindow window)
    {
        _navigationHooks.Add(new WindowNavigationHook(button, this, window));
    }
}
