using UnityEngine;

[CreateAssetMenu(fileName = "New PaintGamePreset", menuName = "PaintMeUP/Paint Game Preset", order = 0)]
public class PaintGamePresets : ScriptableObject 
{

    [SerializeField] private int _secondsPerColor;
    public int SecondsPerColor {get => _secondsPerColor;}

    [SerializeField] private int _extraEndSeconds;
    public int ExtraEndSeconds {get => _extraEndSeconds;}

    public int GameDuration {get => _secondsPerColor * _colorsToPaint.Length + _extraEndSeconds;}

    [SerializeField] private GameColor[] _colorsToPaint;
    public GameColor[] ColorsToPaint {get => _colorsToPaint;}



}
