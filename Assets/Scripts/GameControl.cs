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

    private bool _gameFinished = false;
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
        _gameFinished = false;
        _timerValue = gamePreset.GameDuration;
    }
    private void UpdateTimer()
    {
        if(_gameFinished)
            return;
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
    public void ActivatePaintButton()
    {
        _actionButton.SetEnabled(true);
        _actionButton.text  = "Paint!";
        _actionButton.style.color = Color.white;

        
    }
    public void DisablePaintButton()
    {
        _actionButton.SetEnabled(false);
        _actionButton.text  = "Nothing to Paint!";
        _actionButton.style.color = Color.white;

        
    }
    public void DisableActionButton(GameColor wantedColor)
    {
        _actionButton.SetEnabled(false);
        _actionButton.text  = $"Find {wantedColor.ColorName}";
        _actionButton.style.color = wantedColor.DisplayColor;

        _timerLabel.style.color = wantedColor.DisplayColor;
    }

    public void ChangeUIToEnding(bool victory)
    {
        if(victory)
            _timerLabel.text = "Good Job!";
        else
            _timerLabel.text = "Bad Job!";

        _timerLabel.style.color = Color.white;
        _gameFinished = true;
    }
    private void Update() {
        UpdateTimer();
    }
}
