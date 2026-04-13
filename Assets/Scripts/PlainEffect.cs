using System;
using UnityEngine;

[System.Serializable]

public abstract class PlainEffect
{
    public virtual void OnBeforeBattle(GameObject target) { }
    public virtual void OnBattle(GameObject target, int value, Enum @enum) { }
    public virtual void OnAfterBattle(GameObject target, int turn) { }
}
