using UnityEngine;

[System.Serializable]

public abstract class PlainEffect
{
    public virtual void OnBeforeBattle(GameObject target) { }
    public virtual void OnBattle(GameObject target, int value) { }
    public virtual void OnAfterBattle(GameObject target, int turn) { }
}
