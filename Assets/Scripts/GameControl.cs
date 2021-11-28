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

    private GroupBox _colorSelectGroup;

    private float _timerValue;
    private float _colorTime;

    private bool _gamePaused = false;
    public UnityEvent OnActionPressed;

    public UnityEvent<int> OnColorButtonPressed;

    private Button[] _colorSelectButtons;
    private Button _colorSelectActiveButton = null;

    // Start is called before the first frame update
    void OnEnable()
    {
        var uiRoot = GetComponent<UIDocument>().rootVisualElement;
        _actionButton = uiRoot.Q<Button>("action-button");
        _actionButton.SetEnabled(false);
        _actionButton.clicked += () => OnActionPressed.Invoke();

        _timerLabel = uiRoot.Q<Label>("timer-label");    

        _colorSelectGroup = uiRoot.Q<GroupBox>("color-select-group");
        //_gamePaused = true;
    }
    public void StartTimer(PaintGamePresets gamePreset)
    {
        _gamePaused = false;
        _colorTime = gamePreset.SecondsPerColor;
        _timerValue = _colorTime + 1;
        _colorSelectButtons = new Button[gamePreset.ColorsToPaint.Length];
        for (int i = 0; i < gamePreset.ColorsToPaint.Length; i++)
        {   
            Button colorbtn = new Button();
            _colorSelectGroup.Add(colorbtn);
            colorbtn.name = $"color-select-button-{i}";
            colorbtn.text = "";
            colorbtn.style.backgroundColor = gamePreset.ColorsToPaint[i].DisplayColor;
            colorbtn.SetEnabled(false);
            int index = i;
            colorbtn.clicked += () => SelectColor(index);
;            _colorSelectButtons[i] = colorbtn;
            Debug.Log(_colorSelectButtons[i] + "index: " + i);
        }

        DisableActionButton(gamePreset.ColorsToPaint[0]);
    }
    private void UpdateTimer()
    {
        
        _timerLabel.text = ((int)_timerValue).ToString();
        
        if(_timerValue>0)
            _timerValue -= Time.deltaTime;

    }

    public void SelectColor(int index)
    {
        Debug.Log("Selecting color of index: " + index);
        if(_colorSelectActiveButton != null)
            _colorSelectActiveButton.style.borderLeftWidth = 1;

        _colorSelectButtons[index].style.borderLeftWidth = 6;
        OnColorButtonPressed.Invoke(index);
        _colorSelectActiveButton = _colorSelectButtons[index];
    }

    public void ActivateColorButton(Color realColor, int btnIndex)
    {
        _colorSelectButtons[btnIndex].SetEnabled(true);
        _colorSelectButtons[btnIndex].style.backgroundColor = realColor;
        SelectColor(btnIndex);

        //A new color is to be caught, so we put the timer back
        _timerValue = _colorTime;
    }

    public void ActivateActionButton(Color textColor)
    {
        _actionButton.SetEnabled(true);
        _actionButton.text  = "Capture Color!";
        _actionButton.style.color = textColor;

        _timerLabel.style.color = textColor;
    }
    public void ActivatePaintButton()
    {
        _actionButton.SetEnabled(true);
        _actionButton.text  = "Paint!";
        _actionButton.style.color = Color.black;

        
    }
    public void DisablePaintButton()
    {
        if(!_gamePaused)
        {
            _actionButton.SetEnabled(false);
            _actionButton.text  = "Nothing to Paint!";
            _actionButton.style.color = Color.white;
        }
        else
        {
            _actionButton.text  = "Restart!";
            _actionButton.style.color = Color.white;
        }
        

        
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
        _gamePaused = true;
        if(victory)
            _timerLabel.text = "Good Job!";
        else
            _timerLabel.text = "Bad Job!";

        _timerLabel.style.color = Color.white;
        _actionButton.text = "Restart!";
        _actionButton.SetEnabled(true);
        
    }

    

    private void Update() {
        if(!_gamePaused)
            UpdateTimer();
    }
}
