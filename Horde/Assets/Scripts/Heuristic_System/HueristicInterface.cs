using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HInterface : MonoBehaviour
{

    public enum HType
    {
        SeekNearestEnemy,
        Attack,
        Explode,
        RangedAttack, // Remove
        SeekRangedEnemy,
        SeekMeleeEnemy,
        SeekWeakestEnemy,
        SeekNearestAlly,
        SeekWeakestAlly,
        Heal,
        Mutilate,
        TargetNearestEnemy
    }

    public static System.Type GetHeuristic(HType heuristic)
    {
        switch (heuristic)
        {
            case (HType.SeekNearestEnemy):
                return typeof(H_SeekNearestEnemy);
            case (HType.Attack):
                return typeof(H_Attack);
            case (HType.Explode):
                return typeof(H_Explode);
            case (HType.RangedAttack):
                return typeof(H_AttackRanged);
            case (HType.SeekNearestAlly):
                return typeof(H_SeekNearestAlly);
            case (HType.SeekWeakestEnemy):
                return typeof(H_SeekWeakestEnemy);
            case (HType.Heal):
                return typeof(H_Heal);
            case (HType.SeekWeakestAlly):
                return typeof(H_SeekWeakestAlly);
            case (HType.Mutilate):
                return typeof(H_Mutilate);
            case (HType.TargetNearestEnemy):
                return typeof(H_TargetNearestEnemy);
        }



        return null;
    }
}
