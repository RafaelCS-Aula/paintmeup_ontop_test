using UnityEngine;

public class ViewColorSampler : MonoBehaviour 
{
    [SerializeField] private RenderTexture _viewRenderTexture;
    public Color CaptureColorOnScreen(float x, float y)
    {
        x = Mathf.Clamp01(x);
        y = Mathf.Clamp01(y);
         // Get the image on screen and put it to a Texture2D to compare colors
        RenderTexture.active = _viewRenderTexture;
        
        Texture2D texture = new Texture2D(_viewRenderTexture.width,_viewRenderTexture.height);
        
        texture.ReadPixels(new Rect(0,0,texture.width,texture.height),0,0,false);
        texture.Apply(false);
        //textureTestPlane.SetTexture("RenderToText",texture);
        //textureTestPlane.mainTexture =  texture;
        
        // Matches the current color's range with the center of the view image
        Color textureColor = texture.GetPixel(
        (int)(texture.width * x),
        (int)(texture.height * y ));
        RenderTexture.active = null;
        Debug.Log("Pixel color in view: " + textureColor);
        return textureColor;
        
     
        
       
    }

}