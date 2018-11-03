using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HInterface : MonoBehaviour
{

    public enum HType
    {
        TargetNearestEnemy,
        TargetWeakestEnemy,
        TargetNearestAlly,
        TargetWeakestAlly,
        WatchForEnemy,
        Attack,
        Mutilate,
        Explode,
        Heal,
        Move
    }

    public static System.Type GetHeuristic(HType heuristic)
    {
        switch (heuristic)
        {
            case (HType.TargetNearestEnemy):
                return typeof(H_TargetNearestEnemy);
            case (HType.TargetWeakestEnemy):
                return typeof(H_TargetWeakestEnemy);
            case (HType.TargetNearestAlly):
                return typeof(H_TargetNearestAlly);
            case (HType.TargetWeakestAlly):
                return typeof(H_TargetWeakestAlly);
            case (HType.Attack):
                return typeof(H_Attack);
            case (HType.Mutilate):
                return typeof(H_Mutilate);
            case (HType.Explode):
                return typeof(H_Explode);
            case (HType.Heal):
                return typeof(H_Heal);
            case (HType.WatchForEnemy):
                return typeof(H_WatchForEnemy);
            case (HType.Move):
                return typeof(H_Move);
        }



        return null;
    }
}
