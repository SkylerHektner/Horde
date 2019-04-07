using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSettings/Settings", fileName = "GameData")]
public class GameSettings : ScriptableObject
{
    [ColorUsage(true, true)]
    [SerializeField] private Color exitLightColorUnlocked;
    [ColorUsage(true, true)]
    [SerializeField] private Color exitLightEmissionColorUnlocked;
    [ColorUsage(true, true)]
    [SerializeField] private Color exitLightColorLocked;
    [ColorUsage(true, true)]
    [SerializeField] private Color exitLightEmissionColorLocked;

    public Color ExitLightColorUnlocked { get { return exitLightColorUnlocked; } }
    public Color ExitLightEmissionColorUnlocked { get { return exitLightEmissionColorUnlocked; } }
    public Color ExitLightColorLocked { get { return exitLightColorLocked; } }
    public Color ExitLightEmissionColorLocked { get { return exitLightEmissionColorLocked; } }
}
