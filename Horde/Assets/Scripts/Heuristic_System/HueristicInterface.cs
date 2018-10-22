using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HInterface : MonoBehaviour
{

    public enum HType
    {
        TargetNearestEnemy,
        Attack,
        Explode,
        RangedAttack, // Remove
        TargetRangedEnemy,
        TargetMeleeEnemy,
        TargetWeakestEnemy,
        TargetNearestAlly,
        TargetWeakestAlly,
        Heal,
        Mutilate,

    }

    public static System.Type GetHeuristic(HType heuristic)
    {
        switch (heuristic)
        {
            case (HType.TargetNearestEnemy):
                return typeof(H_TargetNearestEnemy);
            case (HType.Attack):
                return typeof(H_Attack);
            case (HType.Explode):
                return typeof(H_Explode);
            case (HType.RangedAttack):
                return typeof(H_AttackRanged);
            case (HType.TargetNearestAlly):
                return typeof(H_TargetNearestAlly);
            case (HType.TargetWeakestEnemy):
                return typeof(H_TargetWeakestEnemy);
            case (HType.Heal):
                return typeof(H_Heal);
            case (HType.TargetWeakestAlly):
                return typeof(H_TargetWeakestAlly);
            case (HType.Mutilate):
                return typeof(H_Mutilate);
        }



        return null;
    }
}
