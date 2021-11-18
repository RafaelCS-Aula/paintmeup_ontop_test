using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PaintGame : MonoBehaviour
{

    public UnityEvent<PaintGamePresets> OnGameStart;
    public UnityEvent<bool> OnGameEnd;
    public UnityEvent<GameColor> OnSwitchColor;
    public UnityEvent<GameColor> OnKeepOldColor;

    public UnityEvent<Color> OnCaptureColor;

    public UnityEvent OnBeginDetectObject;
    public UnityEvent OnStopDetectObject;

    public PaintGamePresets gamePreset;

    [SerializeField]
    private bool _useColorSampler = true;

    private GameColor _currentColor;
    private int _currentColorIndex = 0;
    private bool _foundColor;

    private float _gameTimer = 0.0f;
    private bool _gameOver = false;

    [SerializeField]
    private bool _runTimer = true;

    [SerializeField]
    private ViewColorSampler _viewColorSampler;


    //Object detection
    [SerializeField] private LayerMask _detectableObjects;

    private bool _foundObject = false;
    private bool _detectingObject = false;
    

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
                {
                    _gameOver = true;
                    OnGameEnd.Invoke(false);
                }
                    

            }

        }

        
        if(_useColorSampler && !_detectingObject)
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
            else if(!matchesColor && _foundColor)
            {
                _foundColor = false;
                OnKeepOldColor.Invoke(_currentColor);
            }
                
        }

        //Detect Object
        RaycastHit hits;
        Ray centerRay = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f));
        _detectingObject = Physics.Raycast(centerRay,out hits, 2000,_detectableObjects);

        if(_detectingObject && !_foundObject)
        {
            OnBeginDetectObject.Invoke();
            _foundObject = true;
        }
        else if(!_detectingObject && _foundObject)
        {
            OnStopDetectObject.Invoke();
            _foundObject = false;
        }



    


    }

    public void StartGame()
    {
        _gameOver = true;
        _currentColorIndex = 0;
        _currentColor = gamePreset.ColorsToPaint[_currentColorIndex];
        if(!_viewColorSampler) //Try to get the sampler from this game object
            _viewColorSampler = GetComponent<ViewColorSampler>();
        if(!_viewColorSampler) // If still nothing, don't try to use it
            _useColorSampler = false;
        if(_viewColorSampler) //Clean the stored color list from past game
            _viewColorSampler.storedColors = new List<Color>();
        OnGameStart.Invoke(gamePreset);
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

    private void ColorFound()
    {
        _viewColorSampler.StoreColor();
        if(!AdvanceColor())
        {
            //If player has found all colors then win the game.
            OnGameEnd.Invoke(true);
            _gameOver = true;
        }

    }

    //Paint object logic
    private void PaintObject(){}
    public void ReactToAction()
    {
        if(_gameOver)
            StartGame();
        else if(_detectingObject)
            PaintObject();
        else if(_foundColor)
            ColorFound();
    }
}
