using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    private ViewColorSampler _viewColorSampler;

    [SerializeField]
    private bool _useColorSampler = true;

    private GameColor _currentColor;
    private int _currentColorIndex = 0;
    private bool _foundColor;

    private float _gameTimer = 0.0f;
    private bool _gameOver = false;

    [SerializeField]
    private bool _runTimer = true;

    


    //Object detection
    [SerializeField]
    private bool _useMeshPainter = true;

    [SerializeField]
    private MeshPainter _meshPainter;
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
        //Detect Object
        if(_useMeshPainter)
        {
            _detectingObject = _meshPainter.DetectMesh();

            if(_detectingObject && !_foundObject)
            {
                OnBeginDetectObject.Invoke();
                _foundObject = true;
            }
            else if(!_detectingObject && _foundObject)
            {
                OnStopDetectObject.Invoke();
                if(!_gameOver)
                    OnKeepOldColor.Invoke(_currentColor);
                _foundObject = false;
            }
        }

        if(_gameOver)
            return;

        if(_runTimer)
            _gameTimer -= Time.deltaTime;

        // Check if the time for this color has passed
        if(_gameTimer <= 0)
        {
            //Debug.Log("Color timer expired");

                    
            OnGameEnd.Invoke(false);
            _gameOver = true;
        }

        
        if(_useColorSampler && !_detectingObject)
        {
            
            //Debug.Log("Searching color");
            Color sampledColor = 
                _viewColorSampler.CaptureColorOnScreen(0.5f,0.5f);

            bool matchesColor = _currentColor.IsColorInRange(sampledColor);
            // Use this bool to only send the efent once after finding the right
            // color
            //Debug.Log(matchesColor);
            //Debug.Log(_foundColor);
            if(matchesColor && !_foundColor)
            {
                OnCaptureColor.Invoke(sampledColor);
                
                _foundColor = true;

            }
            else if(!matchesColor && _foundColor)
            {
                _foundColor = false;
                OnKeepOldColor.Invoke(_currentColor);
            }
                
        }

        
        

    }

    public void StartGame()
    {
        _gameOver = false;
        _currentColorIndex = 0;
        _currentColor = gamePreset.ColorsToPaint[_currentColorIndex];
        if(!_viewColorSampler) //Try to get the sampler from this game object
            _viewColorSampler = GetComponent<ViewColorSampler>();
        if(!_viewColorSampler) // If still nothing, don't try to use it
            _useColorSampler = false;
        if(_viewColorSampler) //Clean the stored color list from past game
            _viewColorSampler.storedColors = new List<Color>();

        if(!_meshPainter) //Try to get the painter from this game object
            _meshPainter = GetComponent<MeshPainter>();
        if(!_meshPainter) // If still nothing, don't try to use it
            _useMeshPainter = false;
        OnGameStart.Invoke(gamePreset);
        OnSwitchColor.Invoke(_currentColor);

        _gameTimer = gamePreset.SecondsPerColor;
    }
    
    ///<summary>Tries to advance to the next color in the preset list
    ///</summary>
    ///<returns> True if successful, False if all colors have been advanced trough</returns>
    private bool AdvanceColor()
    {
        if(_currentColorIndex >= gamePreset.ColorsToPaint.Length - 1)
        {
            //All colors have been advanced
            return false;
        }
        
        
        _currentColorIndex ++;
        _currentColor = gamePreset.ColorsToPaint[_currentColorIndex];
        OnSwitchColor.Invoke(_currentColor);
        _gameTimer = gamePreset.SecondsPerColor;
        Debug.Log("Advanced to color " + _currentColor);
        return true;
    }

    private void ActOnColor()
    {
        _viewColorSampler.StoreColor();
        if(!AdvanceColor())
        {
            //If player has found all colors then win the game.
            OnGameEnd.Invoke(true);
            _gameOver = true;
        }

    }

    public void UpdatePainterBrush(int colorIndex) => _meshPainter._brushColor = _viewColorSampler.storedColors[colorIndex];

    public void ReactToAction()
    {
        if(_gameOver && !_detectingObject)
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
            
            //StartGame();
        else if(_detectingObject && _viewColorSampler.storedColors.Count > 0)
        {
            _meshPainter.PaintTriangle();
        }
        else if(_foundColor && !_gameOver)
            ActOnColor();
    }
}
