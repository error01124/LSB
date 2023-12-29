using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinCrewMenuView : ViewWindow
{
    [SerializeField] private TMP_InputField _idInputField;
    [SerializeField] private Button _joinCrewButton;
    [SerializeField] private Button _backButton;

    private JoinCrewMenu _model;

    private void Start()
    {
        AddHook(_backButton, _allWindows.JoinCrewMenu);
    }

    private void OnEnable()
    {
        _joinCrewButton.onClick.AddListener(OnJoinCrewButtonClicked);
    }

    private void OnDisable()
    {
        _joinCrewButton.onClick.RemoveListener(OnJoinCrewButtonClicked);
    }

    private void OnJoinCrewButtonClicked()
    {
        _model.OnIdReceived(_idInputField.text);
    }
}
