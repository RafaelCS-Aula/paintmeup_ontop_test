using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewGameColor", menuName = "PaintMeUP/New Game Color", order = 0)]
public class GameColor : ScriptableObject 
{
    [SerializeField] private Gradient _colorRange;
    [SerializeField] private string _colorName;
    public string ColorName {get => _colorName;}

    //Give a color that represents the halfway of the gradient for
    // UI purposes.
    public Color DisplayColor {get => _colorRange.Evaluate(0.5f);}

    public bool IsColorInRange(Color color)
    {
        float evalStep = 0.05f;
        float evalProgress = 0;

        do
        {
            Debug.Log("Evaluating against color: " + _colorRange.Evaluate(evalProgress) + "at step of the gradient: " + evalProgress);
            if(CompareColors(_colorRange.Evaluate(evalProgress),color))
                return true;

            evalProgress += evalStep;
            

        }while(evalProgress < 1.00f + evalStep);
        return false;
    }

    private bool CompareColors(Color a, Color b)
    {
        if(Math.Round(a.r,1) != Math.Round(b.r,1))
            return false;
        if(Math.Round(a.g,1) != Math.Round(b.g,1))
            return false;
        if(Math.Round(a.b,1) != Math.Round(b.b,1))
            return false;
        return true;
    }



}
