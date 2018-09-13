using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HInterface : MonoBehaviour
{

    public enum HType
    {
        Seek,
        NormalAttack,
        Explode
    }

    public static System.Type GetHeuristic(HType heuristic)
    {
        switch (heuristic)
        {
            case (HType.Seek):
                return typeof(H_Seek);
            case (HType.NormalAttack):
                return typeof(H_AttackNormal);
            case (HType.Explode):
                return typeof(H_Explode);
        }



        return null;
    }
}
