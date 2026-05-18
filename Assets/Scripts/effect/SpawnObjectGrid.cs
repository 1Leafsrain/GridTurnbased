using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectGrid : PlainEffect
{
    public int damageBonus;
    public GameObject prefab;

    public override void OnBattle(GameObject target, GameObject Doers, int value, Enum @enum)
    {
       Transform spawnPoint = target.transform; 
       SpawnObjectAtPosition(spawnPoint.position);
    }

    public void SpawnObjectAtPosition(Vector2 position)
    {
        UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
        Debug.Log("Spawned object at position: " + position);
    }
}
