using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGame : MonoBehaviour
{
    public PaintGamePresets gamePreset;

    [SerializeField]
    private RenderTexture _viewRenderTexture;

    private GameColor _currentColor;
    private int _currentColorIndex = 0;


    //Testing
    public Material textureTestPlane;


    // Start is called before the first frame update
    void Start()
    {
        if(!gamePreset)
        {
            Debug.LogError("No game pre-set found");
            this.enabled = false;
        }    
        
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentColor = gamePreset.ColorsToPaint[_currentColorIndex];
        
        
        RenderTexture.active = _viewRenderTexture;
        
        Texture2D texture = new Texture2D(_viewRenderTexture.width,_viewRenderTexture.height);
        
        //Graphics.CopyTexture(_viewRenderTexture,texture);
        texture.ReadPixels(new Rect(0,0,texture.width,texture.height),0,0,false);
        texture.Apply(false);
        textureTestPlane.SetTexture("RenderToText",texture);
        textureTestPlane.mainTexture =  texture;
        
        

        Color textureColor = texture.GetPixel(texture.width/2,texture.height/2);
        // Matches the current color's range with the center of the view image
        bool matchesCurrentColor = _currentColor.IsColorInRange(textureColor);
        Debug.Log(textureColor);
        Debug.Log(matchesCurrentColor);
        RenderTexture.active = null;
    }

}
