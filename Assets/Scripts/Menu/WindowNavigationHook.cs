using System;
using UnityEngine.UI;

public class WindowNavigationHook : IDisposable
{
    private Button _button;
    private IViewWindow _nextWindow;
    private IViewWindow _currentWindow;

    public WindowNavigationHook(Button button, IViewWindow currentWindow, IViewWindow nextWindow)
    {
        _button = button;
        _nextWindow = nextWindow;
        _currentWindow = currentWindow;
    }

    public void Dispose()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        _currentWindow.Close();
        _nextWindow.Open();
    }
}
