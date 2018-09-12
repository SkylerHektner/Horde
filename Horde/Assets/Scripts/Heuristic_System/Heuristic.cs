using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// I am the base class for all Heuristics. Inheret from me when making a heuristic. Make
/// sure you override Init, Update, and Resolve.
/// </summary>
[RequireComponent(typeof(Unit))]
public class Heuristic : MonoBehaviour
{
    protected Unit unit;

    virtual public void Init()
    {
        unit = GetComponent<Unit>();
    }

    virtual public void Execute()
    {

    }

    virtual public void Resolve()
    {
        unit.HResolved();
    }
	
}
