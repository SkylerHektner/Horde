using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInfoUI : MonoBehaviour {
    [SerializeField]
    [Header("ABILITY INFO")]
    [Tooltip("The UIText object where the ability name goes")]
    public Text NameText;
    [Tooltip("The UIText object where the ability description goes")]
    public Text DescriptionText;
}
