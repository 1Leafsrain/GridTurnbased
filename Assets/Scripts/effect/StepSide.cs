using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StepSide : PlainEffect
{
    


    Vector3 gridKanan;
    Vector3 gridKiri;

    
    public override void OnBattle(GameObject target, GameObject Doers, int value, Enum @enum)
    {
        var random = Random.Range(0,1);
        GridMove gridMove = Doers.GetComponent<GridMove>() ?? Doers.GetComponentInParent<GridMove>();
        //gridPlayers = Doers.transform.position;

        AigridMove ai = target.GetComponent<AigridMove>() ?? target.GetComponentInParent<AigridMove>();
        if (ai == null)
        {
            Debug.LogWarning("BackDamage: AigridMove not found on target or its parents. Dealing normal damage.");
            
            return;
        }
        gridKanan = ai.BagianKanan;
        gridKiri = ai.BagianKiri;

        if (random == 0)
        {
            gridMove.JustMove(gridKanan);
            Debug.Log("StepSide: Moved to the right side of the target.");
        }
        else if (random == 1)
        {
            gridMove.JustMove(gridKiri);
            Debug.Log("StepSide: Moved to the left side of the target.");
        }




    }
}