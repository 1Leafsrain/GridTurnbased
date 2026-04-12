using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject Above;
    Vector2 Vectors2 ;
    // Start is called before the first frame update
    void Start()
    {
        Vectors2 = this.gameObject.transform.position;
        Collider2D hit = Physics2D.OverlapPoint(Vectors2);
        if (hit != null)
        {
            if (hit.gameObject.CompareTag("EnemyHand"))
            {
                Above = hit.gameObject;
            }
        }
        
            
    }

    public GameObject GetAbove()
    {
        return Above;
    }
}
