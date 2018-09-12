using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HInterface : MonoBehaviour {

	public enum HType
    {
        Seek
    }

    public static System.Type GetHeuristic(HType heuristic)
    {
        switch(heuristic)
        {
            case (HType.Seek):
                return typeof(H_Seek);
        }



        return null;
    }
}
