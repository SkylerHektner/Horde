using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSettings/Settings", fileName = "GameData")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private Color exitLightColorUnlocked;
    [SerializeField] private Color exitLightEmissionColorUnlocked;
    [SerializeField] private Color exitLightColorLocked;
    [SerializeField] private Color exitLightEmissionColorLocked;

    public Color ExitLightColorUnlocked { get { return exitLightColorUnlocked; } }
    public Color ExitLightEmissionColorUnlocked { get { return exitLightEmissionColorUnlocked; } }
    public Color ExitLightColorLocked { get { return exitLightColorLocked; } }
    public Color ExitLightEmissionColorLocked { get { return exitLightEmissionColorLocked; } }
}
