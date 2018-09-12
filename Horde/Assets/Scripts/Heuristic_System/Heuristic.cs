using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// I am the base class for all Heuristics. Inheret from me when making a heuristic. Make
/// sure you override Init, Update, and Resolve.
/// </summary>
abstract public class Heuristic : MonoBehaviour
{

    abstract public void Init();

    abstract public void Execute();

    abstract public void Resolve();
	
}
