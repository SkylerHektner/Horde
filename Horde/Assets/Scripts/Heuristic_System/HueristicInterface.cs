using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HInterface : MonoBehaviour
{

    public enum HType
    {
        Attack,
        Explode,
        Move,
        Wait,
        Beckon,
        Pickup,
        Hug,
        Scream
    }

    public static System.Type GetHeuristic(HType heuristic)
    {
        switch (heuristic)
        {
            case (HType.Attack):
                return typeof(H_Attack);
            case (HType.Explode):
                return typeof(H_Explode);
            case (HType.Move):
                return typeof(H_Move);
            case (HType.Wait):
                return typeof(H_Wait);
            case (HType.Beckon):
                return typeof(H_Beckon);
            case (HType.Pickup):
                return typeof(H_Pickup);
            case (HType.Hug):
                return typeof(H_Hug);
            case (HType.Scream):
                return typeof(H_Scream);
        }
        return null;
    }
}
