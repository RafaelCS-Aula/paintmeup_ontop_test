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
            //Debug.Log("Evaluating against color: " + _colorRange.Evaluate(evalProgress) + "at step of the gradient: " + evalProgress);
            if(CompareColors(_colorRange.Evaluate(evalProgress),color))
                return true;

            evalProgress += evalStep;
            

        }while(evalProgress < 1.00f);
        return false;
    }

    private bool CompareColors(Color a, Color b)
    {
        if(Math.Round(a.r) != Math.Round(b.r))
            return false;
        if(Math.Round(a.g) != Math.Round(b.g))
            return false;
        if(Math.Round(a.b) != Math.Round(b.b))
            return false;
        return true;
    }



}
