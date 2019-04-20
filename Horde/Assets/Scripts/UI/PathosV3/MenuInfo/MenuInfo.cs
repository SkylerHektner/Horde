using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName="UI/InventoryInfo")]
public class MenuInfo : ScriptableObject
{
    [Tooltip("Text to appear as the Title for this information")]
    public string TitleText;
    [Tooltip("Text that describes the purpose or idea of the information")]
    public string DescriptionText;
    [Tooltip("The text that expands store or lore regarding this information")]
    public string LoreText;
    [Tooltip("The Visual representation of this information (must be of 'VideoClip' or 'Image' type)")]
    public Object Visual;
}
