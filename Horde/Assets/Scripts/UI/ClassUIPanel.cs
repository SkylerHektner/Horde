using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassUIPanel : MonoBehaviour {

    [SerializeField]
    private Text classNameText;

    public List<HInterface.HType> Heuristics = new List<HInterface.HType>();

    public void Init(string className)
    {
        classNameText.text = className;
    }
}
