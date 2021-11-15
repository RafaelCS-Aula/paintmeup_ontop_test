using UnityEngine;

[CreateAssetMenu(fileName = "NewGameColor", menuName = "PaintMeUP/New Game Color", order = 0)]
public class GameColor : ScriptableObject 
{
    [SerializeField] private Gradient _colorRange;
    [SerializeField] private string _colorName;
    public string ColorName {get => _colorName;}

    //Give a color that represents the halfway of the gradient for
    // UI purposes.
    public Color DisplayColor {get => _colorRange.Evaluate(0.5f);}


}
