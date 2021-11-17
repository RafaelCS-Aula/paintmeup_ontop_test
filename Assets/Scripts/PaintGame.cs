using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PaintGame : MonoBehaviour
{

    public UnityEvent OnGameStart;
    public UnityEvent OnGameEnd;
    public UnityEvent<GameColor> OnSwitchColor;

    public UnityEvent<Color> OnCaptureColor;

    public PaintGamePresets gamePreset;

    [SerializeField]
    private bool _useColorSampler = true;

    private GameColor _currentColor;
    private int _currentColorIndex = 0;
    private bool _foundColor;

    private float _gameTimer = 0.0f;

    [SerializeField]
    private bool _runTimer = true;

    [SerializeField]
    private ViewColorSampler _viewColorSampler;


    // Start is called before the first frame update
    void Start()
    {
        if(!gamePreset)
        {
            Debug.LogError("No game pre-set found");
            this.enabled = false;
        }    
        
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(_runTimer)
            _gameTimer += Time.deltaTime;

        // Check if the time for this color has passed
        if(Mathf.Round(_gameTimer) % Mathf.Round(gamePreset.SecondsPerColor) == 0)
        {
            if(!AdvanceColor())
            {
                if(_gameTimer >= gamePreset.GameDuration)
                    OnGameEnd.Invoke();

            }

        }

              
        if(_useColorSampler)
        {
            
            Color sampledColor = 
                _viewColorSampler.CaptureColorOnScreen(0.5f,0.5f);

            bool matchesColor = _currentColor.IsColorInRange(sampledColor);
            // Use this bool to only send the efent once after finding the right
            // color
            
            if(matchesColor && !_foundColor)
            {
                OnCaptureColor.Invoke(sampledColor);
                Debug.Log(matchesColor);
                _foundColor = true;

            }
            else if(!matchesColor)
                _foundColor = false;
        }


    }

    public void StartGame()
    {
        _currentColorIndex = 0;
        _currentColor = gamePreset.ColorsToPaint[_currentColorIndex];
        if(!_viewColorSampler) //Try to get the sampler from this game object
            _viewColorSampler = GetComponent<ViewColorSampler>();
        if(!_viewColorSampler) // If still nothing, don't try to use it
            _useColorSampler = false;

        OnGameStart.Invoke();
        OnSwitchColor.Invoke(_currentColor);
    }
    
    ///<summary>Tries to advance to the next color in the preset list
    ///</summary>
    ///<returns> True if successful, False if all colors have been advanced trough</returns>
    private bool AdvanceColor()
    {
        if(_currentColorIndex == gamePreset.ColorsToPaint.Length - 1)
        {
            //All colors have been advanced
            return false;
        }
        
        _currentColorIndex ++;
        _currentColor = gamePreset.ColorsToPaint[_currentColorIndex];
        OnSwitchColor.Invoke(_currentColor);
        return true;
    }

}
