using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

[RequireComponent(typeof(UIDocument))]
public class GameControl : MonoBehaviour
{
    
    private Button _actionButton;
    private Label _timerLabel;

    private float _timerValue;
    public UnityEvent OnActionPressed;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        var uiRoot = GetComponent<UIDocument>().rootVisualElement;
        _actionButton = uiRoot.Q<Button>("action-button");
        _actionButton.SetEnabled(false);
        _actionButton.clicked += () => OnActionPressed.Invoke();

        _timerLabel = uiRoot.Q<Label>("timer-label");    
    }
    public void StartTimer(PaintGamePresets gamePreset)
    {
        _timerValue = gamePreset.GameDuration;
    }
    private void UpdateTimer()
    {
        _timerLabel.text = ((int)_timerValue).ToString();
        
        if(_timerValue>0)
            _timerValue -= Time.deltaTime;

    }
    public void ActivateActionButton(Color textColor)
    {
        _actionButton.SetEnabled(true);
        _actionButton.text  = "That's good enough, tap me!";
        _actionButton.style.color = textColor;

        _timerLabel.style.color = textColor;
    }
    public void DisableActionButton(GameColor wantedColor)
    {
        _actionButton.SetEnabled(false);
        _actionButton.text  = $"Find {wantedColor.ColorName}";
        _actionButton.style.color = wantedColor.DisplayColor;

        _timerLabel.style.color = wantedColor.DisplayColor;
    }

    private void Update() {
        UpdateTimer();
    }
}
