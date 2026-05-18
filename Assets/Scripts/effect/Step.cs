using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : PlainEffect
{
    Vector2Int gridEnemy;
    Vector2Int gridPlayer;

    Vector3 gridPlayers;
    Vector3 gridEnemys;

    public GameObject player;

    public override void OnBattle(GameObject target, GameObject Doers, int value, Enum @enum)
    {
        player = Doers;
        gridEnemy = GenerateGridTile.WorldToGrid(target.transform.position);
        gridPlayer = GenerateGridTile.WorldToGrid(Doers.transform.position);
        GetDirection(Doers);
    }

    public void GetDirection(GameObject mover)
    {
        GridMove gridMove = mover.GetComponent<GridMove>();
        if (gridMove == null) 
        {
            Debug.LogError("GridMove component not found on mover!");
            return;
        }

        if (gridEnemy.x > gridPlayer.x) { gridMove.TryMove(new Vector3(1f, 0f, 0f));
            Debug.LogError("BELOKKKKK ");
        }
        else if (gridEnemy.x < gridPlayer.x) { gridMove.TryMove(new Vector3(-1f, 0f, 0f));
            Debug.LogError("BELOKKKKK ");
        }
        else if (gridEnemy.y > gridPlayer.y) { gridMove.TryMove(new Vector3(0f, 1f, 0f));
            Debug.LogError("BELOKKKKK ");
        }
        else { gridMove.TryMove(new Vector3(0f, -1f, 0f));
            Debug.LogError("BELOKKKKK ");
        }
    }
    
}