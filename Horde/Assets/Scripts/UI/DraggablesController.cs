using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggablesController : MonoBehaviour {

    public static DraggablesController Instance;

    public GameObject CurrentDraggable;

	// Use this for initialization
	void Start ()
    {
        Instance = this;
	}
}
